using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes {
    Bullet,
    Package
}
public class StackManager : MonoBehaviour
{
    public bool isStacking;
    public Transform stackPos;
    public float gapOnYAxis;
    public int maxItemCount;
    public float collectionSpeed;
    public ItemTypes collectedItemType;
    public List<GameObject> collectedItems;

    private void Update()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            Vector3 initPos = collectedItems[i].transform.position;
            initPos = stackPos.position + Vector3.up * i * gapOnYAxis;
            collectedItems[i].transform.position = initPos;
            collectedItems[i].transform.rotation = Quaternion.Slerp(collectedItems[i].transform.rotation, stackPos.rotation, Time.deltaTime * 1 / (i + 1) * collectionSpeed);
        }
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            DeliveryAreaManager deliveryAreaManager = other.gameObject.GetComponentInParent<DeliveryAreaManager>();
            if (collectedItems.Count < maxItemCount && !isStacking)
            {
                if (collectedItems.Count == 0)
                {
                    collectedItemType = deliveryAreaManager.collectionAreaInfo.itemType;
                }
                if (collectedItemType == deliveryAreaManager.collectionAreaInfo.itemType)
                {
                    print("7");
                    GetItem(deliveryAreaManager.collectionAreaInfo.generatedItems.Count, deliveryAreaManager.GetAndRemoveLastItem());
                }
            }
        }
        else if (other.gameObject.layer == 8)
        {
            print("8");
            UnpackAreaManager unpackAreaManager = other.gameObject.GetComponentInParent<UnpackAreaManager>();
            if (collectedItems.Count > 0 && unpackAreaManager.IsInDisposableCondition())
            {
                GameObject g = collectedItems[collectedItems.Count - 1];
                collectedItems.Remove(g);
                unpackAreaManager.DisposeAPackage(g);
            }
           
        }
    }

    public void GetItem(int generatedItemCount, GameObject deliverredPackage)
    {
        if (generatedItemCount <= 0) return;
        GetComponent<Animator>().SetLayerWeight(1, 1);
        isStacking = true;
        GameObject item = deliverredPackage;
        StartCoroutine(StackItems(item));
    }

    private IEnumerator StackItems(GameObject _item)
    {
        collectedItems.Add(_item);
        _item.transform.parent = null;
        yield return new WaitForSeconds(0.2f);
        isStacking = false;
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