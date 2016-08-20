using UnityEngine;

public class StatePatternTarget : MonoBehaviour
{
    //Multi-behavior parameters
    [HideInInspector] public Vector3 currentLocation;
    [HideInInspector] public GameObject player;
    
    //Repulsion parameters
    public bool fearPlayer;
    public bool freezeInPlace;
    [HideInInspector] public GameObject fearedObject;
    [HideInInspector] public float fearedSpeed = 5f;
    [HideInInspector] public float fearedDistance = 5f;
    [HideInInspector] public Vector3 previousLocation;

    //Attracted parameters
    public bool attractable;

    //FollowLeader parameters
    public GameObject leader;
    public float followSpeed = 3.65f;
    public float followStoppingDist = 3f;
    public float beginFollowingDist = 10f;

    //Idle parameters
    public float idleSpeed = 1.5f;
    public float idleStoppingDist = 0f;
    public float idleRange = 2f;

    //Patrol parameters
    public bool startPatrolling;
    public bool loopWaypoints;
    public GameObject[] allWaypoints;
    public GameObject despawnPoint;
    public float patrolSpeed = 3.5f;

    [HideInInspector] public ITargetState currentState;
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public PatrolState patrolState;
    [HideInInspector] public FollowLeaderState followLeaderState;
    [HideInInspector] public RepulsedState repulsedState;
    [HideInInspector] public AttractedState attractedState;
    [HideInInspector] public NavMeshAgent navMeshAgent;

    private void Awake()
    {
        idleState = new IdleState(this);
        patrolState = new PatrolState(this);
        followLeaderState = new FollowLeaderState(this);
        repulsedState = new RepulsedState(this);
        attractedState = new AttractedState(this);

        player = GameObject.FindGameObjectWithTag("Player");
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        currentLocation = transform.position;
        DoIFearPlayer();
        StartingBehaviors();
    }

    public void DoIFearPlayer()
    {
        if (fearPlayer == true)
        {
            fearedObject = GameObject.FindGameObjectWithTag("Player");
        }
    }

    void StartingBehaviors()
    {
        //Valid starting behaviors are FollowLeader, Patrolling, Idle
        if (leader != null)
        {
            currentState = followLeaderState;
        }
        else if (startPatrolling == true)
        {
            currentState = patrolState;
        }
        else if (startPatrolling == false)
        {
            currentState = idleState;
        }
        else
        {
            Debug.Log("You have not set valid starting parameters; defaulting to Idle state");
        }
    }

    void Update()
    {
        currentState.UpdateState();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

}