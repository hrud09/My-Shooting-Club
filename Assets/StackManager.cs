using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public enum ItemTypes {
    Bullet,
    Package
}
public class StackManager : MonoBehaviour
{
    public static StackManager Instance;

    public bool isStacking;
    public Transform stackPos;
    public float gapOnYAxis;
    public int maxItemCount;
    public float collectionSpeed;
    public ItemTypes currectItemType;
    public List<GameObject> collectedItems;


    public void GetItem(int generatedItemCount, DeliveryAreaManager deliveryArea)
    {
        if (generatedItemCount <= 0 || isStacking || collectedItems.Count >= maxItemCount) return;
        GetComponent<Animator>().SetLayerWeight(1, 1);
        isStacking = true;
        GameObject item = deliveryArea.GetAndRemoveLastItem();
        StartCoroutine(StackItems(item));
    }

    private IEnumerator StackItems(GameObject _item)
    {
        collectedItems.Add(_item);
        _item.transform.parent = null;
        yield return new WaitForSeconds(0.2f);       
        isStacking = false;
    }

    private void LateUpdate()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            collectedItems[i].transform.position = Vector3.Lerp(collectedItems[i].transform.position, stackPos.position + Vector3.up * i * gapOnYAxis, Time.deltaTime * (collectedItems.Count - i) * collectionSpeed);
            collectedItems[i].transform.rotation = Quaternion.Slerp(collectedItems[i].transform.rotation, stackPos.rotation, Time.deltaTime * 1 / (i + 1) * collectionSpeed);
        }
    }

    private void Awake()
    {
        Instance = this;
    }
  
}
[System.Serializable]
public class CollectionAreaInfo{

    public string name;
    public ItemTypes itemType;
    public GameObject itemPrefab;
    public LayerMask collectableLayers;
    public float spawnedItemGap;
    public float timeToSpawn;
    public List<GameObject> generatedItems;
}