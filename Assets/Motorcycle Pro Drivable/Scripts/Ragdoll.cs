using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Animator cmpAnimator;
    Rigidbody[] rigidbodies;
    Collider[] colliders;

    // Start is called before the first frame update
    void Start()
    {
        cmpAnimator = GetComponent<Animator>();
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
        colliders = transform.GetComponentsInChildren<Collider>();
        SetRagdoll(false);
    }

    public void SetRagdoll(bool enabled)
    {
        foreach (Collider col in colliders)
        {
            col.isTrigger = !enabled;
        }
        foreach (Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !enabled;
            if (enabled)
            {
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }
            else
            {
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            }
        }


        cmpAnimator.enabled = !enabled;
    }


}
