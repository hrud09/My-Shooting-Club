using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crate : MonoBehaviour
{
    private Rigidbody[] rigidbodies;

    private void Awake()
    {
       
       
    }

    public void Break() {

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject g =  transform.GetChild(i).gameObject;
            g.AddComponent<Rigidbody>();
            MeshCollider m =  g.AddComponent<MeshCollider>();
            m.convex = true;
        }
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).DOScale(Vector3.zero, 0.3f).OnComplete(()=> {

                if (i == transform.childCount - 1) Destroy(gameObject);
            
            });
        }
    }
}
