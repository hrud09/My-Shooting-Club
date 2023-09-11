using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryStarter : MonoBehaviour
{

    public GameObject deliverProgressObj;
    public Image deliverProressFill;
    public float fillAmount;
    public DeliveryVan deliveryVan;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            deliverProgressObj.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            fillAmount += Time.deltaTime;
            deliverProressFill.fillAmount = fillAmount;
            if (fillAmount>=1)
            {
                if (deliveryVan)
                {
                    deliveryVan.readyToDeliver = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            fillAmount = 0;
        }
    }
}
