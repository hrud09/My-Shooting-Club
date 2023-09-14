using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DeliveryVanController : MonoBehaviour
{
    public int maximumVansCount;
    public float deliveryVanSpawnTime;
    private float currentDeliveryVanSpawnTime;
    public GameObject deliveryVanPrefab;
    public Transform[] vanStopPos;
    public List<GameObject> loadedVans;
    private DeliveryAreaManager deliveryArea;
    public float vanSpeed;
    public float deliverySpeedMultplier;
    private void Awake()
    {
        deliveryArea = FindAnyObjectByType<DeliveryAreaManager>();
    }
    private void Start()
    {
        currentDeliveryVanSpawnTime = deliveryVanSpawnTime;
    }

    private void Update()
    {
        currentDeliveryVanSpawnTime -= Time.deltaTime;

        if (loadedVans.Count < maximumVansCount && currentDeliveryVanSpawnTime <= 0)
        {
            currentDeliveryVanSpawnTime = deliveryVanSpawnTime;
            SpawnDeliveryVan();
        }

        for (int i = loadedVans.Count - 1; i >= 0; i--)
        {
            loadedVans[i].transform.position = Vector3.MoveTowards(loadedVans[i].transform.position, vanStopPos[i].position, vanSpeed);
        }
    }

    private void SpawnDeliveryVan()
    {
        GameObject vanGO = Instantiate(deliveryVanPrefab, vanStopPos[vanStopPos.Length-1]);
        vanGO.GetComponent<Van>().vanManager = this;
        vanGO.GetComponent<Van>().deliveryArea = deliveryArea;
        loadedVans.Add(vanGO);
    }


}



