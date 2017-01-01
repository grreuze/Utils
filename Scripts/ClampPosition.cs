using UnityEngine;

public class ClampPosition : MonoBehaviour {

    [SerializeField]
    Vector3 min = Vector3.zero, max = Vector3.zero;
    [SerializeField]
    bool useLocalPosition = false, drawGizmo = true;
    
    Transform _transform;

    void Awake() {
        _transform = transform;
    }

    Vector3 position {
        set {
            if (useLocalPosition) _transform.localPosition = value;
            else _transform.position = value;
        }
    }

    void LateUpdate() {
        float x = Mathf.Clamp(_transform.position.x, min.x, max.x);
        float y = Mathf.Clamp(_transform.position.y, min.y, max.y);
        float z = Mathf.Clamp(_transform.position.z, min.z, max.z);

        position = new Vector3(x, y, z);
    }

    void OnDrawGizmosSelected() {
        if (drawGizmo) {
            Gizmos.color = Color.cyan;

            Vector3 wireCubePosition = (max + min) / 2;
            Vector3 wireCubeSize = max - min;

            Gizmos.DrawWireCube(wireCubePosition, wireCubeSize);
        }
    }
}
