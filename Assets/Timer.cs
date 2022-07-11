using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using Sentry;

public class Timer : MonoBehaviour
{

    public float currentTime;
    public Text currentTimeText;
    public Text startText;

    public bool isFinished = false;

    public void FinishTime()
    {
        isFinished = true;
        currentTime = currentTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        // options.TracesSampleRate = 1.0;
        currentTime = -3;
        startText.text = "";


    }

    // Update is called once per frame
    void Update()
    {

        if (!isFinished)
        {

            currentTime = currentTime + Time.deltaTime;
            TimeSpan time = TimeSpan.FromSeconds(currentTime);
            // if(currentTime > 0 && currentTime < 3){
            //     startText.text = "START";
            // }
            if (currentTime > 0)
            {
                currentTimeText.text = time.ToString(@"mm\:ss\:fff");
                if (currentTime < 3)
                {
                    startText.text = "START";
                }
                else
                {
                    startText.text = "";
                }
            }
            else
            {
                currentTimeText.text = time.ToString(@"\-ss\:fff");
            }
        }
    }
}
