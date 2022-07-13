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

    [SerializeField] Text myText;

    void Awake()
    {
      //  SendToJS();
    }

    void Update()
    {
        setMyText(myText.text);
    }

    public void setMyText(string text)
    {
        myText.text = text;
    }

//     public void SendToJS()
//     {
//         string message = "Hello from C#";
// #if UNITY_WEBGL && !UNITY_EDITOR
//       ShowMessage(message);
// #endif
//     }
}
