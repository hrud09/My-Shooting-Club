using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletStackManager : MonoBehaviour
{

    public UnpackAreaManager unpackAreaManager;
    public CollectionAreaInfo collectionAreaInfo;
    public Transform[] bulletMovementPos;

    public void GenerateBullet(int bulletCount)
    {
        StartCoroutine(GenerateBulletsDelay(bulletCount));
    }

    private IEnumerator GenerateBulletsDelay(int bulletCount) {

        for (int i = 0; i < bulletCount; i++)
        {
            yield return new WaitForSeconds(collectionAreaInfo.timeToSpawn);
            GameObject bullet = Instantiate(collectionAreaInfo.itemPrefab, bulletMovementPos[0].position, Quaternion.identity);
            collectionAreaInfo.generatedItems.Add(bullet);
            MoveObject(bullet.transform);
        }

      
    }

    void MoveObject(Transform bullet) {

        bullet.DOMove(bulletMovementPos[1].position, 0.5f).OnComplete(() => {

            bullet.GetComponent<Rigidbody>().isKinematic = false;
            unpackAreaManager.CheckForUnpacking();
        });
    }
}
