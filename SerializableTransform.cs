using UnityEngine;
using System;

[Serializable]
public struct TransformSerial {
    public Vector3Serial positionS;
    public QuaternionSerial rotationS;
    public Vector3Serial localScaleS;

    public void Fill(Transform t) {
        positionS.Fill(t.position);
        rotationS.Fill(t.rotation);
        localScaleS.Fill(t.localScale);
    }

    public Vector3 position {
        get { return positionS.V3; }
        set { positionS.Fill(value); }
    }
    public Quaternion rotation {
        get { return rotationS.Q; }
        set { rotationS.Fill(value); }
    }
    public Vector3 localScale {
        get { return localScaleS.V3; }
        set { localScaleS.Fill(value); }
    }

    public Transform T {
        get {
            T.position = positionS.V3;
            T.rotation = rotationS.Q;
            T.localScale = localScaleS.V3;
            return T;
        }
    }
}

[Serializable]
public struct Vector3Serial {
    public float x;
    public float y;
    public float z;

    public void Fill(Vector3 v3) {
        x = v3.x;
        y = v3.y;
        z = v3.z;
    }

    public Vector3 V3 { get { return new Vector3(x, y, z); } }
}

[Serializable]
public struct QuaternionSerial {
    public float x;
    public float y;
    public float z;
    public float w;

    public void Fill(Quaternion q) {
        x = q.x;
        y = q.y;
        z = q.z;
        w = q.w;
    }

    public Quaternion Q { get { return new Quaternion(x, y, z, w); } }
}
