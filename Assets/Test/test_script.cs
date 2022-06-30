using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class test_script : MonoBehaviour
{
    public Button mybtn;
    // Start is called before the first frame update
    void Start()
    {
       Button btn = mybtn.GetComponent<Button>();
		btn.onClick.AddListener(clickBtn);
        
    }

    void clickBtn()
    {
        Debug.Log("You have clicked the button!");
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
