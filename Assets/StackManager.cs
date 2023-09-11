using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes { 

    Bullet,
    Package

}
public class StackManager : MonoBehaviour
{
    public static StackManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
