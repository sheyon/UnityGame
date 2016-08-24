using UnityEngine;

public class RailMover : MonoBehaviour {

    public GameObject mover;
    public float speed = 2.5f;
    public Transform[] waypoint;
    public bool loopWaypoints;
    public bool continueWithoutPlayer;

    public bool lockMovement;
    public ControlledBySwitch controlledBySwitch;

    private GameObject player;
    private bool playerIsRiding;

    private bool readyToStart;
    private bool rideIsFinished = false;
    private int i = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (controlledBySwitch == null)
        {
            readyToStart = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        other.transform.SetParent(mover.transform);
        if (other.gameObject == player.gameObject)
        {
            playerIsRiding = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
        if (other.gameObject == player.gameObject)
        {
            playerIsRiding = false;
        }
    }

    void Travelling()
    {
        if (rideIsFinished == false)
        {
            if (loopWaypoints == true && i >= waypoint.Length) { i = 0; }
            mover.transform.position = Vector3.MoveTowards(mover.transform.position, waypoint[i].transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(mover.transform.position, waypoint[i].transform.position) < .1f)
            {
                i++;
                if (loopWaypoints == false && i >= waypoint.Length)
                {
                    rideIsFinished = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (readyToStart == true)
        {
            if (playerIsRiding == true || continueWithoutPlayer == true)
            {
                Travelling();
            }
        }
        if (loopWaypoints == true)
        {
            rideIsFinished = false;
        }
    }
}