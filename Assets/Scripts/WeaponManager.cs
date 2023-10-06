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

    public GameObject bulletPrefab;
    public List<GameObject> spawnedBullet;
    public Transform reloadArea;

    private Tween weaponPunchScaleTween;
    private void Start()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        shootingRange = GetComponent<ShootingRange>();
        for (int i = 0; i < 10; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, weapon.transform);
            bullet.SetActive(false);
            spawnedBullet.Add(bullet);
        }
    }
    public void Reload(GameObject package)
    {
        if (!package) return;
        if (HasLoadedGun()) return;
        currentWeaponTransform = weapon.gameObject.transform;
        //package.transform.localScale = Vector3.one * 0.7f;

        package.transform.parent = null;

        package.transform.DOJump(reloadArea.position, 2, 1, 0.5f).OnComplete(()=> {

          
            float delay = 0.1f; // Initial delay for the first bullet
            weapon.weaponInfo.currentBulletCount+= 10;
            foreach (var bullet in spawnedBullet)
            {
                
                bullet.transform.position = package.transform.position;
                bullet.SetActive(true);
                bullet.transform.localScale = Vector3.one * 0.7f;

                bullet.transform.DOJump(weapon.weaponInfo.weaponPositionTransform.position, 3, 1, 0.1f)
                    .SetDelay(delay)
                    .OnComplete(() =>
                    {
                        if (weaponPunchScaleTween != null) weaponPunchScaleTween.Kill();
                        bullet.SetActive(false);
                        if (spawnedBullet.IndexOf(bullet) == spawnedBullet.Count - 1)
                        {
                            package.GetComponent<Crate>().Break();
                        }                        
                        weaponPunchScaleTween = weapon.gameObject.transform.DOScale(Vector3.one * 1.01f, 0.1f).OnComplete(()=> {

                            weapon.gameObject.transform.localScale = Vector3.one;
                        });
                    });

                delay += 0.1f; // Increase the delay for the next bullet
            }
            // Destroy(package);
            if (!shootingRange.isOccupied && weapon.weaponInfo.currentBulletCount >= weapon.weaponInfo.bulletCapacity)
            {
                customerManager.SendNextCustomerToShoot();
            }
            isReloading = false;
        });
    }

    public bool HasLoadedGun()
    {
        bool _hasLoadedGun = (weapon.weaponInfo.currentBulletCount == weapon.weaponInfo.bulletCapacity);
        hasLoadedGun = _hasLoadedGun;
        return _hasLoadedGun;
    }

    public void EmptyWeaponAction()
    {
        shootingRange.outOfAmmoSign.transform.localScale = Vector3.zero;
        shootingRange.outOfAmmoSign.SetActive(true);
        shootingRange.outOfAmmoSign.transform.DOScale(Vector3.one, 0.4f);
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
