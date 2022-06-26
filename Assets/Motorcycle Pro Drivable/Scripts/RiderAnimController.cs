using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiderAnimController : MonoBehaviour
{
    Animator cmpAnimator;
    MotorcycleController motorcycleController;
    float speed;
    public Transform rightHandIk;
    public Transform LeftHandIk;

    // Start is called before the first frame update
    void Start()
    {
        cmpAnimator = GetComponent<Animator>();
        motorcycleController = GetComponentInParent<MotorcycleController>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = motorcycleController.speed;

        if(speed > 0.1f && speed < 15f && cmpAnimator.GetBool("Reverse") == false)
        {
            cmpAnimator.SetLayerWeight(1, speed / 15f);
        }

        if(motorcycleController.verticalInput < -0.9f && cmpAnimator.GetLayerWeight(1) < 0.1f)
        {
            cmpAnimator.SetBool("Reverse", true);
        }
        else
        {
            cmpAnimator.SetBool("Reverse", false);
        }

        float r = Mathf.Lerp(0.5f, 1, motorcycleController.verticalInput);
        cmpAnimator.SetFloat("MoveY", Mathf.Lerp(0, 1, speed / 200 * r ));

        float newBodyPos = Mathf.Lerp(cmpAnimator.GetFloat("MoveX"), 1 * motorcycleController.horizontalInput, 10f * Time.deltaTime);
        cmpAnimator.SetFloat("MoveX", Mathf.Lerp(0, newBodyPos, speed / 20f   ));


    }

    private void OnAnimatorIK(int layerIndex)
    {
        cmpAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        cmpAnimator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
        cmpAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        cmpAnimator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        cmpAnimator.SetIKPosition(AvatarIKGoal.RightHand, rightHandIk.position);
        cmpAnimator.SetIKRotation(AvatarIKGoal.RightHand, rightHandIk.rotation);
        cmpAnimator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandIk.position);
        cmpAnimator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandIk.rotation);
    }

    
}
