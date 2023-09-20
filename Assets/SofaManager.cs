using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaManager : MonoBehaviour
{
    public Sofa[] sofas;

    public Transform GetFreeSittingPostion() {

        for (int i = 0; i < sofas.Length; i++)
        {
            if (sofas[i].isUnlocked)
            {
                for (int j = 0; j < sofas[i].sittingPositionsInfo.Length; j++)
                {
                    if (sofas[i].sittingPositionsInfo[j].isFree)
                    {
                        sofas[i].sittingPositionsInfo[j].isFree = false;
                        return sofas[i].sittingPositionsInfo[j].positionTransform; 
                    }
                }
            }
        }

        return null;
    }

    public bool HasFreeSit()
    {
        for (int i = 0; i < sofas.Length; i++)
        {
            if (sofas[i].isUnlocked)
            {
                for (int j = 0; j < sofas[i].sittingPositionsInfo.Length; j++)
                {
                    if (sofas[i].sittingPositionsInfo[j].isFree) return true;
                }
            }
        }

        return false;
    }
}
[System.Serializable]
public class FreePlaceInfo {

    public string sofaID;
    public Transform positionTransform;
    public bool isFree;
}