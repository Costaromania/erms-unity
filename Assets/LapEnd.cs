using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapEnd : MonoBehaviour
{
    public Timer current;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Lap End");
            current.FinishTime();

        }
    }
}
