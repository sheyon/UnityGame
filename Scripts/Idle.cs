using UnityEngine;

public class Idle : MonoBehaviour {

    [HideInInspector] public FollowParent followParent;
    [HideInInspector] public Repulsion repulsion;
    public bool isIdling;
    public float wanderRange;
    private NavMeshAgent self;
    private Vector3 idleLocation;
    private Vector3 wanderPoint;
    private bool wanderPointReady;
    private bool doOnce;


    void Awake()
    {
        followParent = GetComponent<FollowParent>();
        repulsion = GetComponent<Repulsion>();
        self = GetComponent<NavMeshAgent>();
        wanderPointReady = false;
        wanderRange = 5.0f;
    }


    void RecordLocation()
    {
        //This is the location around which the object will wander in place.
        idleLocation = transform.position;
        isIdling = true;
    }


    void RecordNewLocation()
    {
        //For when the isIdling flag is not desired.
        idleLocation = transform.position;
    }


    void Wander()
    {
        self.stoppingDistance = 0f;
        self.speed = 1.5f;
        if (wanderPointReady == false)
        {
            wanderPoint = new Vector3(Random.Range(idleLocation.x - wanderRange, idleLocation.x + wanderRange), 0.0f, Random.Range(idleLocation.z - wanderRange, idleLocation.z + wanderRange));
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


    void SuspendIdling()
    {
        //Do nothing.
    }


    void OrphanedObject()
    {
        //This subroutine is for those objects who have no parent to follow.
        //It should be called if an object loses its parent, but it can be a catch-all.
        //It will cause the object to behave like an Idled object instead.

        if (isIdling == false)
        {
            RecordLocation();
        }

        if (isIdling == true)
        {
            Wander();
        }
    }


    void Update()
    {
        //For Idle-only Behaviors
        if (followParent == null && repulsion == null)
        {
            OrphanedObject();
        }

        //For Idle + Repulsion Behaviors
        else if (followParent == null && repulsion != null)
        {
            if (isIdling == false && repulsion.isFleeing == true)
            {
                SuspendIdling();
            }
            if (isIdling == true && repulsion.isFleeing == false)
            {
                if (doOnce == false)
                {
                    //This is needed because Repulsion behaviors will never trigger RecordLocation().
                    RecordNewLocation();
                    doOnce = true;
                }
                Wander();
            }
        }

        //For FollowParent + Idle Behaviors
        else if (followParent != null && repulsion == null)
        {
            if (followParent.leader != null)
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
            else if (followParent.leader == null)
            {
                OrphanedObject();
            }
        }

        //For FollowParent + Idle + Repulsion
        else if (followParent != null && repulsion != null)
        {
            if (followParent.leader == null)
            {
                //This condition could only happen once.
                if (isIdling == false && repulsion.isFleeing == false)
                {
                    if (doOnce == false)
                    {
                        //This is needed because Repulsion behaviors will never trigger RecordLocation().
                        RecordNewLocation();
                        doOnce = true;
                    }
                    isIdling = true;
                }
                if (isIdling == false && repulsion.isFleeing == true)
                {
                    SuspendIdling();
                }
                if (isIdling == true && repulsion.isFleeing == false)
                {
                    Wander();
                }
            }
            else
            {
                if (followParent.amFollowing == false && isIdling == false && repulsion.isFleeing == false)
                {
                    //Do nothing. The Repulsion script should try to reunite them.
                }
                if (followParent.amFollowing == false && repulsion.isFleeing == true)
                {
                    SuspendIdling();
                }
                if (followParent.amFollowing == true)
                {
                    isIdling = false;
                    repulsion.isFleeing = false;
                }
            }
        }

        else
        {
            print("Something does not add up!");
        }
    

    }
}
