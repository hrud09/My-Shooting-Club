using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SofaManager : MonoBehaviour
{

    public Sofa[] allSofa;
    public List<Sofa> unlockedSofas;

    public Transform GetFreeSittingPostion() {

        for (int i = 0; i < unlockedSofas.Count; i++)
        {
            if (unlockedSofas[i].unlockManager.isUnlocked)
            {
                for (int j = 0; j < unlockedSofas[i].sittingPositionsInfo.Length; j++)
                {
                    if (unlockedSofas[i].sittingPositionsInfo[j].isFree)
                    {
                        unlockedSofas[i].sittingPositionsInfo[j].isFree = false;
                        return unlockedSofas[i].sittingPositionsInfo[j].positionTransform; 
                    }
                }
            }
        }

        return null;
    }

    public bool HasFreeSit()
    {
        for (int i = 0; i < unlockedSofas.Count; i++)
        {
            if (unlockedSofas[i].unlockManager.isUnlocked)
            {
                for (int j = 0; j < unlockedSofas[i].sittingPositionsInfo.Length; j++)
                {
                    if (unlockedSofas[i].sittingPositionsInfo[j].isFree) return true;
                }
            }
        }

        return false;
    }
}
[System.Serializable]
public class FreePlaceInfo {


    public Transform positionTransform;
    public bool isFree;
}