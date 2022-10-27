using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Vector3 checkpointPosition;
    public float checkpointRotation;

    public string checkpointName;


    public MotorcycleController trackCheckpoints;

    public bool isLastCheckpoint = false;

    public LapEnd lapEnd;


    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log("Awake: "+gameObject.name);
        // checkpointName = "Checkpoint";
        checkpointPosition = new Vector3(-248f, 0f, 245f);
        // checkpointRotation = 90f;
        // transform.position = checkpointPosition;

        

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        Debug.Log("Checkpoint: " + gameObject.name);
        {
            // switch (gameObject.name)
            // {
            //     case "Checkpoint1":
            //         this.checkpointRotation = 90f;
            //         break;
            //     case "Checkpoint2":
            //         this.checkpointRotation = 90f;
            //         break;
            //     case "Checkpoint3":                    
            //         this.checkpointRotation = 90f;
            //         this.isLastCheckpoint = true;
            //         break;

            // }
            if(isLastCheckpoint)
            {
                lapEnd.LastCheckpoint(this);
            }

            trackCheckpoints.PlayerThroughCheckpoint(this);
        }

    }

    public void SetTrackCheckpoints(MotorcycleController trackCheckpoints)
    {
        this.trackCheckpoints = trackCheckpoints;
    }
    

    // Update is called once per frame
    void Update()
    {

    }
}
