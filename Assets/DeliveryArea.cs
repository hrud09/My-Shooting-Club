using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeliveryArea : MonoBehaviour
{
    public float timeToSpawn;
    public GameObject generateItemPrefab;
    public int bulletPerPackage;
    public List<GameObject> generatedItems;
    public float spawnedItemGap = 0.3f;
    public Transform[] packageStackPos; // Set this to have four positions
    public Transform packageUnpackPos;
    public CollectingArea collectingArea;
    private int currentItemIndex = 0;

    void Start()
    {
        generatedItems = new List<GameObject>();
        GetDelivery(4);
    }
    public void GetDelivery(int packageCount)
    {
        StartCoroutine(GenerateItems(packageCount));
        SendAPackageForUnpacking();
    }


    public void SendAPackageForUnpacking()
    {
        if (generatedItems.Count <= 0) return;
        GameObject package = GetAndRemoveLastItem();
        package.transform.DOMove(packageUnpackPos.position, 1).OnComplete(() =>
        {
            collectingArea.GenerateItems(bulletPerPackage);           
            Destroy(package);
        });
        
    }
    IEnumerator GenerateItems(int _packageCount)
    {
        while (_packageCount > 0)
        {
            _packageCount--;
            GameObject newItem = Instantiate(generateItemPrefab, packageStackPos[currentItemIndex].position, Quaternion.identity);
            newItem.transform.SetParent(packageStackPos[currentItemIndex]);
            //newItem.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            generatedItems.Add(newItem);
            Reposition();
            yield return new WaitForSeconds(timeToSpawn);
            currentItemIndex = (currentItemIndex + 1) % packageStackPos.Length;

        }
        Reposition();
    }
    void Reposition()
    {
        if (generatedItems.Count > 0)
        {
            for (int i = 0; i < generatedItems.Count; i++)
            {
                float stackHeight = Mathf.Floor(i / packageStackPos.Length);
                Vector3 newPosition = packageStackPos[i % packageStackPos.Length].position + Vector3.up * spawnedItemGap * stackHeight;
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
