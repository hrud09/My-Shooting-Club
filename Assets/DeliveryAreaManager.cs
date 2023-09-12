using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeliveryAreaManager : MonoBehaviour
{

    public int bulletPerPackage;
    public GameObject deliverySign;
    public Transform[] packageStackPos;
    public CollectionAreaInfo collectionAreaInfo;

    private int currentItemIndex = 0;

    void Start()
    {
        collectionAreaInfo.generatedItems = new List<GameObject>();   
    }
    public void GetDelivery(int packageCount, Van van)
    {
        StartCoroutine(GenerateItems(packageCount, van));  
    }
    IEnumerator GenerateItems(int _packageCount, Van van)
    {
        int deliverredAmount = 0;
        while (_packageCount > 0)
        {
            _packageCount--;
            deliverredAmount++;
            GameObject newItem = Instantiate(collectionAreaInfo.itemPrefab, packageStackPos[currentItemIndex].position, Quaternion.identity);
            newItem.transform.SetParent(packageStackPos[currentItemIndex]);
            collectionAreaInfo.generatedItems.Add(newItem);
            Reposition();
            yield return new WaitForSeconds(collectionAreaInfo.timeToSpawn);
            currentItemIndex = (currentItemIndex + 1) % packageStackPos.Length;
        }

        if (deliverredAmount == van.itemCarried) van.delivered = true;
        yield return new WaitUntil(() => van.delivered);
        van.RemoveThis();
    }

    void Reposition()
    {
        if (collectionAreaInfo.generatedItems.Count > 0)
        {
            for (int i = 0; i < collectionAreaInfo.generatedItems.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / packageStackPos.Length);
                Vector3 newPosition = packageStackPos[i % packageStackPos.Length].position + Vector3.up * collectionAreaInfo.spawnedItemGap * stackHeight;
                collectionAreaInfo.generatedItems[i].transform.position = newPosition;
            }
        }
    }

    public GameObject GetAndRemoveLastItem()
    {
        if (collectionAreaInfo.generatedItems.Count > 0)
        {
            int lastIndex = collectionAreaInfo.generatedItems.Count - 1;
            GameObject lastItem = collectionAreaInfo.generatedItems[lastIndex];
            collectionAreaInfo.generatedItems.RemoveAt(lastIndex);
            Reposition();
            return lastItem;
        }
        else
        {
            return null;
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            print("Hello");
            StackManager stackManager = other.gameObject.GetComponent<StackManager>();

            if (stackManager.collectedItems.Count == 0)
            {
                stackManager.currectItemType = collectionAreaInfo.itemType;
            }          
             if (stackManager.currectItemType == collectionAreaInfo.itemType)
            {
                stackManager.GetItem(collectionAreaInfo.generatedItems.Count, this);
            }

        }
    }
}
