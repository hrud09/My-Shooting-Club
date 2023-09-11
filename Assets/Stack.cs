using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    public bool isStacking;
    public Transform stackPos;
    public ItemTypes currentStackItemType;
    public List<GameObject> collectedItems;
    public void StackItem(GameObject item)
    {
        if (item.GetComponent<Item>().itemType == currentStackItemType)
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collectedItems.Count>0) return;
        
    }
}
