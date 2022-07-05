using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JavaScriptHelper : MonoBehaviour
{
   [SerializeField] Text myText;

   void Awake()
    {
        setMyText(myText.text);
    }

   public void setMyText(string text)
   {
      myText.text = text;
   }
}
