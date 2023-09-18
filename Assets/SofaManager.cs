using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaManager : MonoBehaviour
{
    public SofaInfo[] sofaInfos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class SofaInfo {

    public string sofaNo;
    public bool isFree;
    public Transform[] wayPoints;
}