using System.Collections;
using UnityEngine;

public class CameraRig : MonoBehaviour {

    public float speed = 3f;
    public Transform followTarget;
    Transform _transform;

    private void Awake() {
        _transform = transform;
    }

    /// <summary>
    /// Smoothly lerp to follow the target's position
    /// </summary>
    private void Update() {
        if (followTarget)
            _transform.position = Vector3.Lerp(_transform.position, followTarget.position, speed * Time.deltaTime);
    }
}
