using UnityEngine;

public class AutoDestroyParticles : MonoBehaviour {

    void Start() {
        Destroy(gameObject, GetComponent<ParticleSystem>().duration);
    }

}