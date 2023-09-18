using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WeaponManager : MonoBehaviour
{
    public Weapon[] weapons;
    public Transform currentWeaponTransform;
    public bool isReloading;

    public void CheckWeaponReload(GameObject bullet)
    {
        if (!bullet) return;
        Weapon emptyWeapon = EmptyWeapon();
        if (emptyWeapon == null) return;
        isReloading = true;
        currentWeaponTransform = emptyWeapon.gameObject.transform;
        bullet.transform.localScale = Vector3.one * 0.7f;
        bullet.transform.DOJump(currentWeaponTransform.position, 3, 1, 0.3f).OnComplete(() =>
        {
            emptyWeapon.weaponInfo.currentBulletCount++;
            emptyWeapon.gameObject.transform.DOPunchScale(Vector3.one*1.02f,0.1f, 10);
            Destroy(bullet);
            isReloading = false;
        });

    }

    private Weapon EmptyWeapon()
    {
        foreach (Weapon item in weapons)
        {
            if (item.weaponInfo.bulletCapacity > item.weaponInfo.currentBulletCount && !item.isInUse)
            {
                return item;
            }
        }
        return null;
    }
}

[System.Serializable]
public class WeaponInfo
{

    public string ID;
    public GameObject weaponVisual; 
    public int bulletCapacity;
    public int currentBulletCount;

}
