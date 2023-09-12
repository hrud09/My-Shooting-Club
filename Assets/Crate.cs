using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crate : MonoBehaviour
{
    private Rigidbody[] rigidbodies;

    private void Awake()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
    }

    public void Break() {

        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
            rigidbodies[i].transform.DOScale(Vector3.zero, 0.1f).SetDelay(1f).OnComplete(()=> {

                Destroy(gameObject);
            
            });
        }
    }
}
