using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Newtonsoft.Json;

public class LapEnd : MonoBehaviour
{
    public bool check = false;
    public Timer current;
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
            StartCoroutine(RaceStart());

        }
    }

    IEnumerator RaceEnd()
    {
        WWWForm form = new WWWForm();
        form.AddField("address", "erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5");
        form.AddField("raceId", "123");

        var myData = new
        {
            address = @"erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5",
            raceId = "123"
        };

        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(myData);


        UnityWebRequest req = UnityWebRequest.Post("http://localhost:3002/api/firebase/race-end", "POST");
        req.SetRequestHeader("Content-Type", "application/json");
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(myData))) as UploadHandler;
        req.certificateHandler = new BypassCertificate();

        yield return req.SendWebRequest();

        if (req.isNetworkError || req.isHttpError || req.isNetworkError)
            print("Error: " + req.error);

        print(req.downloadHandler.text);
    }

    IEnumerator RaceStart()
    {
        WWWForm form = new WWWForm();
        form.AddField("address", "erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5");
        form.AddField("raceId", "123");


        var user = new UserData();
        user.address = "erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5";
        user.raceId = "123";

        //Tranform it to Json object
        string jsonData = JsonConvert.SerializeObject(user);


        UnityWebRequest req = UnityWebRequest.Post("http://localhost:3002/api/firebase/race-end", "POST");
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
}
