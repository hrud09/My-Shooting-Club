using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

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
    public float stackTime;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            Vector3 initPos = stackPos.position + Vector3.up * i * gapOnYAxis;
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
                if (collectedItemType == deliveryAreaManager.collectionAreaInfo.itemType && !isStacking && deliveryAreaManager.collectionAreaInfo.generatedItems.Count > 0)
                {
                   
                    GetItem(deliveryAreaManager.GetAndRemoveLastItem());
                }
            }
        }
        else if (other.gameObject.layer == 8)
        {
            UnpackAreaManager unpackAreaManager = other.gameObject.GetComponentInParent<UnpackAreaManager>();
            if (collectedItems.Count > 0 && unpackAreaManager.IsInDisposableCondition())
            {
                GameObject g = collectedItems[collectedItems.Count - 1];
                collectedItems.Remove(g);
                if (collectedItems.Count == 0) animator.SetLayerWeight(1, 0);
                unpackAreaManager.DisposeAPackage(g);
            }
           
        }
    }

    public void GetItem(GameObject deliverredPackage)
    {
        animator.SetLayerWeight(1, 1);
        isStacking = true;
        GameObject item = deliverredPackage;
        StackItems(item);
    }

    private void StackItems(GameObject _item)
    {
        int _index = collectedItems.Count;
        Vector3 initPos = stackPos.position + Vector3.up * _index * gapOnYAxis;
        _item.transform.parent = null;
        _item.transform.DOJump(initPos, 2, 1, stackTime).OnComplete(() => {

            collectedItems.Add(_item);
            isStacking = false;
        });
    }

}
[System.Serializable]
public class CollectionAreaInfo
{
    public string name;

    public ItemTypes itemType;
    public int bulletPerPackage_IfPackage;

    public GameObject itemPrefab;
    public float spawnedItemGap;
    public float timeToSpawn;
    public List<GameObject> generatedItems;
}
