using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSReporter : MonoBehaviour {
    UnityEngine.UI.Text fpsText;

    public bool clearFPSlock;
	// Use this for initialization
	void Start () 
	{
        fpsText = this.GetComponent<UnityEngine.UI.Text>();
       // if (clearFPSlock)
        Application.targetFrameRate = 60;
            
         Debug.Log(Application.targetFrameRate);
    }

    float lastFrameTime;
    float thisFrameTime;

    int fps0;

    int fpsDisp;

    int lowest;
    float lowestFrameOccurTime;


    float lastDispTime;
    
    // Update is called once per frame
    void Update () {
        float time = Time.time;

        thisFrameTime = time;
        fps0 = Mathf.RoundToInt(1 / (thisFrameTime - lastFrameTime));


        if (fps0 < lowest || time > lowestFrameOccurTime + 1)
        {
            lowest = fps0;
            lowestFrameOccurTime = thisFrameTime;
        }

        if (time > lastDispTime + 0.1f)
        {
            lastDispTime = time;
            fpsDisp = fps0;
        }


        fpsText.text = string.Format("FPS: {0}\nMin: {1}", fpsDisp, lowest);
        lastFrameTime = thisFrameTime;
	}
}
