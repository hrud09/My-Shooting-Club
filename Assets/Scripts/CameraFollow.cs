using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smootSpeed;
    Vector3 offset;
    private void Start()
    {
        offset = transform.position - target.position;
    }
    private void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smootSpeed);
    }
}