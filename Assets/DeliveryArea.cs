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
   
    private int currentItemIndex = 0;
    private CollectingArea collectingArea;

    private void Awake()
    {
        collectingArea = GetComponent<CollectingArea>();
    }
    void Start()
    {
        generatedItems = new List<GameObject>();   
    }
    public void GetDelivery(int packageCount, DeliveryVan van)
    {
        StartCoroutine(GenerateItems(packageCount));
        van.RemoveThis();
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
    }
    public void SendAPackageForUnpacking(float delay)
    {
        StartCoroutine(SendAPackageForUnpackingWithDelay(delay));
    }
    IEnumerator SendAPackageForUnpackingWithDelay(float delay)
    {
        if (generatedItems.Count > 0)
        {
            yield return new WaitForSeconds(delay);
            GameObject package = GetAndRemoveLastItem();
            package.transform.DOMove(packageUnpackPos.position, 1).OnComplete(() =>
            {
                collectingArea.GenerateItems(bulletPerPackage);
                Destroy(package);
            });
        }
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
