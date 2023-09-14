using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTray : MonoBehaviour
{

    public bool trayFilled;
    public int bulletCapacity;
    public int currentBulletCount;
    public List<GameObject> bulletsStored;
    public GameObject trayCover;
    public GameObject maxSign;

    public void StoreBullet(GameObject bullet)
    {
        if (!trayFilled)
        {
            if (maxSign) maxSign.SetActive(false);
            if (currentBulletCount < bulletCapacity)
            {
                bulletsStored.Add(bullet);
                currentBulletCount++;
                if (currentBulletCount >= bulletCapacity)
                {
                    trayFilled = true;
                    if (maxSign) maxSign.SetActive(true);
                    Vector3 initScale = trayCover.transform.localScale;
                    trayCover.transform.localScale = Vector3.zero;
                    trayCover.SetActive(true);
                    trayCover.transform.DOScale(initScale, 0.2f);
                }
            }
        }
    }
}
