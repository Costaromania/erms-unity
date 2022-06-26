using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoardCam : MonoBehaviour
{
    GameObject cam;
    GameObject target;
    Vector3 initialPos;
    MotorcycleController controller;


    void Start()
    {
        cam = transform.GetChild(0).gameObject;
        target = transform.GetChild(1).gameObject;
        initialPos = cam.transform.localPosition;
        controller = GetComponentInParent<MotorcycleController>();
    }

    void Update()
    {
        cam.transform.localPosition = Vector3.Lerp(initialPos, new Vector3(0, 0, -0.07f), controller.speed/320f);

        cam.transform.LookAt(target.transform.position);
    }
}
