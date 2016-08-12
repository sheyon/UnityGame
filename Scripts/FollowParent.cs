using UnityEngine;

public class FollowParent : MonoBehaviour {

    //If you attach a FollowParent script, you SHOULD attach an Idle script as well.
    //Otherwise, if the NMA gets out of range of its Parent, it will only stand still.

    public GameObject parent;
    public Idle idle;
    public bool amFollowing;
    private NavMeshAgent self;
    private float beginFollowDistance;
    private float followTolerance;
    private Vector3 parentNoFly;

    
    void Start()
    {
        self = GetComponent<NavMeshAgent>();
        idle = GetComponent<Idle>();
        beginFollowDistance = 10f;
        DoIHaveAParent();
    }


    void DoIHaveAParent()
    {
        //If I don't have a parent, begin Idle script. Otherwise, continues to Update.
        if (parent == null && idle != null)
        {
            amFollowing = false;
            idle.isIdling = true;
        }

        //If you see this error, fix it, dummy!
        if (parent == null && idle == null)
        {
            print("I have no parent and no other behaviors set!");
        }
    }


    void Following()
    {
        if (amFollowing == true)
        {
            self.stoppingDistance = 3.0f;
            self.speed = 3.5f;

            if (Vector3.Distance(transform.position, parent.transform.position) >= beginFollowDistance)
            {
                amFollowing = false;
                self.ResetPath();
            }
            else
            {
                self.SetDestination(parentNoFly);
            }
        }

        if (amFollowing == false)
        {
            if (Vector3.Distance(transform.position, parent.transform.position) <= beginFollowDistance)
            {
                amFollowing = true;
            }
        }
    }


    void Update()
    {
        //If I do have a parent, continue as normal.
        if (parent != null)
        {
            parentNoFly = new Vector3(parent.transform.position.x, 0.5f, parent.transform.position.z);
            Following();
        }
    }
}
