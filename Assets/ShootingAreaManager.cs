using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAreaManager : MonoBehaviour
{

    public ShootingRange[] shootingRanges;
   
    public bool HasFreeShootingRange()
    {
        for (int i = 0; i < shootingRanges.Length; i++)
        {
            if (shootingRanges[i].isUnlocked && !shootingRanges[i].isOccupied && !shootingRanges[i].isOutOfService)
            {
                return true;
            }
        }
        return false;
    }

    public Transform GetFreeShootinRange()
    {
        for (int i = 0; i < shootingRanges.Length; i++)
        {
            if (shootingRanges[i].isUnlocked && !shootingRanges[i].isOccupied && !shootingRanges[i].isOutOfService)
            {
                shootingRanges[i].isOccupied = true;
                return shootingRanges[i].shootingSpot;
            }
        }
        return null;
    }
}
