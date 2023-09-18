using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BulletTrayManager : MonoBehaviour
{
    public ItemTypes itemType;
    public Transform bulletParent;
    public Transform[] bulletStackPositions;
    public List<GameObject> storedBullets;
    public float spawnedItemGap;
    public void StoreBullet(GameObject _bullet)
    {
        StartCoroutine(StoreBulletDelay(_bullet));
    }
    // Update is called once per frame

    IEnumerator StoreBulletDelay(GameObject bullet)
    {
        Reposition();
        float stackHeight = Mathf.Floor(storedBullets.Count / bulletStackPositions.Length);
        Vector3 newPosition = bulletStackPositions[storedBullets.Count % bulletStackPositions.Length].position + Vector3.up * spawnedItemGap * stackHeight;
        yield return new WaitForSeconds(1f);
        bullet.GetComponent<Rigidbody>().isKinematic = true;
        bullet.transform.DOJump(newPosition, 3, 1, 0.5f).OnComplete(()=> {

            storedBullets.Add(bullet);
            Reposition();
        });
    }

    void Reposition()
    {
        if (storedBullets.Count > 0)
        {
            for (int i = 0; i < storedBullets.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / bulletStackPositions.Length);
                Vector3 newPosition = bulletStackPositions[i % bulletStackPositions.Length].position + Vector3.up * spawnedItemGap * stackHeight;
                storedBullets[i].transform.parent = bulletStackPositions[i % bulletStackPositions.Length];
                storedBullets[i].transform.localRotation = Quaternion.identity;
              
               storedBullets[i].transform.position = newPosition;
            }
        }
    }

    public GameObject GetBullet()
    {
        if (storedBullets.Count > 0)
        {
            int lastIndex = storedBullets.Count - 1;
            GameObject lastItem = storedBullets[lastIndex];
            storedBullets.RemoveAt(lastIndex);
            Reposition();
            return lastItem;
        }
        else
        {
            return null;
        }
    }
}
