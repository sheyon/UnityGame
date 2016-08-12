using UnityEngine;

public class Idle : MonoBehaviour {

    public FollowParent followParent;
    public bool isIdling;
    private NavMeshAgent self;
    private Vector3 currentLocation;
    private Vector3 wanderPoint;
    private bool wanderPointReady;
    private float wanderRange;


    void Awake()
    {
        self = GetComponent<NavMeshAgent>();
        followParent = GetComponent<FollowParent>();
        wanderPointReady = false;
        wanderRange = 5.0f;
    }


    void RecordLocation()
    {
        currentLocation = transform.position;
        isIdling = true;
    }


    void Wander()
    {
        self.stoppingDistance = 0f;
        self.speed = 1.5f;
        if (wanderPointReady == false)
        {
            wanderPoint = new Vector3(Random.Range(currentLocation.x - wanderRange, currentLocation.x + wanderRange), 0.0f, Random.Range(currentLocation.z - wanderRange, currentLocation.z + wanderRange));
            wanderPointReady = true;
        }

        self.SetDestination(wanderPoint);
        AreWeThere();
    }


    void AreWeThere()
    {
        if (self.remainingDistance <= 0.5f || self.pathStatus == NavMeshPathStatus.PathInvalid || self.pathStatus == NavMeshPathStatus.PathPartial)
        {
            wanderPointReady = false;
        }
    }


    void Update()
    {
        //For FollowParent + Idle Behaviors
        if (followParent != null)
        {
            if (followParent.amFollowing == false && isIdling == false)
            {
                RecordLocation();
            }

            if (followParent.amFollowing == false && isIdling == true)
            {
                Wander();
            }
            if (followParent.amFollowing == true)
            {
                wanderPointReady = false;
                isIdling = false;
            }
        }

        //For Idle-only Behaviors
        if (followParent == null)
        {
            if (isIdling == false)
            {
                RecordLocation();
            }

            if (isIdling == true)
            {
                Wander();
            }
        }
    }
}
