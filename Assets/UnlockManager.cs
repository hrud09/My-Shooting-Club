using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class UnlockManager : MonoBehaviour
{
    public BuildingType buildingType;
    public string unlockIndex;
    private string unlockAreaID;
    [SerializeField]
    public UnityEvent onStartingUnlock, onNewUnlock;

    public bool isUnlocked;
    public int unlockCost;
    public int currentValue;
    public TMP_Text currentValueText; // Reference to the TMP_Text component for displaying currentValue.
    public GameObject objectToUnlock;
    public GameObject player;

    // Variables for the pop effect.
    private float popDuration = 0.5f; // Adjust the duration as needed.
    private float popScale = 1.2f; // Adjust the scale factor as needed.
    private float originalTextScale;


    float timePerDollarConst = 0.01f;
    public float timePerDoller;
    // Start is called before the first frame update
    void Awake()
    {
        unlockAreaID = buildingType.ToString() + unlockIndex;
        currentValue = PlayerPrefs.GetInt(unlockAreaID + "CurrentValue", unlockCost);
        timePerDoller = 5 / currentValue;
        if (currentValueText != null) originalTextScale = currentValueText.transform.localScale.x;
        if (PlayerPrefs.GetInt(unlockAreaID, 0) == 1) Unlock(true);
        UpdateCurrentValueText();
    }

    private void Update()
    {
        if (IsUnlockable())
        {
            timePerDollarConst = timePerDoller;
            currentValue -= 1;
            EconomyManager.UpdateEconomy(-1);
            if (currentValue <= 0) Unlock();
            UpdateCurrentValueText();
        }
        else
        {
            timePerDollarConst -= Time.deltaTime;
        }
    }
    private bool IsUnlockable()
    {
        return player && !isUnlocked && currentValue > 0 && EconomyManager.MoneyCount > 0 && timePerDollarConst <= 0;
    }
    private void Unlock(bool isStartingUnlockCall = false)
    {
        isUnlocked = true;
        currentValue = 0;
        transform.parent.gameObject.SetActive(false);
        PlayerPrefs.SetInt(unlockAreaID, 1);
        objectToUnlock.SetActive(true);
        if (isStartingUnlockCall) onStartingUnlock.Invoke();
        else onNewUnlock.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            //currentValue = PlayerPrefs.GetFloat(unlockAreaID + "CurrentValue", currentValue);
            UpdateCurrentValueText();
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerPrefs.SetInt(unlockAreaID + "CurrentValue", currentValue);
            player = null;
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(unlockAreaID + "CurrentValue", currentValue);
    }
    // Function to update the current value text and apply a pop effect.
    private void UpdateCurrentValueText()
    {
        if (currentValueText != null)
        {
            currentValueText.text = currentValue.ToString("0") + "$";

            // Apply a pop effect by changing the text's local scale.
            float scale = Mathf.Lerp(originalTextScale, popScale, Mathf.PingPong(Time.time / popDuration, 1f));
            currentValueText.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}

public enum BuildingType
{
    ShootingArea,
    Sofa,
    HR,
    DeliveryArea,
    UnpackArea
}
