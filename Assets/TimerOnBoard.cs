using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerOnBoard : MonoBehaviour
{
    Timer timer;
    public Text currentTimeText;
    public Text startText;
    // Start is called before the first frame update
    void Start()
    {
        currentTimeText = timer.currentTimeText;
        startText = timer.startText;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
