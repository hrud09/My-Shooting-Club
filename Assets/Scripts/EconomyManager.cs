using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public static class EconomyManager
{
    private static TMP_Text moneyText;
    private static Coroutine countingCoroutine;
    private static Vector3 originalScale;

    public static int MoneyCount
    {
        get { return PlayerPrefs.GetInt("MoneyCount", 0); }
        private set { PlayerPrefs.SetInt("MoneyCount", value); }
    }

    public static void Initialize(TMP_Text textMeshPro)
    {
        moneyText = textMeshPro;
        originalScale = moneyText.transform.localScale;
        UpdateEconomy(0); // Initialize the money count from PlayerPrefs
    }

    public static void UpdateEconomy(int valueToAdd)
    {
        // Calculate the previous expected value
        int previousValue = MoneyCount;

        // Increase the money count by the specified value
        MoneyCount += valueToAdd;

        // Stop the previous counting coroutine if it's running
        if (countingCoroutine != null)
        {
            MonoBehaviour monoBehaviour = moneyText.gameObject.GetComponent<MonoBehaviour>();
            monoBehaviour.StopCoroutine(countingCoroutine);
        }

        // Start a new counting coroutine to update the TextMeshPro text with the new money count and add the pop effect
        countingCoroutine = moneyText.gameObject.GetComponent<MonoBehaviour>().StartCoroutine(CountToValue(previousValue, MoneyCount));
    }

    public static void SaveMoneyCount()
    {
        PlayerPrefs.Save();
    }

    private static string FormatMoneyCount(int count)
    {
        if (count >= 1000000) // 1 million or more
        {
            float millionCount = count / 1000000.0f;
            return millionCount.ToString("0.0") + "m";
        }
        else if (count >= 1000) // 1 thousand or more
        {
            float thousandCount = count / 1000.0f;
            return thousandCount.ToString("0.0") + "k";
        }
        else
        {
            return count.ToString();
        }
    }

    private static IEnumerator CountToValue(int startValue, int endValue)
    {
        float duration = 1.0f; // Change this value to control the counting speed
        float startTime = Time.time;

        while (Time.time - startTime < duration)
        {
            float progress = (Time.time - startTime) / duration;
            int currentValue = Mathf.RoundToInt(Mathf.Lerp(startValue, endValue, progress));
            moneyText.text = FormatMoneyCount(currentValue);

            // Add a pop effect by scaling the text slightly
            float scale = Mathf.PingPong(progress * 3f, 0.1f) + 1f; // Adjust the values for the desired pop effect
            moneyText.transform.localScale = originalScale * scale;

            yield return null;
        }

        moneyText.text = FormatMoneyCount(endValue);
        moneyText.transform.localScale = originalScale; // Reset the scale to the original size
    }
}
