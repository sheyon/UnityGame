using UnityEngine;

public class PatrolState : ITargetState {

    private readonly StatePatternTarget target;
    private int i = 0;
    private bool doOnce;

    public PatrolState (StatePatternTarget statePatternTarget)
    {
        target = statePatternTarget;
    }

    public void UpdateState()
    {
        Debug.Log("I am in Patrol State");
        target.navMeshAgent.speed = target.patrolSpeed;
        GoToNextWaypoint();
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
    }

    public void Idle()
    {
        target.currentLocation = target.transform.position;
        target.currentState = target.idleState;
    }

    public void FollowLeader()
    {
        Debug.Log("A target that is patrolling waypoints cannot be in a FollowLeader state");
    }

    public void Repulsed()
    {
        target.previousLocation = target.transform.position;
        target.currentState = target.repulsedState;
    }

    public void Patrol()
    {
        Debug.Log("Can't transition to same state: Patrol");
    }

    public void Attracted()
    {
        target.currentState = target.attractedState;
    }

    //-----

    void GoToNextWaypoint()
    {
        if (i >= target.allWaypoints.Length)
        {
            if (target.loopWaypoints == true)
            {
                i = 0;
            }
            else if (target.loopWaypoints == false && target.despawnPoint != null)
            {
                //Go to despawn point.
                target.navMeshAgent.destination = target.despawnPoint.transform.position;
            }
            else if (target.loopWaypoints == false && target.despawnPoint == null)
            {
                Idle();
            }
            else
            {
                Debug.Log("Invalid patrol parameters; defaulting to waypoint loop");
                target.loopWaypoints = true;
            }
        }

        else
        {
            target.navMeshAgent.destination = target.allWaypoints[i].transform.position;

            if (Vector3.Distance(target.navMeshAgent.transform.position, target.allWaypoints[i].transform.position) < 1f)
            {
                i++;
            }
        }
    }

}
