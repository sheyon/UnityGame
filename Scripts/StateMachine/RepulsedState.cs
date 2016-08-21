using UnityEngine;

public class RepulsedState : ITargetState
{
    private readonly StatePatternTarget target;

    public RepulsedState(StatePatternTarget statePatternTarget)
    {
        target = statePatternTarget;
    }

    public void UpdateState()
    {
        Debug.Log("I am in RepulsedState!");
        target.navMeshAgent.speed = target.fearedSpeed;

        if (Vector3.Distance(target.transform.position, target.fearedObject.transform.position) < target.fearedDistance)
        {
            Flee();
        }

        if (Vector3.Distance(target.transform.position, target.fearedObject.transform.position) >= target.fearedDistance)
        {
            ReturnHome();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //TODO: Calming ball?
    }

    public void Idle()
    {
        target.currentState = target.idleState;
    }

    public void FollowLeader()
    {
        target.currentState = target.followLeaderState;
    }

    public void Repulsed()
    {
        Debug.Log("Can't transition to same state: Repulsed");
    }

    public void Patrol()
    {
        target.currentState = target.patrolState;
    }

    public void Attracted()
    {
        Debug.Log("A target that is being Repulsed should not be Attracted");
    }

    // -----

    void Flee()
    {
        target.navMeshAgent.ResetPath();

        //Look at player.
        Quaternion repulsorRotation = Quaternion.LookRotation(target.fearedObject.transform.position - target.transform.position);
        target.transform.rotation = Quaternion.Slerp(target.transform.rotation, repulsorRotation, target.fearedSpeed * Time.deltaTime);

        //Move backwards.
        if (target.fearedInPlace == false)
        {
            target.transform.position += -target.transform.forward * target.fearedSpeed * Time.deltaTime;
        }

    }

    void ReturnHome()
    {
        if (target.allWaypoints == null && target.leader == null)
        {
            //Return to original idle position
            target.navMeshAgent.SetDestination(target.previousLocation);
            if (target.navMeshAgent.remainingDistance <= .5f)
            {
                Idle();
            }
        }

        else if (target.leader != null)
        {
            //Return to parent
            target.navMeshAgent.SetDestination(target.leader.transform.position);
            if (target.navMeshAgent.remainingDistance <= .5f)
            {
                FollowLeader();
            }
        }

        else if (target.allWaypoints != null)
        {
            //If the target has waypoints assigned, it should continue as normal.
            Patrol();
        }
    }

}