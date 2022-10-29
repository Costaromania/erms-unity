using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
// using Sentry; // On the top of the script

public class MotorcycleController : MonoBehaviour
{
    public static bool gameIsPaused;

    Vector3 startPosition;
    Quaternion startRotation;

    [SerializeField] WheelCollider frontWheelCollider, rearWheelCollider;
    public float maxEngineRpm = 94000f;
    float minEngineRpm;
    [HideInInspector] public float engineRpm;
    [HideInInspector] public float speed;
    [SerializeField] float[] gearRatio;
    [HideInInspector] public int currentGear = 0;
    float acceleration;
    float totalPower;
    float finalDrive = 3f;
    [SerializeField] AnimationCurve enginePower;
    public bool isAutomatic;
    float maxSteerAngle = 15f;
    float steeringAngle;
    bool isBraking;

    /// <summary>
    /// Mobile controller variables
    /// </summary>

    public bool useTouchControls;

    public GameObject theStart;

    public GameObject menu;
    public Button openMenu;
    public Button resumeBtn;
    public GameObject throttleButton;
    MobileController throttlePTI;
    public GameObject reverseButton;
    MobileController reversePTI;
    public GameObject turnRightButton;
    MobileController turnRightPTI;
    public GameObject turnLeftButton;
    MobileController turnLeftPTI;
    public GameObject handbrakeButton;
    MobileController handbrakePTI;



    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;


    Rigidbody rb;

    [SerializeField] GameObject handlebar;
    [SerializeField] GameObject motorcycleMesh;

    //Rear Wheel
    [SerializeField] GameObject hinge_Rear;
    [SerializeField] GameObject susp_Rear;
    [SerializeField] GameObject wheel_Rear;
    GameObject offset_Wheel;
    float offset;

    //Front Wheel
    [SerializeField] GameObject hinge_Front;
    [SerializeField] GameObject susp_Front;
    [SerializeField] GameObject wheel_Front;
    Vector3 startLocalPos;
    Quaternion handlebarStartRot;

    //CRASH
    [HideInInspector] public bool isCrashed;
    Ragdoll cmpRagdollRider;

    //Audio
    AudioEngine cmpAudio;

    //Cams
    GameObject cam_OnBoard;
    GameObject cam_Follow;

    //Canvases
    GameObject canvas_OnBoard;
    GameObject canvas_Screen;

    //Lights
    [SerializeField] GameObject brakeLight;
    [SerializeField] GameObject dayLight;
    [SerializeField] GameObject tailLight;
    public GameObject headLight;
    public GameObject turnLeftLight;
    public GameObject turnRightLight;

    public Checkpoint Checkpoint;

    Transform checkpointsTransform;

    public JavaScriptHelper jsHelper;

    public Timer current;

    public LapEnd lapEnd;



    bool isLeftLightIndicator;
    bool isRigthLightIndicator;

    Vector3 originalPos;
    Vector3 lastCheckpointPosition;
    float lastCheckpointRotation;


    [HideInInspector] public float currentTime = 0f;
    public float bonusAcceleration;
    public float bonusSpeed;
    public float bonusBraking;
    public float bonusSteering;
    public float bonusHandling;
    public float maxSpeed;

    public int lapsCount;


    public GameObject resetButton;
    public GameObject menuResetButton;

    public void enableTouchControls(bool touchControls)
    {
        throttleButton.SetActive(touchControls);
        reverseButton.SetActive(touchControls);
        turnRightButton.SetActive(touchControls);
        turnLeftButton.SetActive(touchControls);
        handbrakeButton.SetActive(touchControls);
    }

    void Awake()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lastCheckpointPosition = Checkpoint.checkpointPosition;
        lastCheckpointRotation = Checkpoint.checkpointRotation;
        menu.SetActive(false);
        originalPos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        

        cmpAudio = GetComponentInChildren<AudioEngine>();
        rb = GetComponent<Rigidbody>();

        offset_Wheel = new GameObject();
        offset = hinge_Rear.transform.position.y - rearWheelCollider.transform.position.y;

        startLocalPos = susp_Front.transform.localPosition;
        handlebarStartRot = handlebar.transform.localRotation;

        rearWheelCollider.ConfigureVehicleSubsteps(1000, 30, 30);
        frontWheelCollider.ConfigureVehicleSubsteps(1000, 30, 30);

        cmpRagdollRider = GetComponentInChildren<Ragdoll>();
        rb.centerOfMass = new Vector3(0, 0.3f, 0);

        cam_Follow = FindObjectOfType<CameraFollow>().gameObject;
        cam_OnBoard = GetComponentInChildren<OnBoardCam>().gameObject;
        cam_OnBoard.SetActive(false);

        canvas_OnBoard = GetComponentInChildren<Display>().gameObject;
        canvas_Screen = GameObject.Find("CanvasScreen");
        canvas_OnBoard.SetActive(false);

        tailLight.SetActive(true);
        dayLight.SetActive(true);

        Button btnReset = resetButton.GetComponent<Button>();
        btnReset.onClick.AddListener(reset);

        Button menuBtnReset = menuResetButton.GetComponent<Button>();
        menuBtnReset.onClick.AddListener(menuReset);

        Button btnResume = resumeBtn.GetComponent<Button>();
        btnResume.onClick.AddListener(resumeMenu);

        Button btnOpenMenu = openMenu.GetComponent<Button>();
        btnOpenMenu.onClick.AddListener(resumeMenu);

        throttlePTI = throttleButton.GetComponent<MobileController>();
        reversePTI = reverseButton.GetComponent<MobileController>();
        turnLeftPTI = turnLeftButton.GetComponent<MobileController>();
        turnRightPTI = turnRightButton.GetComponent<MobileController>();
        handbrakePTI = handbrakeButton.GetComponent<MobileController>();

        // bonusBraking = bonusBraking;
        // bonusHandling = bonusHandling;
        // bonusSpeed = bonusSpeed;
        // bonusSteering = bonusSteering;
        // bonusAcceleration = bonusAcceleration;
        // maxSpeed = maxSpeed;



    }


    public void PlayerThroughCheckpoint(Checkpoint checkpoint)
    {
        // Debug.Log("Current checkpoint position: " + checkpoint.transform.position);
        lastCheckpointPosition = checkpoint.transform.position;
        lastCheckpointPosition.y = 0f;
        lastCheckpointRotation = checkpoint.checkpointRotation;
    }

    private void menuReset()
    {
        resetToStart();
        resumeMenu();
    }

    private void resetToStart()
    {
        current.Reset();
        isCrashed = false;
        lapEnd.isLastCheckpoint = false;
        Debug.Log("Rotation: " + lastCheckpointRotation);
        //SceneManager.LoadScene(0);
        verticalInput = 0f;
        horizontalInput = 0f;
        totalPower = 0f;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(0, 0, 0);
        // gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.5f);
        // gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);

        gameObject.transform.position = startPosition;
        gameObject.transform.rotation = startRotation;


        // switch (Checkpoint.checkpointName)
        // {
        //     case "Checkpoint":
        //         gameObject.transform.position = new Vector3(Checkpoint.checkpointPosition.x, Checkpoint.checkpointPosition.y, Checkpoint.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint.checkpointRotation, 0);
        //         break;
        //     case "Checkpoint1":
        //         gameObject.transform.position = new Vector3(Checkpoint1.checkpointPosition.x, Checkpoint1.checkpointPosition.y, Checkpoint1.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint1.checkpointRotation, 0);
        //         break;
        //     case "Checkpoint2":
        //         gameObject.transform.position = new Vector3(Checkpoint2.checkpointPosition.x, Checkpoint2.checkpointPosition.y, Checkpoint2.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint2.checkpointRotation, 0);
        //         break;
        // }



        cmpRagdollRider.SetRagdoll(false);
        cam_Follow.SetActive(cam_Follow.activeInHierarchy);
        cam_Follow.SetActive(true);
        isCrashed = false;
        Stabilizer();
        currentTime = 0f;
        speed = 0f;
        acceleration = 0f;
        LoseInput();
    }

    private void reset()
    {
        isCrashed = false;
        Debug.Log("Rotation: " + lastCheckpointRotation);
        //SceneManager.LoadScene(0);
        verticalInput = 0f;
        horizontalInput = 0f;
        totalPower = 0f;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(0, 0, 0);
        // gameObject.transform.position = new Vector3(gameObject.transform.position.x - 0.5f, gameObject.transform.position.y, gameObject.transform.position.z - 0.5f);
        // gameObject.transform.rotation = Quaternion.Euler(0, 90, 0);

        gameObject.transform.position = lastCheckpointPosition;
        gameObject.transform.rotation = Quaternion.Euler(0, lastCheckpointRotation, 0);


        // switch (Checkpoint.checkpointName)
        // {
        //     case "Checkpoint":
        //         gameObject.transform.position = new Vector3(Checkpoint.checkpointPosition.x, Checkpoint.checkpointPosition.y, Checkpoint.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint.checkpointRotation, 0);
        //         break;
        //     case "Checkpoint1":
        //         gameObject.transform.position = new Vector3(Checkpoint1.checkpointPosition.x, Checkpoint1.checkpointPosition.y, Checkpoint1.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint1.checkpointRotation, 0);
        //         break;
        //     case "Checkpoint2":
        //         gameObject.transform.position = new Vector3(Checkpoint2.checkpointPosition.x, Checkpoint2.checkpointPosition.y, Checkpoint2.checkpointPosition.z);
        //         gameObject.transform.rotation = Quaternion.Euler(0, Checkpoint2.checkpointRotation, 0);
        //         break;
        // }



        cmpRagdollRider.SetRagdoll(false);
        cam_Follow.SetActive(cam_Follow.activeInHierarchy);
        cam_Follow.SetActive(true);
        isCrashed = false;
        Stabilizer();
        acceleration = 0f;
        speed = 0f;

    }

    public void Finish()
    {
        verticalInput = 0f;
        horizontalInput = 0f;
        totalPower = 0f;
        rb.velocity = new Vector3(0, 0, 0);
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(0, 0, 0);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        cmpRagdollRider.SetRagdoll(false);
        cam_Follow.SetActive(cam_Follow.activeInHierarchy);
        cam_Follow.SetActive(true);
        isCrashed = false;
        Stabilizer();
    }

    private void resumeMenu()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }

    private void Update()
    {

        useTouchControls = jsHelper.useTouchControls;
        enableTouchControls(useTouchControls);

        if (isCrashed)
        {
            resetButton.SetActive(true);
        }
        else
        {
            resetButton.SetActive(false);
        }

        if (useTouchControls)
        {

            if (reversePTI.buttonPressed)
            {
                Brake(true);
                brakeLight.SetActive(true);
            }
            else
            {
                Brake(false);
                brakeLight.SetActive(false);
            }

            if (throttlePTI.buttonPressed)
            {

                verticalInput += 0.01f;

            }
            else
            {
                verticalInput = 0f;
            }

            if (turnLeftPTI.buttonPressed || turnRightPTI.buttonPressed)
            {
                if (turnLeftPTI.buttonPressed)
                {
                    horizontalInput -= 0.03f;
                }
                if (turnRightPTI.buttonPressed)
                {
                    horizontalInput += 0.03f;
                }

            }
            else
            {

                horizontalInput = 0f;

            }



        }
        else
        {

            if (Input.GetButton("Brake"))
            {
                // Debug.Log("Space Brake");
                Brake(true);
                brakeLight.SetActive(true);

            }
            // else if (rearWheelCollider.rpm < -1f && verticalInput > -0.7f)
            // {
            //     Brake(true);
            //     brakeLight.SetActive(true);

            // }
            else
            {
                Brake(false);
            }

            if (Input.GetButton("Reset"))
            {
                reset();
            }

            //ShifterManual
            if (!isAutomatic)
            {
                if (Input.GetButtonDown("GearUp"))
                {
                    ShifterManualUp();
                }
                else if (Input.GetButtonDown("GearDown"))
                {
                    ShifterManualDown();
                }
            }

            //Change View
            if (Input.GetButtonDown("ChangeView") && !isCrashed)
            {
                cam_Follow.SetActive(!cam_Follow.activeInHierarchy);
                cam_OnBoard.SetActive(!cam_OnBoard.activeInHierarchy);

                canvas_OnBoard.SetActive(cam_OnBoard.activeInHierarchy);
                canvas_Screen.SetActive(cam_Follow.activeInHierarchy);
            }

            //Head Light
            if (Input.GetButtonDown("HeadLights"))
            {
                headLight.SetActive(!headLight.activeInHierarchy);
            }

            //Right Light
            if (Input.GetButtonDown("RigthIndicator"))
            {
                turnLeftLight.SetActive(false);
                turnRightLight.SetActive(false);

                StopAllCoroutines();

                isRigthLightIndicator = !isRigthLightIndicator;
                if (isRigthLightIndicator)
                {
                    isLeftLightIndicator = false;
                    StartCoroutine(FlickerLight(turnRightLight));
                }
            }

            //Left Light
            if (Input.GetButtonDown("LeftIndicator"))
            {
                turnRightLight.SetActive(false);
                turnLeftLight.SetActive(false);

                StopAllCoroutines();

                isLeftLightIndicator = !isLeftLightIndicator;
                if (isLeftLightIndicator)
                {
                    isRigthLightIndicator = false;
                    StartCoroutine(FlickerLight(turnLeftLight));
                }
            }
        }


    }


    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (!useTouchControls)
        {
            if (currentTime >= 3)
            {
                GetInput();
                //   Debug.Log("Current time: " + currentTime);
            }
            // Debug.Log("Horizontal: " + horizontalInput);
            // Debug.Log("Vertical: " + verticalInput);
        }



        Steer();
        UpdateRearWheel();
        UpdateFrontWheel();
        Stabilizer();
        EnginePower();
        if (isAutomatic)
        {
            ShifterAuto();
        }

        if (!isCrashed)
        {
            rb.drag = (verticalInput <= 0f && speed > 10f) ? 0.05f : 0f;
        }


        speed = rb.velocity.magnitude * 3.6f;

    }

    void MobileInput()
    {
        if (useTouchControls)
        {
            if (reversePTI.buttonPressed)
            {
                Brake(true);
                brakeLight.SetActive(true);
            }
            else
            {
                Brake(false);
                brakeLight.SetActive(false);
            }
        }
    }



    void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

    }

    public void LoseInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput * bonusSteering;
        steeringAngle += (speed > 10f) ? 0f : steeringAngle * (speed / 10f);
        frontWheelCollider.steerAngle = Mathf.Lerp(frontWheelCollider.steerAngle, steeringAngle, 2f * Time.deltaTime);


        float angleHandlebar = Mathf.Lerp(steeringAngle, 0, speed / 300f);
        handlebar.transform.localRotation = Quaternion.Lerp(handlebar.transform.localRotation, handlebarStartRot * Quaternion.Euler(0, angleHandlebar, 0), 2f * Time.deltaTime);

        float angleVelocity = Mathf.Lerp(0, 45f, speed / 50f);
        float inclinationAngle = Mathf.Lerp(0, angleVelocity, Mathf.Abs(steeringAngle / maxSteerAngle));


        Quaternion meshRot = this.transform.rotation * Quaternion.Euler(0f, 0f, -inclinationAngle * horizontalInput);
        motorcycleMesh.transform.rotation = Quaternion.Lerp(motorcycleMesh.transform.rotation, meshRot, 2f * Time.deltaTime);


    }

    private void EnginePower()
    {
        acceleration = verticalInput > 0 ? verticalInput : rearWheelCollider.rpm <= 1 && speed < 5 ? verticalInput * 0.1f : 0;
        acceleration = acceleration * bonusAcceleration;


        if (IsGrounded())
        {
            if (engineRpm < maxEngineRpm + 100)
            {
                engineRpm = Mathf.Lerp(engineRpm, 1000f + Mathf.Abs(rearWheelCollider.rpm) * (gearRatio[currentGear] * finalDrive), 10 * Time.deltaTime);
                totalPower = enginePower.Evaluate(engineRpm) * finalDrive * acceleration;

                rearWheelCollider.motorTorque = (verticalInput == 0 || speed > maxSpeed) ? 0 : currentGear == gearRatio.Length - 1 ? totalPower / 3 : totalPower;

            }
            else
            {
                rearWheelCollider.motorTorque = 0;
                engineRpm -= 50f;
            }
        }
        else
        {
            rearWheelCollider.motorTorque = 0;
            engineRpm -= 5f;
        }
    }

    private void ShifterAuto()
    {
        if (!IsGrounded()) return;

        if (engineRpm > maxEngineRpm && currentGear < gearRatio.Length - 1)
        {
            currentGear++;
        }
        if (currentGear > 0)
        {
            minEngineRpm = 2000f + Mathf.Abs(rearWheelCollider.rpm) * (gearRatio[currentGear - 1] * finalDrive);
            if (minEngineRpm < maxEngineRpm)
            {
                currentGear--;
                if (!isBraking)
                {
                    cmpAudio.DownGear();
                }

            }
        }
    }

    private void ShifterManualUp()
    {
        if (!IsGrounded()) return;

        if (currentGear < gearRatio.Length - 1)
        {
            currentGear++;
        }
    }

    private void ShifterManualDown()
    {
        if (!IsGrounded()) return;

        if (currentGear > 0)
        {
            minEngineRpm = 2000f + Mathf.Abs(rearWheelCollider.rpm) * (gearRatio[currentGear - 1] * finalDrive);
            if (minEngineRpm < maxEngineRpm)
            {
                currentGear--;
                if (currentGear == 0 && !isBraking && verticalInput <= 0)
                {
                    cmpAudio.DownGear();
                }
            }
        }
    }

    public bool IsGrounded()
    {
        if (rearWheelCollider.isGrounded && frontWheelCollider.isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Brake(bool brake)
    {
        isBraking = brake;
        float bBraking = brake ? bonusBraking : 0f;
        // Debug.Log("Did Brake: " + isBraking);
        if (brake)
        {
            rb.mass = 500f;
            rearWheelCollider.brakeTorque = rb.mass * 5 * bBraking;
            frontWheelCollider.brakeTorque = rb.mass * 5 * (bBraking / 2);

        }
        else
        {
            rearWheelCollider.brakeTorque = 0;
            frontWheelCollider.brakeTorque = 0;
            rb.mass = 200f;
            brakeLight.SetActive(false);
        }
    }

    void UpdateRearWheel()
    {
        Quaternion rot;
        Vector3 pos;
        rearWheelCollider.GetWorldPose(out pos, out rot);

        pos.y += offset;
        offset_Wheel.transform.position = pos;

        Vector3 targetDir = offset_Wheel.transform.position - hinge_Rear.transform.position;
        float angle = Vector3.SignedAngle(targetDir, -hinge_Rear.transform.forward, hinge_Rear.transform.right);


        if (angle < 0)
        {
            susp_Rear.transform.localRotation = Quaternion.Euler(-angle, susp_Rear.transform.localRotation.y, susp_Rear.transform.localRotation.z);

        }
        else
        {
            susp_Rear.transform.localRotation = Quaternion.Euler(0, susp_Rear.transform.localRotation.y, susp_Rear.transform.localRotation.z);
        }

        wheel_Rear.transform.Rotate(wheel_Rear.transform.InverseTransformVector(wheel_Rear.transform.right) * rearWheelCollider.rpm * 2 * Mathf.PI / 60.0f * Time.deltaTime * Mathf.Rad2Deg, Space.Self);
    }

    void UpdateFrontWheel()
    {
        Quaternion rot;
        Vector3 pos;
        frontWheelCollider.GetWorldPose(out pos, out rot);

        float offset = pos.y - hinge_Front.transform.position.y;
        susp_Front.transform.localPosition = startLocalPos + new Vector3(0, offset, 0);
        wheel_Front.transform.Rotate(wheel_Front.transform.InverseTransformVector(wheel_Front.transform.right) * frontWheelCollider.rpm * 2 * Mathf.PI / 60.0f * Time.deltaTime * Mathf.Rad2Deg, Space.Self);
    }

    void Stabilizer()
    {
        if (!isCrashed)
        {
            Vector3 axisFromRotate = Vector3.Cross(transform.up, Vector3.up);

            Vector3 torqueForce = axisFromRotate.normalized * axisFromRotate.magnitude * 50;
            torqueForce.x = torqueForce.x * 0.4f;

            torqueForce -= rb.angularVelocity;
            rb.AddTorque(torqueForce * rb.mass * 0.02f, ForceMode.Impulse);


            float rpmSign = Mathf.Sign(engineRpm) * 0.02f;
            if (rb.velocity.magnitude > 1.0f && frontWheelCollider.isGrounded && rearWheelCollider.isGrounded)
            {
                rb.angularVelocity += new Vector3(0, horizontalInput * rpmSign, 0);
            }
        }
    }


    IEnumerator FlickerLight(GameObject obj)
    {

        while (true)
        {
            obj.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            obj.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }

    }


    private void OnCollisionEnter(Collision collision)
    {


        // Debug.Log(collision.gameObject.name);


        Vector3 collisionForce = collision.impulse / Time.fixedDeltaTime;

        if (collisionForce.magnitude > 100f && speed > 50f)
        {
            isCrashed = true;
            cmpRagdollRider.SetRagdoll(true);
            rb.drag = 0.5f;
            if (cam_OnBoard.activeInHierarchy)
            {
                cam_Follow.transform.position = this.transform.position + new Vector3(0, 2, -2);
                // Debug.Log(cam_Follow.transform.position);
                cam_Follow.SetActive(true);
                cam_OnBoard.SetActive(false);
            }
        }
    }

    void PauseGame()
    {
        if (gameIsPaused)
        {
            menu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1;
            menu.SetActive(false);
        }
    }




    private void OnCollisionStay(Collision collision)
    {
        //  Debug.Log(collision.collider.name);
    }

    private void OnCollisionExit(Collision collision)
    {
        // Debug.Log(collision.collider.name);

    }
}
