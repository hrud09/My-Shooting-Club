using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShootingRange : MonoBehaviour
{


    public bool isUnlocked;
    public bool isOccupied;
    public bool isOutOfService;
    public Transform shootingSpot;
    public WeaponManager weaponManager;
    public float yRotationWhileShooting;
    public GameObject outOfAmmoSign;
    public ShootingAreaManager shootingAreaManager;

    public GameObject[] sideWalls;

    private void Start()
    {
        if (transform.position.x > 0) yRotationWhileShooting = 90;
        else yRotationWhileShooting = -90;

        outOfAmmoSign.transform.localScale = Vector3.zero;
        outOfAmmoSign.SetActive(true);
        outOfAmmoSign.transform.DOScale(Vector3.one, 0.4f);
    }


    public void OnNewUnlock()
    {
        shootingAreaManager = GetComponentInParent<ShootingAreaManager>();
        isUnlocked = true;
        shootingAreaManager.unlockedShootingRanges.Add(this);
        shootingAreaManager.lockedShootingRanges.Remove(this);
        shootingAreaManager.CheckForUnlock();
    }

    public void OnStartUnlock()
    {
        shootingAreaManager = GetComponentInParent<ShootingAreaManager>();
        isUnlocked = true;
        shootingAreaManager.unlockedShootingRanges.Add(this);
        shootingAreaManager.lockedShootingRanges.Remove(this);
    }
    private void Update()
    {
        if (!weaponManager.HasLoadedGun() && !outOfAmmoSign.activeInHierarchy && !isOccupied )
        {
            outOfAmmoSign.transform.localScale = Vector3.zero;
            outOfAmmoSign.SetActive(true);
            outOfAmmoSign.transform.DOScale(Vector3.one, 0.4f);
        }
        else if (weaponManager.HasLoadedGun() && outOfAmmoSign.activeInHierarchy)
        {
            
            outOfAmmoSign.transform.DOScale(Vector3.zero, 0.4f).OnComplete(()=> {

                outOfAmmoSign.SetActive(false);
            });


        }
    }

    public bool IsFreeAndUsable()
    {
        return isUnlocked && !isOccupied && !isOutOfService && weaponManager.HasLoadedGun();
    }
}
