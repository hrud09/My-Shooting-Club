using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingArea : MonoBehaviour
{
    public ItemTypes itemType;

    public bool unpackingPackage;
    public float timeToSpawn;
    public GameObject generateItemPrefab;
    public List<GameObject> generatedItems;
    public float spawnedItemGap = 0.3f;
    public Transform[] BulletStackPos; // Set this to have four positions
    private int currentItemIndex = 0;
    private DeliveryAreaManager deliveryArea;

    public int maxBulletAmount;

    private void Awake()
    {
        deliveryArea = GetComponent<DeliveryAreaManager>();
    }
    void Start()
    {      
        generatedItems = new List<GameObject>();
        //StartCoroutine(CheckForDelivery(1));
    }
    public void GenerateItems(int bulletsPerPackage)
    {
        if (unpackingPackage) return;
        unpackingPackage = true;
        StartCoroutine(GenerateItemsDelay(bulletsPerPackage));
    }
    IEnumerator GenerateItemsDelay(int _bulletsPerPackage)
    {
        for (int j = 0; j < _bulletsPerPackage; j++)
        {
            GameObject newItem = Instantiate(generateItemPrefab, BulletStackPos[currentItemIndex].position, Quaternion.identity);
            newItem.transform.SetParent(BulletStackPos[currentItemIndex]);
            newItem.transform.DORotate(Vector3.up * 30, 0.2f);
            generatedItems.Add(newItem);
            Reposition();
            yield return new WaitForSeconds(timeToSpawn);
            currentItemIndex = (currentItemIndex + 1) % BulletStackPos.Length;
        }
       
        // Call the CheckForDelivery method after all items are generated
       //StartCoroutine(CheckForDelivery(0));
    }

   /* public IEnumerator CheckForDelivery(float delay)
    {
        unpackingPackage = false;
        yield return new WaitUntil(() => deliveryArea.generatedItems.Count > 0);
       // if (generatedItems.Count < maxBulletAmount) deliveryArea.SendAPackageForUnpacking(delay);

    }
*/
    void Reposition()
    {
        if (generatedItems.Count > 0)
        {
            for (int i = 0; i < generatedItems.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / BulletStackPos.Length);
                Vector3 newPosition = BulletStackPos[i % BulletStackPos.Length].position + Vector3.up * spawnedItemGap * stackHeight;
                generatedItems[i].transform.position = newPosition;
            }
        }
    }

    public GameObject GetAndRemoveLastItem()
    {
        if (generatedItems.Count > 0)
        {
            int lastIndex = generatedItems.Count - 1;
            GameObject lastItem = generatedItems[lastIndex];
            generatedItems.RemoveAt(lastIndex);
            Reposition();
            return lastItem;
        }
        else
        {
            return null; // Return null if the list is empty
        }
    }

}
