using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{

    MotorcycleController controller;
    [SerializeField]public Image Rpm;
    [SerializeField] public Text speed;
    [SerializeField] public Text gear;
    [SerializeField] public Text shifterMode;
    [SerializeField] public Image turnRight;
    [SerializeField] public Image turnLeft;
    [SerializeField] public Image Head_Light;
    [SerializeField] public Image Leds_Display;


    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<MotorcycleController>();

        shifterMode.text = controller.isAutomatic ? "A" : "M";

    }

    // Update is called once per frame
    void Update()
    {
        Rpm.fillAmount = controller.engineRpm / 20000f;
        speed.text = controller.speed.ToString("F0");
        if(controller.speed < 1)
        {
            gear.text = "N";
        }
        else
        {
            gear.text = (1 + controller.currentGear).ToString();
        }

        turnLeft.enabled = controller.turnLeftLight.activeInHierarchy;
        turnRight.enabled = controller.turnRightLight.activeInHierarchy;
        Head_Light.enabled = controller.headLight.activeInHierarchy;

        if (controller.engineRpm > 13000f)
        {
            Leds_Display.fillAmount = (controller.engineRpm - 13000f) / 1000;
        }
        else
        {
            Leds_Display.fillAmount = 0;
        }
    }
}
