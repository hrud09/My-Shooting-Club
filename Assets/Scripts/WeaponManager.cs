using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponManager : MonoBehaviour
{
    public Weapon weapon;
    public Transform currentWeaponTransform;
    public bool isReloading;
    public CustomerManager customerManager;
    ShootingRange shootingRange;
    public bool hasLoadedGun;
    private void Start()
    {
        shootingRange = GetComponent<ShootingRange>();
    }
    public void CheckWeaponReload(GameObject bullet)
    {
        if (!bullet) return;
        if (HasLoadedGun()) return;
        currentWeaponTransform = weapon.gameObject.transform;
        bullet.transform.localScale = Vector3.one * 0.7f;
        bullet.transform.DOJump(currentWeaponTransform.position, 3, 1, 0.1f).OnComplete(() =>
        {
            weapon.weaponInfo.currentBulletCount++;
            if (!shootingRange.isOccupied && weapon.weaponInfo.currentBulletCount >= weapon.weaponInfo.bulletCapacity) customerManager.SendNextCustomerToShoot();
            weapon.gameObject.transform.DOPunchScale(Vector3.one * 1.02f,0.1f, 5);
            Destroy(bullet);
            isReloading = false;
        });

    }

    public bool HasLoadedGun()
    {
        bool _hasLoadedGun = (weapon.weaponInfo.currentBulletCount == weapon.weaponInfo.bulletCapacity);
        hasLoadedGun = _hasLoadedGun;
        return _hasLoadedGun;
    }
}

[System.Serializable]
public class WeaponInfo
{

    public string ID;
    public Transform weaponPositionTransform; 
    public int bulletCapacity;
    public int currentBulletCount;

}
