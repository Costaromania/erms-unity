using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public Vector3 checkpointPosition;
    public float checkpointRotation;

    public string checkpointName;


    public MotorcycleController MotorcycleController;

    public bool isLastCheckpoint = false;

    public LapEnd lapEnd;
    public int CurrentCheckpointNumber;
    public int NextCheckpointNumber;
    public int CheckpointsCount;
    public int LapsCount;
    int CurrentLapNumber;



    // Start is called before the first frame update
    void Awake()
    {

        // Debug.Log("Awake: "+gameObject.name);
        // checkpointName = "Checkpoint";
        checkpointPosition = new Vector3(-248f, 0f, 245f);
        // checkpointRotation = 90f;
        // transform.position = checkpointPosition;

        //get number of child objects for checkpoint
        CheckpointsCount = transform.parent.childCount;
        if (MotorcycleController != null)
        {
            LapsCount = MotorcycleController.lapsCount;
            CurrentLapNumber = 1;
            Debug.Log("CheckpointsCount: " + CheckpointsCount);
        }




    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (gameObject.name == "Checkpoint" + CurrentCheckpointNumber)
            {
                Debug.Log("Checkpoint: " + CurrentCheckpointNumber);
                NextCheckpointNumber++;
                if (CurrentCheckpointNumber == CheckpointsCount)
                {

                    if (CurrentLapNumber == LapsCount)
                    {
                        isLastCheckpoint = true;
                        // lapEnd.isLastCheckpoint = true;
                        lapEnd.LastCheckpoint(this);
                    }
                    NextCheckpointNumber = 1;
                    CurrentLapNumber++;
                }
            }
            else
            {
                Debug.Log("Wrong Checkpoint");
            }
            MotorcycleController.PlayerThroughCheckpoint(this);
        }

    }

    public void SetTrackCheckpoints(MotorcycleController trackCheckpoints)
    {
        this.MotorcycleController = trackCheckpoints;
    }


    // Update is called once per frame
    void Update()
    {

    }
}
