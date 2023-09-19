using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WelcomeDeskManager : MonoBehaviour
{
    // Start is called before the first frame update
    public CustomerManager customerManager;
    public GameObject customerOverview;
    public TMP_Text customerDesignation;
    public TMP_Text customerName;

    public bool playerOnDesk;
    public Customer currentCustomer;
    public Transform[] exitWays;

    public Button acceptButton;
    public Button rejectButton;

    private void Start()
    {
       
    }
    public void SetCustomerInfoOnUi(Customer customer) {

        currentCustomer = customer;
        rejectButton.onClick.RemoveListener(RejectTicket);
        rejectButton.onClick.AddListener(RejectTicket);
        StartCoroutine(SetCustomerInfoOnUiDelay(customer.customerInfo));
    }
    public IEnumerator SetCustomerInfoOnUiDelay(CustomerInfo customerInfo)
    {
        customerName.text = customerInfo.name;
        customerDesignation.text = customerInfo.designation;
        yield return new WaitUntil(()=> playerOnDesk);
        customerOverview.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerOnDesk = true;
            if (currentCustomer)
            {
                StartCoroutine(SetCustomerInfoOnUiDelay(currentCustomer.customerInfo));
            }
        }   
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {
            playerOnDesk = false;
            customerOverview.SetActive(false);
            StopAllCoroutines();
        }
    }

    public void RejectTicket()
    {
      
        currentCustomer.MoveAlongWaypoints(exitWays);
        currentCustomer = null;

    }

    public void AcceptTicket() {

        currentCustomer.MoveAlongWaypoints(exitWays);
        currentCustomer = null;
    }
}
