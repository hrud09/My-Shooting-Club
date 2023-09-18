using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == 10) Destroy(collision.collider.gameObject);
    }
}
