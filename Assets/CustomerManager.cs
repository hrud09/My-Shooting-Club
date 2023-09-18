using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum CustomerTasks { 

    NormalMovement,
    GoingToClub,
    GoingInfrontOfDesk,
    TakingTicket,
    GoingToSofa,
    GoingToShootingRange
}
public class CustomerManager : MonoBehaviour
{
    public SofaManager sofaManager;

    public GameObject customerPrefab;
    public int maxCustomerSpawn;
    public int minCustomerSpawn;
    public Transform[] customerSpawnRandomTransform;

    private List<Transform> freeTransform;

    public List<Customer> spawnedCustomer;
    public List<Customer> customersInsideClub;
    public List<Customer> customersInLine;
    public List<Customer> customersOnSofa;
    public List<Customer> customersShooting;


    public int randomCustomerCount, randomPositionIndex;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        freeTransform = customerSpawnRandomTransform.ToList();
        randomCustomerCount = Random.Range(minCustomerSpawn, maxCustomerSpawn);
        for (int i = 0; i < randomCustomerCount; i++)
        {
            yield return new WaitForSeconds(0.1f);
            randomPositionIndex = Random.Range(0, freeTransform.Count);
            Transform t = freeTransform[randomPositionIndex];
            GameObject _customerObject = Instantiate(customerPrefab,t);
            freeTransform.Remove(t);

            if (freeTransform.Count == 0) break;
            Customer customer = _customerObject.GetComponent<Customer>();
            customer.customerManager = this;
            customer.SelectRandomTask();
            spawnedCustomer.Add(customer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class CustomerInfo { 

    public string name;
    public CustomerTasks currentTask;
    public bool hasTicket;
    public bool isFree;
    public bool isShooting;
    public int shootingRoundCount;
    public float worthMoney;
    public Transform[] wayPoints;
    public Animator customerAnimator;
}
