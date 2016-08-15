using UnityEngine;

public class CycleWaypoints : MonoBehaviour
{
    public SpawnTrigger spawnTrigger;
    public GameObject[] allWaypoints;
    public bool loopWaypoints;
    [HideInInspector] public NavMeshAgent self;

    private int i = 0;


    void Start()
    {
        spawnTrigger = FindObjectOfType(typeof(SpawnTrigger)) as SpawnTrigger;
        allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");
        self = GetComponent<NavMeshAgent>();
        GoToNextWaypoint();
    }


    void GoToNextWaypoint()
    {
        if (i >= allWaypoints.Length)
        {
            if (loopWaypoints == true)
            {
                i = 0;
            }
            else
            {
                //Go to despawn point.
                self.destination = spawnTrigger.despawnPoint.transform.position;
            }
        }
        else
        {
            self.destination = allWaypoints[i].transform.position;

            if (Vector3.Distance(self.transform.position, allWaypoints[i].transform.position) < 1f )
            {
                i++;
            }
        }
    }


    void Update()
    {
        GoToNextWaypoint();
    }

}
