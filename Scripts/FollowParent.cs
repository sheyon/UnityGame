using UnityEngine;

public class FollowParent : MonoBehaviour {

    //If you attach a FollowParent script, you SHOULD attach an Idle script as well.
    //Otherwise, if the NMA gets out of range of its Parent, it will only stand still.

    [HideInInspector] public Idle idle;
    public GameObject leader;
    public bool amFollowing;
    private NavMeshAgent self;
    private float beginFollowDistance;
    private Vector3 leaderNoFly;
    private bool doOnce;

    
    void Start()
    {
        self = GetComponent<NavMeshAgent>();
        idle = GetComponent<Idle>();
        beginFollowDistance = 10f;
        DoIHaveALeader();
    }


    void DoIHaveALeader()
    {
        //If I don't have a leader, begin Idle script. Otherwise, continues to Update.
        if (leader == null && idle != null)
        {
            amFollowing = false;
            idle.isIdling = true;
        }

        //If you see this error, fix it, dummy!
        if (leader == null && idle == null)
        {
            print("I have no leader and no other behaviors set!");
        }
    }


    void Following()
    {
        if (amFollowing == true)
        {
            self.stoppingDistance = 3.0f;
            self.speed = 3.5f;

            if (Vector3.Distance(transform.position, leader.transform.position) >= beginFollowDistance)
            {
                amFollowing = false;
                self.ResetPath();
            }
            else
            {
                self.SetDestination(leaderNoFly);
            }
        }

        if (amFollowing == false)
        {
            if (Vector3.Distance(transform.position, leader.transform.position) <= beginFollowDistance)
            {
                amFollowing = true;
            }
        }
    }


    void Update()
    {
        //If I do have a parent, continue as normal.
        if (leader != null)
        {
            leaderNoFly = new Vector3(leader.transform.position.x, 0.5f, leader.transform.position.z);
            Following();
        }
        else
        {
            //If the object's parent is permanently destroyed, it should revert to Idle behaviors.
            if (doOnce == false)
            {
                DoIHaveALeader();
                doOnce = true;
            }
        }
    }
}
