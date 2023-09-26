using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAreaManager : MonoBehaviour
{

    public ShootingRange[] allShootingRanges;
    public List<ShootingRange> unlockedShootingRanges;
   
    public bool HasFreeShootingRange()
    {
        for (int i = 0; i < unlockedShootingRanges.Count; i++)
        {
            if (unlockedShootingRanges[i].isUnlocked && !unlockedShootingRanges[i].isOccupied && !unlockedShootingRanges[i].isOutOfService && unlockedShootingRanges[i].weaponManager.HasLoadedGun())
            {
                return true;
            }
        }

        return false;
    }
   
    public Transform GetFreeShootinRange()
    {
        for (int i = 0; i < unlockedShootingRanges.Count; i++)
        {
            if (unlockedShootingRanges[i].isUnlocked && !unlockedShootingRanges[i].isOccupied && !unlockedShootingRanges[i].isOutOfService && unlockedShootingRanges[i].weaponManager.HasLoadedGun())
            {
                unlockedShootingRanges[i].isOccupied = true;
                return unlockedShootingRanges[i].shootingSpot;
            }
        }
        return null;
    }
}
