using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        // The target (player) that the camera should follow.
    public float smoothSpeed = 0.125f;  // The smoothing speed for camera movement.
    Vector3 offset;

    private void Start()
    {
        // Calculate the initial offset between the camera and the target.
        offset = transform.position - target.position;
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the desired position of the camera.
            Vector3 desiredPosition = target.position + offset;

            // Use Vector3.Lerp to smoothly move the camera towards the desired position.
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position.
            transform.position = smoothedPosition;
        }
    }
}
