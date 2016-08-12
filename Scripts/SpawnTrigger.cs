using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    public bool hasCompleted;
    public GameObject spawnPoint;
    public GameObject despawnPoint;
    public GameObject target;


    void Start()
    {
        hasCompleted = false;

        //Use the Inspector, as there may be multiple spawn points in a map.
        //spawnPoint = GameObject.Find("SpawnPoint");
    }


    void OnTriggerEnter(Collider other)
    {
        if (hasCompleted == false && other.tag == "Player")
        {
            StartSpawning();
        }
        else
        {
            print("Cannot spawn anymore.");
        }
    }


    void StartSpawning()
    {
        Quaternion rotation = Quaternion.identity;
        Instantiate(target, spawnPoint.transform.position, rotation);
        //hasCompleted = true;
    }
}