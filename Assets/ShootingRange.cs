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


    private void Update()
    {
        if (!weaponManager.HasLoadedGun() && !outOfAmmoSign.activeInHierarchy)
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

        if (weaponManager.HasLoadedGun()) print("Loaded Gun Found");
    }
}
