using System;
using System.Collections;
using UnityEngine;

public class Car : MonoBehaviour
{

    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    private float horizontalInput;
    private float verticalInput;
    private bool isBreaking;

    private float currentBreakForce;
    private float steeringAngle;

    private float currentSpeed;

    public bool arrived;
    public GameObject passanger; // when arrive at destinaion turn it on

    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteeringAngle;

    [Header("Wheel Colliders")]
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider backLeftWheelCollider;
    [SerializeField] private WheelCollider backRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform backLeftWheelTransform;
    [SerializeField] private Transform backRightWheelTransform;

    [Header("Lights")]
    [SerializeField] private GameObject carLights;

    private GameObject system;
    private PassagerSpawner pass;
    private bool turnLights;

    public Vector3 com;
    public Rigidbody rb;

    private int money;
    public HudManager HUD;
    public Tutorial tut;
    private bool phoneActive;

    private Health myHealth;
    bool stopSteer;
    bool canMove;
    bool fuck;

    float v_slide;
    int bonusMoney;
    [Range(0, 1)]
    public float currentTimeInCar = 1f;

    public float timeToUpdateInCar = 60f;
    private float timeRateInCar;

    bool once = true;
    public GameObject mecLocation;
    public OpenWheatherMapAPI OpenWheatherInfo;

    [HideInInspector]public bool onceAndDone = true;
    bool stopSteerAUX;

    bool firstDeath = true;
    void Start()
    {
        Cursor.visible = false;
        timeRateInCar = 1.0f / timeToUpdateInCar;
        canMove = true;
        stopSteer = false;
        myHealth = gameObject.GetComponent<Health>();
        phoneActive = false;
        money = 0;
        //system = GameObject.FindGameObjectWithTag("System").GetComponent<PassagerSpawner>();
        turnLights = true;
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
        system = GameObject.FindGameObjectWithTag("System");
        pass = system.GetComponent<PassagerSpawner>();
    }
    private void FixedUpdate()
    {
        if(HUD.startMoving == true && canMove == true)
        {
            if (myHealth.GetCurrentHealthNormalized() <= 0.5f && stopSteerAUX == false)
            {
                stopSteer = true;
            }
            else
            {
                stopSteer = false;
            }
        
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            currentSpeed = rb.velocity.magnitude;
        }

        
    }
    private void Update()
    {
        if (tut.enabled)
            fuck = tut.canClickArrow;
        if (Input.GetKeyDown(KeyCode.H) && canMove)
        {
            carLights.SetActive(turnLights);
            turnLights = !turnLights;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && fuck == true)
        {
            //celular vai p cima

            HUD.PhoneMove(true);
        }
        if (Input.GetKeyDown(GameInputManager.GM.hidePhone))
        {
            //celular vai p cima
            HUD.MakeSelected();
            HUD.ExitPhone();
            HUD.PhoneMove(false);
            //Cursor.visible = false;
        }

        if(passanger != null)
        {
            CheckPlayerIn();
        }
        if(onceAndDone == true)
            CheckDieState();
    }

    private void CheckPlayerIn()
    {
        if (currentTimeInCar <= 0)
        {
            HUD.UpdateBoxWhenInCar();
            currentTimeInCar = 1f;
        }
        else
        {
            currentTimeInCar -= timeRateInCar * Time.deltaTime;
        }
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
       
        isBreaking = Input.GetKey(GameInputManager.GM.breakCar);
    }
   
    private void ResetWheels()
    {
        frontRightWheelCollider.brakeTorque = 0f;
        frontLeftWheelCollider.brakeTorque = 0f;
        backLeftWheelCollider.brakeTorque = 0f;
        backRightWheelCollider.brakeTorque = 0f;
    }
    public bool onceText = true;
    void CheckDieState()
    {
        if(myHealth.currentHealth <= 0)
        {
            if(onceText == true)
            {
                string t;
                if (firstDeath == true)
                {
                     t = "HAHA! Looks like you destroyed your car... I will help you this time..." +
                    " Just wait and see!";
                }
                else
                {
                     t = "HAHA! Looks like you destroyed your car... You will have to pay this time..." +
                  " Just wait! OK?";
                }
                  
                HUD.ShowSpecialText(t, "Mechanical");
                if (passanger != null)
                {
                    GameObject o = this.passanger;
                    o.GetComponent<WaypointNavegator>().KillDestination();
                    pass.passagersInGameList.Remove(o);
                    Destroy(o.gameObject);
                    this.GetComponent<Destinator>().ResetBool();
                    arrived = false;
                    passanger = null;
                }
                onceText = false;
            }

           
            if(!HUD.textBox.activeInHierarchy && onceAndDone == true)
            {
                HUD.FadeAnimation(true);
                StartCoroutine(ChangeCarWhenDead());
                onceAndDone = false;
            }
               
         
            
        }
    }
    IEnumerator ChangeCarWhenDead()
    {
       
        yield return new WaitForSeconds(2f);
        this.transform.position = mecLocation.transform.position;
        this.transform.rotation = mecLocation.transform.rotation;
        rb.constraints = RigidbodyConstraints.FreezePositionX;
        rb.constraints = RigidbodyConstraints.FreezePositionZ;

        yield return new WaitForSeconds(3f);

        if (pass.passagersInGameList.Count ==0)
            StartCoroutine(SpawnOtherPassager());
        HUD.FadeAnimation(false);
        rb.constraints = RigidbodyConstraints.None;

    }
    private void HandleMotor()
    {
        if(stopSteer == false)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce * myHealth.GetCurrentHealthNormalizedToVelocity();
            frontRightWheelCollider.motorTorque = verticalInput * motorForce * myHealth.GetCurrentHealthNormalizedToVelocity();
        }
        else
        {
            frontLeftWheelCollider.motorTorque = 0f;
            frontRightWheelCollider.motorTorque = verticalInput * (motorForce*10f) * myHealth.GetCurrentHealthNormalizedToVelocity();
        }
      
        currentBreakForce = isBreaking ? breakForce : 0f;
        if (isBreaking == true)
        {
            ApplyBreak();
        }
        else
        {
            ResetWheels();
        }
    }
    private void ApplyBreak()
    {
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        backLeftWheelCollider.brakeTorque = currentBreakForce;
        backRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        steeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
        //Debug.Log("Angle:" + frontLeftWheelCollider.steerAngle);
    }
    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(backLeftWheelCollider, backLeftWheelTransform);
        UpdateSingleWheel(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTranform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTranform.rotation = rot;
        wheelTranform.position = pos;
    }
    public void SetFDeath(bool f)
    {
        firstDeath = f;
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "HumanTrigger")
        {
            arrived = true;

        }
        if(other.tag == "AI_Destination") 
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            arrived = false;
            // passanger.GetComponent<WaypointNavegator>().detectorObject.SetActive(false);
            Transform childOther = other.transform.GetChild(0).transform;
            Debug.Log(childOther.gameObject.name);
            passanger.transform.position = childOther.position; /*new Vector3(other.transform.position.x,
                passanger.transform.position.y,other.transform.position.z);*/
            passanger.GetComponent<WaypointNavegator>().isPassager = false;
            passanger.GetComponent<WaypointNavegator>().aiState = HumanoidState.PathWalking;
            
           passanger.gameObject.SetActive(true);
            //Destroy(passanger);
            //money += 25;
            if(OpenWheatherInfo.temp_ >= 30)
            {
                if (HUD.hasAirConditioner == false)
                    bonusMoney = -10;
                else
                    bonusMoney = 25;
            }
            else
            {
                if (HUD.hasAirConditioner == true)
                    bonusMoney = 5;
                else
                    bonusMoney = 0;
            }
       
            pass.passagersInGameList.Remove(passanger);
            float finalMoney = (40 * (HUD.QualityLevelNormalized()*2f)*  (myHealth.GetCurrentHealthNormalized())*1.5f)+bonusMoney;
            finalMoney = Mathf.Clamp(finalMoney, 0, finalMoney);
            HUD.ShowNotification(finalMoney);
            HUD.UpdateMoney(Mathf.RoundToInt(finalMoney));
            this.GetComponent<Destinator>().ResetBool();
            passanger = null;
            StartCoroutine(SpawnOtherPassager());
            //pass.SpawnOnePassager();
        }
        if(other.tag == "MechanicalStore" && GetCarSpeed()*3.6f <=1.5f && passanger == null)
        {
            //lança por script no other q to aki
            Debug.Log("NA LOJA E PARADO");
            //canMove = false;
            other.GetComponent<MeshRenderer>().enabled = false;
            other.GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<MechanicalInitializer>().InitializeMechanical(other.gameObject,this.gameObject,firstDeath);
        }
        else if(other.tag == "MechanicalStore" && GetCarSpeed() * 3.6f <= 1.5f && passanger != null)
        {
            if(once == true && !HUD.textBox.activeInHierarchy)
            {
                once = false;
                string tex = "I cant help you... You are in the middle of a ride! Come back when you are done!";
                HUD.ShowSpecialText(tex, "Mechanical");
                
            }
           
        }
        if(other.tag == "Beach")
        {
            stopSteerAUX = true;
            if (myHealth.GetCurrentHealthNormalized() <= 0.5f)
                motorForce = 1800f;
            else
                motorForce = 800f;
            stopSteer = false;
        }

    }
    public void SetCanMove(bool move)
    {
        canMove = move;
    }
    private IEnumerator SpawnOtherPassager()
    {
        yield return new WaitForSeconds(2f);
        pass.SpawnOnePassager();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "HumanTrigger")
        {
            arrived = false;
        }
        if(other.tag == "MechanicalStore")
        {
            once = true;
        }
        if (other.tag == "Beach")
        {
            motorForce = 300f;
            stopSteerAUX = false;
            

        }

    }
    public int PlayerMoney()
    {
        return this.money;
    }
    public float GetCarSpeed()
    {
        return this.currentSpeed;
    }

}
