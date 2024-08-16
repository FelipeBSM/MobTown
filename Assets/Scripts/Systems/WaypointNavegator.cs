using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum HumanoidState
{
    PathWalking, // keeps walkin all the way
    OnHold, // waiting for car
    PlaceWalking,
    FinalMoment// one destiny only
}

public class WaypointNavegator : MonoBehaviour
{
    // Start is called before the first frame update
   
    [Header("Waypoints Father")]
    [SerializeField]private GameObject currentWay;

    [SerializeField] private GameObject starObject;
    [SerializeField] public GameObject detectorObject;

    private GameObject playerReference;
    private Car player;
    private Destinator playerDestination;

    [Header("Agent Config")]
    [Range(0f,3f)]public float agentSpeed;
    [Range(0f, 3f)]public float agentLinkSpeed;

    public int chanceToOnHold;
    public int timeOnHold;
    public bool isPassager;
    private bool hasEntered =  false;
    public HumanoidState aiState;
    public string currentStreet; // AI current street
    public string destinationStreet; // AI future location street
    public string locationName;

    private GameObject[] destinationLocations; // array of possible destinations
    private GameObject finalDestination;

    public float coolDownTimerConst1, coolDownTimerConst2;
    private float coolDownTimer;
    public float timeToChangeState;

    private float timeRate;
    [SerializeField][Range(0.0f, 1.0f)] private float time;
    private NavMeshAgent agent;
    private Animator anim;
    private List<GameObject> pointsList = new List<GameObject>();

    public bool tutorial;

    void Awake()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
       
        for (int i = 0; i < currentWay.transform.childCount; i++)
        {
            pointsList.Add(currentWay.transform.GetChild(i).gameObject);
        }

        agent = GetComponent<NavMeshAgent>();
        anim = gameObject.GetComponent<Animator>();
        if (isPassager)
        {
            player = playerReference.GetComponent<Car>();
            playerDestination = playerReference.GetComponent<Destinator>();
        }
      
        timeRate = 1.0f / timeOnHold;
        time = 1f;
        //playerReference = gameObject.findOb
        //movementSpeed = agent.speed;
    }
    private void Start()
    {
        destinationLocations = GameObject.FindGameObjectsWithTag("AI_Destination");
        Debug.Log(destinationLocations.Length);
        if (isPassager)
            detectorObject.SetActive(false);
        int index = Random.Range(0, pointsList.Count);
        GameObject currPoint = pointsList[index];
        Debug.Log(currPoint.name);

        coolDownTimer = Random.Range(coolDownTimerConst1,coolDownTimerConst2);
        timeToChangeState = coolDownTimer;

        if(tutorial == false)
            aiState = HumanoidState.PathWalking;
        else { aiState = HumanoidState.OnHold; }
           // initialize walking on a certain path
        agent.speed = agentSpeed;
        agent.destination = currPoint.transform.position;

       
    }
    // Update is called once per frame
    void Update()
    {
        if (aiState != HumanoidState.OnHold && aiState != HumanoidState.PlaceWalking
            && isPassager && !agent.isOnOffMeshLink) //&& only ai
        {
            if(timeToChangeState > 0)
            {
                timeToChangeState -= Time.deltaTime;
            }
            else if(timeToChangeState <= 0)
            {
                ChangeAIState();
                coolDownTimer = Random.Range(coolDownTimerConst1, coolDownTimerConst2);
                timeToChangeState = coolDownTimer;
            }
        }
        if(aiState == HumanoidState.PathWalking)
        {
            agent.isStopped = false;
            anim.SetBool("onHold", false);
            starObject.SetActive(false);
            if(isPassager)
                detectorObject.SetActive(false);

            MoveHumanAI();
            
        }
        else if(aiState == HumanoidState.OnHold)
        {
            //change animation, clear target and if car is in front go inside
            agent.isStopped = true;
            anim.SetBool("onHold", true);
            starObject.SetActive(true);
            detectorObject.SetActive(true);
            transform.LookAt(playerReference.transform);
           
            StopMovement();
           
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("onHold", false);
            agent.destination = playerReference.transform.position;
            if(playerReference.GetComponent<Car>().arrived == false)
            {
                aiState = HumanoidState.OnHold;
            }

        }
        ChangeVelocity();


    }

    private void StopMovement()
    {

        if(tutorial == false)
            time -= timeRate*Time.deltaTime;
       
        if(time <= 0 && tutorial == false)
        {
            aiState = HumanoidState.PathWalking;
            playerDestination.DirectMessage();
            time = 1f;   
        }
        else
        {
            if (playerReference.GetComponent<Car>().arrived == true)
            {
                aiState = HumanoidState.PlaceWalking;
            }
            
        }

    }

    private void MoveHumanAI()
    {
        float dist = agent.remainingDistance;
        if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
        {

            int index = Random.Range(0, pointsList.Count);
            GameObject currPoint = pointsList[index];
            //Debug.Log(currPoint.name);
            agent.destination = currPoint.transform.position;
        }

       
    }


    private void ChangeVelocity()
    {
        if (agent.isOnOffMeshLink)
        {

            agent.speed = agentSpeed - 2;
        }
        else
        {
            agent.speed = agentSpeed;
        }
    }
   
    public void ChangeAIState()
    {
      
        float percentageChance = 0.6f;
        if(Random.value < percentageChance)
        {
            aiState = HumanoidState.OnHold;
        }
    }
    private void RandomizeDestination()
    {
        Debug.Log(destinationLocations.Length);
        int random = Random.Range(0, destinationLocations.Length);
        Debug.Log(random);
        finalDestination = destinationLocations[random];
        finalDestination.GetComponent<MeshRenderer>().enabled = true;
        finalDestination.GetComponent<BoxCollider>().enabled = true;
        destinationStreet = finalDestination.GetComponent<DestinationsIntel>().GetStreetName();
        locationName = finalDestination.GetComponent<DestinationsIntel>().GetLocationName();
    }
    public void KillDestination()
    {
        finalDestination.GetComponent<MeshRenderer>().enabled = false;
        finalDestination.GetComponent<BoxCollider>().enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isPassager)
        {
            if (other.tag == "PlayerDoor" && hasEntered == false && aiState == HumanoidState.PlaceWalking)
            {
                Debug.Log("InCar");
                player.passanger = this.gameObject;

                player.currentTimeInCar = 1f;
                RandomizeDestination();
                hasEntered = true;
                playerDestination.SetDestination(finalDestination,destinationStreet,locationName); //passamo destino srteado e nome da rua
                this.gameObject.SetActive(false);
            }
            else if(other.tag == "StreetIntel" && aiState == HumanoidState.OnHold)
            {
                currentStreet = other.gameObject.GetComponent<StreetInformation>().GetStreetName();
                playerDestination.SetCurrentStreet(currentStreet,true);
                Debug.Log(currentStreet);
            }
        }
        else
        {
            if(other.tag == "DeathKillPass" && !isPassager)
            {
                Debug.LogError("THCAUUUUUUUUUUUUU");
                Destroy(this.gameObject);
            }
        }
        
    }
    //{
    //    if(other.tag == "PedestrianJunc" && hadPassed == false)
    //    {
    //        Debug.Log("Here");
    //        hadPassed = true;
    //        //get the info if juction is free or not, and the go
    //        this.gameObject.GetComponent<NavMeshAgent>().enabled = false; //stop navegating
    //        crossingDestination = other.GetComponent<PedestrianCrossing>().GetDestination();
    //        isOnJuction = true;
    //    }
    //}



}
