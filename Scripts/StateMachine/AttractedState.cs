using UnityEngine;

public class AttractedState : ITargetState
{
    private readonly StatePatternTarget target;

    public AttractedState(StatePatternTarget statePatternTarget)
    {
        target = statePatternTarget;
    }

    public void UpdateState()
    {
        target.navMeshAgent.speed = target.attractedSpeed;
        target.navMeshAgent.stoppingDistance = target.attractedStoppingDist;

        if (target.lure != null)
        {
            target.navMeshAgent.SetDestination(target.lure.transform.position);

            //Look at lure
            Quaternion attractorRotation = Quaternion.LookRotation(target.lure.transform.position - target.transform.position);
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, attractorRotation, target.attractedSpeed * Time.deltaTime);
        }
        else
        {
            target.lure = null;
            target.navMeshAgent.ResetPath();
            ReturnHome();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //Do nothing
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
        Debug.Log("A target that is Attracted cannot be Repulsed");
    }

    public void Patrol()
    {
        target.currentState = target.patrolState;
    }

    public void Attracted()
    {
        Debug.Log("Cannot transition to same state: Attracted");
    }

    // -----

    void ReturnHome()
    {
        if (target.allWaypoints == null && target.leader == null)
        {
            //Return to original idle position
            target.navMeshAgent.SetDestination(target.currentLocation);
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