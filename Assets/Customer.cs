using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customer : MonoBehaviour
{

    public CustomerInfo customerInfo; 
    public CustomerManager customerManager;
    public void SelectRandomTask()
    {
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            customerInfo.currentTask = CustomerTasks.GoingToClub;
        }
        else
        {
            customerInfo.currentTask = CustomerTasks.NormalMovement;
        }
    }
}
