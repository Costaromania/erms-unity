using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class LapEnd : MonoBehaviour
{
    public bool check = false;
    public bool checkStart = false;
    public Timer current;
    public JavaScriptHelper userDataJS;

    public long startTime;
    public long endTime;

    void Awake()
    {
        // System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        // int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        startTime = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
        // Debug.Log(startTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (check)
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("Lap End");
                current.FinishTime();
                StartCoroutine(RaceEnd());

            }
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Lap start");
            check = true;
            // StartCoroutine(RaceStart());

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
            }
        }
    }



    IEnumerator RaceStart()
    {
        Debug.Log(userDataJS.getAddress());
        var address = userDataJS.getAddress();

        var user = new UserData();
        user.address = address;
        user.raceId = "cursa no. 1";
        user.startTime = startTime;

        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(user);


        UnityWebRequest req = UnityWebRequest.Post("https://api.erms.ro/api/firebase/race-end", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            Debug.Log("Received: " + req.downloadHandler.text);
        }
    }

    IEnumerator RaceEnd()
    {
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

        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(user);


        UnityWebRequest req = UnityWebRequest.Post("https://api.erms.ro/api/firebase/race-end", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        //Send the request then wait here until it returns
        yield return req.SendWebRequest();

        if (req.isNetworkError)
        {
            Debug.Log("Error While Sending: " + req.error);
        }
        else
        {
            Debug.Log("Received: " + req.downloadHandler.text);
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
