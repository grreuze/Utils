using UnityEngine;

public class FollowTarget : MonoBehaviour {

    /// <summary>
    /// The transform the FollowTarget object is following.
    /// </summary>
    public Transform target;
    /// <summary>
    /// The distance between the FollowTarget object and its target.
    /// </summary>
    [HideInInspector]
    public Vector3 distance;
    Transform _transform;

	void Start () {
        _transform = transform;
        distance = _transform.position - target.position;
	}
	
	void Update () {
        _transform.position = target.position + distance;
	}
}
