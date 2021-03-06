using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(ParticleSystem))]
public class FadeParticlesNearPoint : MonoBehaviour {

	ParticleSystem ps;
	[Header("Use \"Infinity\" to ignore an axis")]
	[SerializeField] Vector3 point;
	[SerializeField] float radius;
	[SerializeField] float startFadeDistance = 3;
	[SerializeField] bool killParticleWhenFaded;
	bool isLocal;
	bool ignoreX, ignoreY, ignoreZ;
	Color startColor;

	void Start() {
		ps = GetComponent<ParticleSystem>();
		isLocal = ps.main.simulationSpace == ParticleSystemSimulationSpace.Local;
	}

	void OnValidate() {
		ignoreX = point.x == Mathf.Infinity;
		ignoreY = point.y == Mathf.Infinity;
		ignoreZ = point.z == Mathf.Infinity;
	}

	void LateUpdate() {

		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.main.maxParticles];
		startColor = ps.main.startColor.color;
		int count = ps.GetParticles(particles);

		for (int i = 0; i < count; i++) {
			ParticleSystem.Particle p = particles[i];
			Vector3 pos = isLocal ? transform.TransformPoint(p.position) : p.position;

			float distance = 0;

			if (!ignoreX) distance += (pos.x - point.x) * (pos.x - point.x);
			if (!ignoreY) distance += (pos.y - point.y) * (pos.y - point.y);
			if (!ignoreZ) distance += (pos.z - point.z) * (pos.z - point.z);

			distance = Mathf.Sqrt(distance);

			if (distance < startFadeDistance + radius) {
				Color color = p.startColor;
				color.a = startColor.a * Mathf.InverseLerp(radius, startFadeDistance + radius, distance);
				p.startColor = color;
				if (killParticleWhenFaded && distance < radius)
					p.remainingLifetime = 0;
				particles[i] = p;
			} else {
				p.startColor = startColor;
				particles[i] = p;
			}
		}
		ps.SetParticles(particles, count);
	}
}
