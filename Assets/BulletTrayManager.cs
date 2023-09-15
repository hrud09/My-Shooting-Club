using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTrayManager : MonoBehaviour
{
    public List<BulletTray> bulletTrays;
    public BulletTray currentFreeBulletTray;
    public Transform[] bulletTrayPositions;
    public List<BulletTray> freeTrays;
    public BulletSpawnManager bulletSpawnManager;
    // Update is called once per frame
    void Awake()
    {
        freeTrays = bulletTrays;
        currentFreeBulletTray = freeTrays[0];
        bulletSpawnManager.bulletCapacityLeft = currentFreeBulletTray.bulletCapacity;
    }

    public void TrackFreeBulletTrays(BulletTray filledTray)
    {
        currentFreeBulletTray = null;
        freeTrays.Remove(filledTray);
        if (freeTrays.Count > 0) {
            filledTray.transform.DOMove(bulletTrayPositions[1].position, 2f).SetDelay(1).OnComplete(() => {

                GameObject tray = freeTrays[0].gameObject;
                tray.transform.GetChild(0).localScale = Vector3.zero;
                tray.SetActive(true);
                tray.transform.GetChild(0).DOScale(Vector3.one * 1.6f, 0.3f).OnComplete(() => {
                    currentFreeBulletTray = freeTrays[0];
                    bulletSpawnManager.bulletCapacityLeft = currentFreeBulletTray.bulletCapacity;
                    bulletSpawnManager.unpackAreaManager.CheckForUnpacking();
                });
            });

        }

    }
}
