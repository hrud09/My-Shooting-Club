using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform[] waypointsToClubEntry;
    public Transform[] customerSpawnPos;
    public Transform firstWaypoint;
    public int maxCustomerCount;
    public float customerSpeed;

    public WelcomeDeskManager welcomeDeskManager;

    public int customersInLineCount;
    public List<Customer> customersInLine = new List<Customer>();

    public int pooledCustomerCount;
    public List<Customer> spawnedCustomer = new List<Customer>();
    public Customer currentCustomer;
    public AnimationCurve customerSpawnTimeCurve;
    private float timeUntilNextSpawn;

    void Start()
    {
        timeUntilNextSpawn = 0;
        StartCoroutine(SpawnPoolObjects(pooledCustomerCount));
    }

    void Update()
    {
        if (customersInLineCount < maxCustomerCount)
        {
            timeUntilNextSpawn -= Time.deltaTime;

            if (timeUntilNextSpawn <= 0f)
            {
                SelectACustomer();
                timeUntilNextSpawn = Random.Range(customerSpawnTimeCurve.keys[0].value, customerSpawnTimeCurve.keys[1].value) * customersInLineCount;
            }
        }
    }

    public void SelectACustomer()
    {

        Customer customer = GetACustomer();
        GameObject customerObject = customer.gameObject;
           
        customer.customerManager = this;
        customer.currentWaypointIndex = 0;
        customersInLineCount++;

        CustomerInfo info = new CustomerInfo();      
        info.name = RandomName();
        info.movementSpeed = customerSpeed;
        info.designation = RandomDesignation();
        customer.InitializeCustomer(info);


        customerObject.transform.DOLookAt(firstWaypoint.position, 0.1f);
        customer.customerInfo.customerAnimator.SetFloat("Speed", 1);
        float dist = Vector3.Distance(customerObject.transform.position, firstWaypoint.position);
        customerObject.transform.DOMove(firstWaypoint.position, dist / customerSpeed).SetEase(Ease.Linear).OnComplete(() => {

            customer.MoveToNextTargetPoint(waypointsToClubEntry[customersInLine.Count], welcomeDeskManager, customersInLine.Count == 0);
            customersInLine.Add(customer);
           
        });
    }



    private Customer GetACustomer() {

        if (spawnedCustomer.Count>0)
        {
            Customer electedCustomer = spawnedCustomer[0];
            spawnedCustomer.Remove(electedCustomer);
            electedCustomer.gameObject.SetActive(true);
            return electedCustomer;
        }
        else
        {
            Spawn();
            Customer electedCustomer = spawnedCustomer[0];
            spawnedCustomer.Remove(electedCustomer);
            electedCustomer.gameObject.SetActive(true);
            return electedCustomer;
        }

    }
    private IEnumerator SpawnPoolObjects(int _pooledCustomerCount)
    {
        for (int i = 0; i < _pooledCustomerCount; i++)
        { 
            Spawn();
            yield return new WaitForEndOfFrame();
        } 
    }
    private void Spawn()
    {
        int randomIndex = Random.Range(0, customerSpawnPos.Length);
        GameObject customerObject = Instantiate(customerPrefab, customerSpawnPos[randomIndex]);
        Customer customer = customerObject.GetComponent<Customer>();
        customerObject.SetActive(false);
        spawnedCustomer.Add(customer);
    }
    public void RemoveCustomerFromLine(Customer customer)
    {
        customersInLine.Remove(customer);
        if (customersInLine.Count > 0)
        {
            currentCustomer = customersInLine[0];
            customersInLineCount -= 1;
            Invoke("UpdateCustomerWaypoints", 1);
        }
    }
    void UpdateCustomerWaypoints()
    {
        for (int i = 0; i < customersInLine.Count; i++)
        {
            customersInLine[i].MoveToNextTargetPoint(waypointsToClubEntry[i], welcomeDeskManager, i == 0);
        }
    }
    private string RandomDesignation()
    {
        string filePath = "Assets/Resources/Designations.txt"; // Replace with the correct path to your text file

        string[] designationArray = System.IO.File.ReadAllText(filePath).Split(',');
        string randomDesignation = "";
        if (designationArray.Length > 0)
        {
            int randomIndex = Random.Range(0, designationArray.Length);
            randomDesignation = designationArray[randomIndex].Trim(); // Trim to remove leading/trailing spaces
        }

        return randomDesignation;
    }
    private string RandomName()
    {
        string filePath = "Assets/Resources/Name.txt"; // Replace with the correct path to your text file

        string[] nameArray = System.IO.File.ReadAllText(filePath).Split(',');
        string randomName = "";
        if (nameArray.Length > 0)
        {
            int randomIndex = Random.Range(0, nameArray.Length);
            randomName = nameArray[randomIndex].Trim(); // Trim to remove leading/trailing spaces
        }

        return randomName;
    }
}

[System.Serializable]
public class CustomerInfo
{
    public string name;
    public string designation;
    public bool hasTicket;
    public bool isFree;
    public bool isShooting;
    public int shootingRoundCount;
    public float worthMoney;
    public float movementSpeed;
    public Animator customerAnimator;
}
