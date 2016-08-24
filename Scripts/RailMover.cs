using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class RailMover : MonoBehaviour {

    [HideInInspector] public FirstPersonController firstPersonController;
    public ControlledBySwitch controlledBySwitch;

    public GameObject mover;
    public float moverVertOffset = 1f;
    public float speed = 2.5f;
    public Transform[] waypoint;
    public bool loopWaypoints;
    public bool continueWithoutPlayer;

    public bool lockMovement;
    private Vector3 moverOffsetPos;

    private GameObject player;
    private bool playerIsRiding;

    private bool readyToStart;
    private bool rideIsFinished = false;
    private int i = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        firstPersonController = FindObjectOfType<FirstPersonController>();

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
            if (lockMovement == true)
            {
                moverOffsetPos = new Vector3(mover.transform.position.x, (mover.transform.position.y + moverVertOffset), mover.transform.position.z);
                firstPersonController.canFreeMove = false;
                player.transform.position = Vector3.MoveTowards(player.transform.position, moverOffsetPos, Time.deltaTime * speed);
            }

            if (loopWaypoints == true && i >= waypoint.Length) { i = 0; }
            mover.transform.position = Vector3.MoveTowards(mover.transform.position, waypoint[i].transform.position, Time.deltaTime * speed);

            if (Vector3.Distance(mover.transform.position, waypoint[i].transform.position) < .1f)
            {
                i++;
                if (loopWaypoints == false && i >= waypoint.Length)
                {
                    rideIsFinished = true;
                    firstPersonController.canFreeMove = true;
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
        if (loopWaypoints == true && rideIsFinished == true)
        {
            rideIsFinished = false;
        }
        if (loopWaypoints == true && lockMovement == true)
        {
            lockMovement = false;
        }
    }
}