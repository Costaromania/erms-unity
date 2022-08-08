using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Vector3 checkpointPosition;
    public float checkpointRotation;

    public string checkpointName;

    public bool isLastCheckpoint = false;


    public MotorcycleController trackCheckpoints;

    // Start is called before the first frame update
    void Awake()
    {
        // Debug.Log("Awake: "+gameObject.name);
        // checkpointName = "Checkpoint";
        checkpointPosition = new Vector3(-248f, 0f, 245f);
        checkpointRotation = 90f;
        // transform.position = checkpointPosition;

        

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        Debug.Log("Checkpoint: " + transform.position);
        {
            switch (gameObject.name)
            {
                case "Checkpoint1":
                    this.checkpointRotation = 90f;
                    break;
                case "Checkpoint2":
                    this.checkpointRotation = 90f;
                    break;
                case "Checkpoint3":                    
                    this.checkpointRotation = 90f;
                    break;

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
