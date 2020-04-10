using UnityEngine;
using System;

public class Follow : MonoBehaviour {
    public Transform follow;
    public float smoothTime, maxSpeed;

    private Vector2 currentVelocity;

    private void Update() {
        var newPosition = Vector2.SmoothDamp(transform.position, follow.position, ref currentVelocity, smoothTime, maxSpeed);
        transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
    }
}