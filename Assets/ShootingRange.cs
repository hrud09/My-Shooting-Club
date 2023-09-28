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
    public GameObject targetSheet;
    public GameObject outOfAmmoSign;
    public ShootingAreaManager shootingAreaManager;

    public GameObject[] sideWalls;
    private void Start()
    {
       
        isUnlocked = true;
        shootingAreaManager = GetComponentInParent<ShootingAreaManager>();
        shootingAreaManager.unlockedShootingRanges.Add(this);
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
}
