using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeliveryVan : MonoBehaviour
{

    public int itemCarried;
    public bool isDeliverable, readyToDeliver;
    public bool delivered;
    public DeliveryVanManager vanManager;
    public DeliveryArea deliveryArea;
    private IEnumerator Start()
    {
        itemCarried = Random.Range(10, 30);
        yield return new WaitUntil(()=> Vector3.Distance(transform.position, vanManager.vanStopPos[0].position) <= 1f);
        isDeliverable = true;
        deliveryArea.deliverySign.SetActive(true);
        yield return new WaitUntil(() => readyToDeliver);
        yield return new WaitForSeconds(4);
        deliveryArea.GetDelivery(itemCarried, this);
    }
   
    public void RemoveThis()
    {
        vanManager.loadedVans.Remove(gameObject);
        transform.DOMoveZ(-100, 5).OnComplete(()=> {

            gameObject.SetActive(false);
        
        });
    }
}
