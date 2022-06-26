using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offsetRot;
    [SerializeField] Vector3 offsetPos;
    GameObject target;
    MotorcycleController controller;

    private void Start()
    {
        controller = FindObjectOfType<MotorcycleController>();
        target = FindObjectOfType<RiderAnimController>().transform.GetChild(0).transform.GetChild(0).gameObject;
    }



    void LateUpdate ()
    {
        if (!controller.isCrashed)
        {
            float camDistance;
            if (controller.speed <= 90)
            {
                camDistance = controller.speed / 90;
            }
            else
            {
                camDistance = 1;
            }

            Vector3 desiredPos = controller.gameObject.transform.forward;
            desiredPos.y = 0;
            this.transform.position = controller.gameObject.transform.position - desiredPos * (offsetPos.z + camDistance);
            transform.position = new Vector3(transform.position.x, transform.position.y + offsetPos.y, transform.position.z);
        }

        this.transform.LookAt(target.transform.position + offsetRot);
    }
}
