using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JavaScriptHelper : MonoBehaviour
{
   [SerializeField] Text myText;

   public void setMyText(string text)
   {
      myText.text = text;
   }
}
