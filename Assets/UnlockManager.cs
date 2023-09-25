using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{

    public BuildingType buildingType;
    public string unlockID;
    public bool isUnlocked;
    public int unlockCost;
    public int currentValue;
    public Image unlockFill;
    public GameObject objectToUnlock;
    // Start is called before the first frame update
    void Start()
    {
        CheckUnlock();   
    }

    public void CheckUnlock()
    {
        if (PlayerPrefs.GetInt(unlockID + buildingType, 0) == 1)
        {
            isUnlocked = true;
        }
    }
}
public enum BuildingType{

    ShootingArea,
    Sofa,
    HR,
    DeliveryArea,
    UnpackArea
}