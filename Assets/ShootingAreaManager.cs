using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAreaManager : MonoBehaviour
{

    public ShootingRange[] allShootingRanges;
    [SerializeField]
    public List<ShootingRange> unlockedShootingRanges;
    [SerializeField]
    public List<ShootingRange> lockedShootingRanges;


    private void Start()
    {
        CheckForUnlock();
    }
    public bool HasFreeShootingRange()
    {
        for (int i = 0; i < unlockedShootingRanges.Count; i++)
        {
            if (unlockedShootingRanges[i].IsFreeAndUsable())
            {
                return true;
            }
        }
        return false;
    }
   
    public ShootingRange GetFreeShootinRange()
    {
        for (int i = 0; i < unlockedShootingRanges.Count; i++)
        {
            if (unlockedShootingRanges[i].IsFreeAndUsable())
            {
                unlockedShootingRanges[i].isOccupied = true;
                return unlockedShootingRanges[i];
            }
        }
        return null;
    }

    public void CheckForUnlock()
    {
        if (lockedShootingRanges.Count > 0)
        {
            lockedShootingRanges[0].gameObject.transform.parent.gameObject.SetActive(true);
        }
        for (int i = 1; i < lockedShootingRanges.Count; i++)
        {
            lockedShootingRanges[i].gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
