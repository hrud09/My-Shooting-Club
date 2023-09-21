using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public enum ItemTypes
{
    None,
    Bullet,
    Package
}

[System.Serializable]
public class ItemTypeData
{
    public ItemTypes itemType;
    public float gapOnYAxis;
    public int maxCapacity;
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

public class StackManager : MonoBehaviour
{
    public bool isStacking;
    public Transform stackPos;
    public int maxItemCount;
    public float collectionSpeed;
    public ItemTypes collectedItemType;
    public List<GameObject> collectedItems;
    public float stackTime;

    private Animator animator;
    public GameObject filleImageGameobject;
    public Image fillImage;
    public Transform bulletTrayPos;

    [SerializeField] DeliveryAreaManager deliveryAreaManager;
    [SerializeField] UnpackAreaManager unpackAreaManager;
    [SerializeField] BulletTrayManager trayManager;
    [SerializeField] WeaponManager weaponManager;
    public GameObject bulletTray;

    [SerializeField]
    private List<ItemTypeData> itemTypeData; // List of item types and their respective gap values

    [SerializeField]
    private List<CollectionAreaInfo> collectionAreaInfo; // List of collection area info

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        for (int i = 0; i < collectedItems.Count; i++)
        {
            Vector3 initPos = stackPos.position + Vector3.up * i * GetGapForItemType(collectedItemType);
            collectedItems[i].transform.position = initPos;
            collectedItems[i].transform.rotation = Quaternion.Slerp(collectedItems[i].transform.rotation, stackPos.rotation, Time.deltaTime * 1 / (i + 1) * collectionSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            deliveryAreaManager = other.gameObject.GetComponentInParent<DeliveryAreaManager>();
            if (IsItemTypeMatch(deliveryAreaManager.collectionAreaInfo.itemType))
            {
                SetMaxItemCount(collectedItemType);
            }
        }
        else if (other.gameObject.layer == 8)
        {
            unpackAreaManager = other.gameObject.GetComponentInParent<UnpackAreaManager>();
        }
        else if (other.gameObject.layer == 9)
        {
            trayManager = other.GetComponentInParent<BulletTrayManager>();
            if (IsItemTypeMatch(trayManager.itemType))
            {
                SetMaxItemCount(trayManager.itemType);
                bulletTray.SetActive(true);
                animator.SetLayerWeight(1, 1);
            }
        }
        else if (other.gameObject.layer == 11)
        {
            weaponManager = other.GetComponentInParent<WeaponManager>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7 && deliveryAreaManager)
        {
            if (collectedItems.Count < maxItemCount && !isStacking)
            {
                GetItem(deliveryAreaManager.GetAndRemoveLastItem());
            }
        }
        else if (other.gameObject.layer == 8 && collectedItemType == ItemTypes.Package)
        {
            if (collectedItems.Count > 0 && unpackAreaManager.IsInDisposableCondition())
            {
                GameObject g = collectedItems[0];
                collectedItems.Remove(g);
                unpackAreaManager.DisposeAPackage(g);
                if (collectedItems.Count == 0)
                {
                    animator.SetLayerWeight(1, 0);
                    collectedItemType = ItemTypes.None;
                }
            }
        }
        else if (other.gameObject.layer == 9)
        {
            if (collectedItems.Count < maxItemCount && !isStacking)
            {
                GetItem(trayManager.GetBullet());
            }
        }
        else if (other.gameObject.layer == 11 && !weaponManager.isReloading && collectedItemType == ItemTypes.Bullet && weaponManager.EmptyWeapon() !=null)
        {
            weaponManager.isReloading = true;
            weaponManager.CheckWeaponReload(GetABullet());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            deliveryAreaManager = null;
        }
        else if (other.gameObject.layer == 8)
        {
            unpackAreaManager = null;
        }
        else if (other.gameObject.layer == 9)
        {
            if (collectedItems.Count <= 0)
            {
                bulletTray.SetActive(false);
                animator.SetLayerWeight(1, 0);
            }
            trayManager = null;
        }
        else if (other.gameObject.layer == 11)
        {
            if (collectedItems.Count <= 0)
            {
                bulletTray.SetActive(false);
                animator.SetLayerWeight(1, 0);
            }
            weaponManager = null;
        }
    }

    public void GetItem(GameObject collectedItem)
    {
        if (collectedItem == null) return;
        animator.SetLayerWeight(1, 1);
        isStacking = true;

        ItemTypes itemType = GetItemType(collectedItem); // Determine the item type

        if (collectedItemType == ItemTypes.None || collectedItemType == itemType)
        {
            collectedItemType = itemType; // Update collectedItemType
            GameObject item = collectedItem;
            StackItems(item);
        }
        else
        {
            Debug.LogWarning("Different item type encountered. Ignoring the item.");
        }
    }

    private float GetGapForItemType(ItemTypes itemType)
    {
        foreach (var data in itemTypeData)
        {
            if (data.itemType == itemType)
            {
                return data.gapOnYAxis;
            }
        }
        return 0.0f; // Default gap when item type is None or unrecognized
    }

    private ItemTypes GetItemType(GameObject item)
    {
        if (item.CompareTag("Bullet"))
        {
            return ItemTypes.Bullet;
        }
        else if (item.CompareTag("Package"))
        {
            return ItemTypes.Package;
        }
        return ItemTypes.None;
    }

    private void StackItems(GameObject _item)
    {
        int _index = collectedItems.Count;
        Vector3 initPos = stackPos.position + Vector3.up * _index * GetGapForItemType(collectedItemType);
        _item.transform.parent = null;
        _item.transform.DOJump(initPos, 2, 1, stackTime).OnComplete(() => {

            collectedItems.Add(_item);
            isStacking = false;
        });
    }

    private bool IsItemTypeMatch(ItemTypes requiredType)
    {
        return collectedItemType == ItemTypes.None || collectedItemType == requiredType;
    }

    private void SetMaxItemCount(ItemTypes itemType)
    {
        foreach (var capacityData in itemTypeData)
        {
            if (capacityData.itemType == itemType)
            {
                maxItemCount = capacityData.maxCapacity;
                break;
            }
        }
    }
    public GameObject GetABullet()
    {
        if (collectedItems.Count > 0)
        {
            int lastIndex = collectedItems.Count - 1;
            GameObject lastItem = collectedItems[lastIndex];
            collectedItems.Remove(lastItem);
            return lastItem;
        }
        else
        {
            collectedItemType = ItemTypes.None;
            bulletTray.SetActive(false);
            animator.SetLayerWeight(1, 0);
            return null;
        }
    }
}
