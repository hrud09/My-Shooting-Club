using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
    public List<Customer> spawnedCustomer = new List<Customer>();
    public Customer currentCustomer;
    public AnimationCurve customerSpawnTimeCurve;
    private float timeUntilNextSpawn; // Time until the next customer spawn.

    void Start()
    {
        // Set an initial random time for the first customer spawn.
        timeUntilNextSpawn = 0;
    }

    void Update()
    {
        if (customersInLineCount < maxCustomerCount)
        {
            // Countdown the time until the next spawn.
            timeUntilNextSpawn -= Time.deltaTime;

            if (timeUntilNextSpawn <= 0f)
            {
                // Spawn a customer.
                SpawnCustomer();

                // Reset the timer for the next customer spawn.
                timeUntilNextSpawn = Random.Range(customerSpawnTimeCurve.keys[0].value, customerSpawnTimeCurve.keys[1].value);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCustomer();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace) && customersInLine.Count > 0)
        {
            RemoveCustomerFromLine(customersInLine[0]);
        }
    }
    private string RandomDesignation() {
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
    private string RandomName() {
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
    public void SpawnCustomer()
    {
        int randomIndex = Random.Range(0, customerSpawnPos.Length);
        GameObject customerObject = Instantiate(customerPrefab, customerSpawnPos[randomIndex]);
        Customer customer = customerObject.GetComponent<Customer>();

        customer.customerManager = this;
        spawnedCustomer.Add(customer);
        customersInLineCount++;

        CustomerInfo info = new CustomerInfo();
        customer.currentWaypointIndex = 0;
        info.name = RandomName();
        info.movementSpeed = customerSpeed;


        info.designation = RandomDesignation();



        customer.InitializeCustomer(info);
        customerObject.transform.DOLookAt(firstWaypoint.position, 0.1f);
        customer.customerInfo.customerAnimator.SetFloat("Speed", 1);
        float dist = Vector3.Distance(customerSpawnPos[randomIndex].position, firstWaypoint.position);
        customerObject.transform.DOMove(firstWaypoint.position, dist / customerSpeed).SetEase(Ease.Linear).OnComplete(() => {
            customer.MoveToNextTargetPoint(waypointsToClubEntry[customersInLine.Count], welcomeDeskManager, customersInLine.Count == 0);
            customersInLine.Add(customer);
           
        });
    }

    public void RemoveCustomerFromLine(Customer customer)
    {
        customersInLine.Remove(customer);
        if (customersInLine.Count > 0)
        {
            currentCustomer = customersInLine[0];
            customersInLineCount -= 1;
            UpdateCustomerWaypoints();
        }
    }

    void UpdateCustomerWaypoints()
    {
        for (int i = 0; i < customersInLine.Count; i++)
        {
            customersInLine[i].MoveToNextTargetPoint(waypointsToClubEntry[i], welcomeDeskManager, i == 0);
        }
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
