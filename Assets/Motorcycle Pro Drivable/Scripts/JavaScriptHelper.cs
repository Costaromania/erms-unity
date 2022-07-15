using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class JavaScriptHelper : MonoBehaviour
{
    // #if UNITY_WEBGL && !UNITY_EDITOR
    //    [DLLImport("__Internal")]
    //    private static void ShowMessage(string message);
    // #endif


    [SerializeField]
    public Text address;
    public string userAddress;
    public string raceName;
    public string raceId;

    void Awake()
    {
        //  SendToJS();
        setAddress(address.text);
        setRaceId(raceId);
        setRaceName(raceName);
        // userAddress = "erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5";

    }

    void Update()
    {
        // setAddress("erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5");

    }

    public void setAddress(string text)
    {
        address.text = text;
        if(text != "")
        {
            userAddress = text;
        } else {
            userAddress = "no address";
        }
        // address.text = "erd19sayrzwrx90ypkcgwg9m0el48hv8u4dczxst4r2c6l6v65mcv42qnjkkx5";
    }

    public string getAddress()
    {
        return userAddress;
    }

    public void setRaceName(string text)
    {
        if(text != "")
        {
            raceName = text;
        } else {
            raceName = "no name";
        }
    }

    public string getRaceName()
    {
        return raceName;
    }

    public void setRaceId(string text)
    {
        if(text != "")
        {
            raceId = text;
        } else {
            raceId = "no id";
        }
    }

    public string getRaceId()
    {
        return raceId;
    }



    //     public void SendToJS()
    //     {
    //         string message = "Hello from C#";
    // #if UNITY_WEBGL && !UNITY_EDITOR
    //       ShowMessage(message);
    // #endif
    //     }
}
