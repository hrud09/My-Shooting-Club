using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Van : MonoBehaviour
{

    public int itemCarried;
    public bool isDeliverable, readyToDeliver;
    public bool delivered;
    public DeliveryVanManager vanManager;
    public DeliveryAreaManager deliveryArea;
    private DeliveryStarter deliveryStarter;

  
    private IEnumerator Start()
    {
        deliveryStarter = vanManager.deliveryStarter;
        itemCarried = Random.Range(5, 10);
        yield return new WaitUntil(()=> Vector3.Distance(transform.position, vanManager.vanStopPos[0].position) <= 1f);
        isDeliverable = true;
        deliveryStarter.deliveryVan = this;
        deliveryArea.deliverySign.SetActive(true);
        yield return new WaitUntil(() => readyToDeliver);
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
