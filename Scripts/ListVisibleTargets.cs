using UnityEngine;

public class ListVisibleTargets : MonoBehaviour
{
    public Collider[] visibleTargets;

    public ListVisibleTargets(Collider[] targets)
    {
        visibleTargets = targets;
    }
}
