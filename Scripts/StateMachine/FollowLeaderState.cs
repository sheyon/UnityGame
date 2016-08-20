using UnityEngine;

public class FollowLeaderState : ITargetState
{
    private readonly StatePatternTarget target;
    private Vector3 leaderNoFly;

    public FollowLeaderState(StatePatternTarget statePatternTarget)
    {
        target = statePatternTarget;
    }

    public void UpdateState()
    {
        target.navMeshAgent.speed = target.followSpeed;
        target.navMeshAgent.stoppingDistance = target.followStoppingDist;

        //If I have a leader, follow them.
        if (target.leader != null)
        {
            leaderNoFly = new Vector3(target.leader.transform.position.x, 0.5f, target.leader.transform.position.z);
            Following();
        }

        //If my leader is destroyed, revert to Idle behaviors.
        else if (target.leader == null)
        {
            Idle();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target.player && target.fearPlayer == true)
        {
            target.fearedObject = other.gameObject;
            Repulsed();
        }
        if (other.gameObject.CompareTag("Feared"))
        {
            target.fearedObject = other.gameObject;
            Repulsed();
        }
        if (other.gameObject.CompareTag("Lure") && target.attractable == true)
        {
            Attracted();
        }
        if (other.gameObject == target.leader)
        {
            Following();
        }
    }

    public void Idle()
    {
        target.currentLocation = target.transform.position;
        target.currentState = target.idleState;
    }

    public void FollowLeader()
    {
        Debug.Log("Can't transition to same state: FollowLeader");
    }

    public void Repulsed()
    {
        target.currentState = target.repulsedState;
    }

    public void Patrol()
    {
        Debug.Log("A target that is in a FollowLeader state cannot be patrolling");
    }

    public void Attracted()
    {
        target.currentState = target.attractedState;
    }

    // -----

    //This method is needed so targets can begin following their leaders ONLY when certain conditions are met.
    void Following()
    {
        if (Vector3.Distance(target.transform.position, target.leader.transform.position) >= target.beginFollowingDist)
        {
            target.navMeshAgent.ResetPath();
            Idle();
        }
        else
        {
            target.navMeshAgent.SetDestination(leaderNoFly);
        }
    }
}