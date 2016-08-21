using UnityEngine;

public class StatePatternTarget : MonoBehaviour
{
    //Multi-behavior parameters
    [HideInInspector] public Vector3 currentLocation;
    [HideInInspector] public GameObject player;
    
    //Repulsion parameters
    public bool fearPlayer;
    public bool fearedInPlace;
    [HideInInspector] public GameObject fearedObject;
    [HideInInspector] public float fearedDistance = 5f;
    [HideInInspector] public Vector3 previousLocation;

    //Attracted parameters
    public bool attractable;
    [HideInInspector] public GameObject lure;

    //FollowLeader parameters
    public GameObject leader;
    public float beginFollowingDist = 10f;

    //Idle parameters
    public float idleRange = 5f;
    public bool isPassive;

    //Patrol parameters
    public bool patrolOnStart;
    public bool loopWaypoints;
    public GameObject[] allWaypoints;
    public GameObject despawnPoint;

    //Speed
    [HideInInspector] public float fearedSpeed = 5f;
    [HideInInspector] public float attractedSpeed = 3f;
    [HideInInspector] public float followSpeed = 3.65f;
    [HideInInspector] public float idleSpeed = 1.5f;
    [HideInInspector] public float patrolSpeed = 3.5f;

    //StoppingDist
    [HideInInspector] public float attractedStoppingDist = 2f;
    [HideInInspector] public float followStoppingDist = 3f;
    [HideInInspector] public float idleStoppingDist = 0f;
    [HideInInspector] public float patrolStoppingDist = 0f;

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
        StartingBehaviors();
    }

    void StartingBehaviors()
    {
        //Valid starting behaviors are FollowLeader, Patrolling, Idle
        if (leader != null)
        {
            currentState = followLeaderState;
        }
        else if (patrolOnStart == true)
        {
            currentState = patrolState;
        }
        else if (patrolOnStart == false)
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