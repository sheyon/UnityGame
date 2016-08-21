using UnityEngine;

public class IdleState : ITargetState
{
    private readonly StatePatternTarget target;
    private Vector3 wanderPoint;
    private bool wanderPointReady = false;

    public IdleState(StatePatternTarget statePatternTarget)
    {
        target = statePatternTarget;
    }

    public void UpdateState()
    {
        if (target.isPassive == true)
        {
            if (Vector3.Distance(target.transform.position, target.player.transform.position) < 5f)
            {
                //Look at player
                Quaternion idleRotation = Quaternion.LookRotation(target.player.transform.position - target.transform.position);
                target.transform.rotation = Quaternion.Slerp(target.transform.rotation, idleRotation, target.idleSpeed * Time.deltaTime);
            }
        }
        else
        {
            Wander();
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
            target.lure = other.gameObject;
            Attracted();
        }
        if (other.gameObject == target.leader)
        {
            FollowLeader();
        }
    }

    public void Idle()
    {
        Debug.Log("Can't transition to same state: Idle");
    }

    public void FollowLeader()
    {
        target.currentState = target.idleState;
    }

    public void Repulsed()
    {
        target.previousLocation = target.transform.position;
        target.currentState = target.repulsedState;
    }

    public void Patrol()
    {
        target.currentState = target.patrolState;
    }

    public void Attracted()
    {

        target.currentState = target.attractedState;
    }

    // -----

    void Wander()
    {
        target.navMeshAgent.speed = target.idleSpeed;
        target.navMeshAgent.stoppingDistance = target.idleStoppingDist;
        if (wanderPointReady == false)
        {
            wanderPoint = new Vector3(Random.Range(target.currentLocation.x - target.idleRange, target.currentLocation.x + target.idleRange), 0.0f, Random.Range(target.currentLocation.z - target.idleRange, target.currentLocation.z + target.idleRange));
            wanderPointReady = true;
        }
        target.navMeshAgent.SetDestination(wanderPoint);
        AreWeThere();
    }


    void AreWeThere()
    {
        if (target.navMeshAgent.remainingDistance <= 0.5f || target.navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid || target.navMeshAgent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            wanderPointReady = false;
        }
    }

    void Passive()
    {
        target.navMeshAgent.SetDestination(target.currentLocation);
    }
}