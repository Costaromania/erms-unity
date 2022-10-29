using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class LapEnd : MonoBehaviour
{
    public bool check = false;
    public bool isFinished = false;
    public bool checkStart = false;
    public Timer current;
    public JavaScriptHelper userDataJS;



    // #if UNITY_EDITOR
    //         public const string BackendURL = "http://127.0.0.1:2000"; // REPLACE THIS WITH YOUR LOCAL SERVER URL
    // #else
    //     public const string BackendURL = "https://MyAPI.com; // REPLACE THIS WITH YOUR SERVER URL
    // #endif


    public MotorcycleController motorcycleController;

    public long startTime;
    public long endTime;

    public bool isLastCheckpoint;

    bool finished = false;


    void Awake()
    {
        // System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        // int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        // Debug.Log(startTime);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (isLastCheckpoint & !finished)
        {
            if (gameObject.name == "LapEnd")
            {
                Debug.Log("Lap End");
                current.FinishTime();
                StartCoroutine(RaceEnd());
                finished = true;

            }
        }
    }

    void Update()
    {
        if (!checkStart)
        {
            if (current.isStarted)
            {
                startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
                checkStart = true;
                Debug.Log("Start Time: " + startTime);
            }
        }
    }

    private void HandleRequest(UnityWebRequest request)
    {

        try
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error: " + request.error);
            }
            else
            {
                Debug.Log("Received: " + request.downloadHandler.text);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }


    }

    public void LastCheckpoint(Checkpoint checkpoint)
    {
        isLastCheckpoint = checkpoint.isLastCheckpoint;
    }





    IEnumerator RaceStart()
    {
        UnityWebRequest req = UnityWebRequest.Post("https://api.erms.ro/api/firebase/race-end", "POST");

        Debug.Log(userDataJS.getAddress());
        var address = userDataJS.getAddress();

        var user = new UserData();
        user.address = address;
        user.raceId = "cursa no. 1";
        user.startTime = startTime;

        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(user);


        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        req.SendWebRequest();
        HandleRequest(req);

        yield return 0;


    }

    IEnumerator RaceEnd()
    {
        if (!isFinished)
        {
            isFinished = true;
            UnityWebRequest req = UnityWebRequest.Post("https://api.erms.ro/api/firebase/race-end", "POST");
            // motorcycleController.Finish();
            // motorcycleController.LoseInput();
            endTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Debug.Log("Start Time: " + startTime);
            Debug.Log("End Time: " + endTime);
            Debug.Log("Current Time: " + current.currentTime);
            Debug.Log(userDataJS.getAddress());
            var address = userDataJS.getAddress();
            var raceName = userDataJS.getRaceName();
            var raceId = userDataJS.getRaceId();
            var raceTime = current.currentTime;

            var user = new UserData();
            user.address = address;
            user.raceName = raceName;
            user.raceId = raceId;
            user.startTime = startTime;
            user.endTime = endTime;
            user.raceTime = raceTime;

            // //Tranform it to Json object
            string jsonData = JsonConvert.SerializeObject(user);


            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.disposeUploadHandlerOnDispose = true;
            req.disposeDownloadHandlerOnDispose = true;
            req.SendWebRequest();
            HandleRequest(req);

            yield return 0;
        }
    }
}



public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        return true;
    }
}

public class UserData
{
    public string address;
    public string raceId;
    public string raceName;
    public long startTime;
    public long endTime;
    public float raceTime;

    // public UserData(string ad,string racei)
    // {
    //     address = ad;
    //     raceId = racei;
    // }
}
