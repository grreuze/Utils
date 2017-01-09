using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LocalTrailRenderer : MonoBehaviour {
    
    public int vertices = 200;
    
    public Material material {
        get { return line.material; }
        set { line.material = value; }
    }

    public Color startColor {
        get { return line.startColor; }
        set { line.startColor = value; }
    }
    public Color endColor {
        get { return line.endColor; }
        set { line.endColor = value; }
    }

    public float startWidth {
        get { return line.startWidth; }
        set { line.startWidth = value; }
    }
    public float endWidth {
        get { return line.endWidth; }
        set { line.endWidth = value; }
    }
    
    LineRenderer line;
    Vector3[] positions;
    Transform _transform;

    void SetPosition(int index, Vector3 position) {
        positions[index] = position;
        line.SetPosition(index, _transform.InverseTransformPoint(_transform.parent.TransformPoint(position)));
    }

    void Awake() {
        _transform = transform;
        line = GetComponent<LineRenderer>();
        line.numPositions = vertices;
        positions = new Vector3[vertices];
        for (int i = 0; i < vertices; i++)
            SetPosition(i, _transform.localPosition);
        line.useWorldSpace = false;
    }

	void LateUpdate () {
        for (int i = 0; i < vertices - 1; i++)
            SetPosition(i, positions[i + 1]);
        SetPosition(vertices - 1, _transform.localPosition);
    }

    void OnDestroy() {
        Destroy(line);
    }
}
