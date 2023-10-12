using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float averageFPS = 0.0f;
    private int frameCount = 0;

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        frameCount++;
        if (frameCount >= 10) // Calculate average FPS over the last 10 frames
        {
            averageFPS = frameCount / deltaTime;
            frameCount = 0;
            deltaTime = 0.0f;
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 40;
        style.normal.textColor = Color.white;

        float currentFPS = 1 / deltaTime;

        GUI.Label(new Rect(20, 20, 150, 30), "FPS: " + Mathf.Round(currentFPS), style);
        GUILayout.Space(100);
        GUI.Label(new Rect(20, 70, 200, 40), "Avg FPS: " + Mathf.Round(averageFPS), style);
       
        // Add space between labels

    }
}
