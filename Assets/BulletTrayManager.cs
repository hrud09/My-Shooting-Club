using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTrayManager : MonoBehaviour
{
    public GameObject bulletTrayPrefab;
    public List<BulletTray> SpawnedBulletTrays;
    public BulletTray currentFreeBulletTray;
    public Transform[] bulletTrayPositions;
    public BulletSpawnManager bulletSpawnManager;
    // Update is called once per frame
    void Awake()
    {
        bulletSpawnManager.bulletCapacityLeft = currentFreeBulletTray.bulletCapacity;
    }

    public void TrackFreeBulletTrays(BulletTray filledTray)
    {
        currentFreeBulletTray = null;

        filledTray.transform.DOMove(bulletTrayPositions[1].position, 2f).SetDelay(1).OnComplete(() =>
        {

            GameObject tray = Instantiate(bulletTrayPrefab, bulletTrayPositions[0]);
            currentFreeBulletTray = tray.GetComponent<BulletTray>();
            SpawnedBulletTrays.Add(currentFreeBulletTray);
            tray.transform.GetChild(0).localScale = Vector3.zero;
            tray.SetActive(true);
            tray.transform.GetChild(0).DOScale(Vector3.one * 1.6f, 0.3f).OnComplete(() =>
            {
                bulletSpawnManager.bulletCapacityLeft = currentFreeBulletTray.bulletCapacity;
                bulletSpawnManager.unpackAreaManager.CheckForUnpacking();
            });
        });
    }

    void Reposition()
    {
        if (SpawnedBulletTrays.Count > 0)
        {
            for (int i = 0; i < disposedPackages.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / disposPoses.Length);
                Vector3 newPosition = disposPoses[i % disposPoses.Length].position + Vector3.up * spawnedItemGap * stackHeight;
                disposedPackages[i].transform.position = newPosition;
            }
        }
    }
}
