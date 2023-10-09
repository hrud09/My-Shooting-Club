using UnityEngine;
using TMPro;

public class EconomyManagerScript : MonoBehaviour
{
    public TMP_Text moneyText;


    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        EconomyManager.Initialize(moneyText);
        // You can call EconomyManager.UpdateEconomy(value) whenever you want to update the money count.
       // EconomyManager.UpdateEconomy(100); // Example: Update the money count to 100.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Increase the amount by 100
            EconomyManager.UpdateEconomy(100);
        }
    }
}
