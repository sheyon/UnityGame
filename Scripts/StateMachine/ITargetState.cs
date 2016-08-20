using UnityEngine;

public interface ITargetState
{
    void UpdateState();

    void OnTriggerEnter(Collider other);

    void Idle();

    void FollowLeader();

    void Repulsed();

    void Patrol();

    void Attracted();
}
