using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEngine : MonoBehaviour
{

    MotorcycleController controller;


    [SerializeField] AudioSource idle;
    [SerializeField] AnimationCurve idleCurve;

    [SerializeField] AudioSource midOn;
    [SerializeField] AnimationCurve midOnCurve;

    [SerializeField] AudioSource midOff;
    [SerializeField] AnimationCurve midOffCurve;


    [SerializeField] AudioSource downGear;
    bool isDownGear;



    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponentInParent<MotorcycleController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (controller.verticalInput > 0)
        {
            midOff.volume = 0;
            midOn.volume = midOnCurve.Evaluate(controller.engineRpm);
        }
        else
        {
            midOff.volume = midOffCurve.Evaluate(controller.engineRpm);
            if (!isDownGear)
            {
                midOn.volume = midOnCurve.Evaluate(controller.engineRpm) - 0.2f;
            }

        }
        idle.volume = idleCurve.Evaluate(controller.engineRpm);
        midOn.pitch = Mathf.Lerp(0.3f, 1.5f, controller.engineRpm / controller.maxEngineRpm);
        midOff.pitch = Mathf.Lerp(0.5f, 1f, controller.engineRpm / controller.maxEngineRpm);


    }

    public void DownGear()
    {
        StartCoroutine(MuteSounds(true, 0f));
        downGear.PlayOneShot(downGear.clip);
        StartCoroutine(MuteSounds(false, 0.2f));


    }

    IEnumerator MuteSounds(bool gear, float delay)
    {
        yield return new WaitForSeconds(delay);
        isDownGear = gear;

        midOn.volume = 0.7f;

    }
}
