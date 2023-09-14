using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryStartCollider : MonoBehaviour
{

    public GameObject deliverProgressObj;
    public Image deliverProressFill;
    public float fillAmount;
    public Van deliveryVan;
    public GameObject deliveryIndicatorArrow;
    public DeliveryVanController deliveryVanManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            deliverProgressObj.SetActive(true);
            deliveryIndicatorArrow.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && deliveryVan)
        {
            fillAmount += Time.deltaTime * deliveryVanManager.deliverySpeedMultplier; ;
            deliverProressFill.fillAmount = fillAmount;
            if (fillAmount>=1)
            {
                if (!deliveryVan.readyToDeliver)
                {
                    deliveryVan.readyToDeliver = true;
                    fillAmount = 0;
                    deliverProressFill.fillAmount = fillAmount;
                    deliveryVan = null;
                }             
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fillAmount = 0;
            deliverProressFill.fillAmount = fillAmount;
            deliverProgressObj.SetActive(false);
            deliveryIndicatorArrow.SetActive(true);
        }
    }
}
