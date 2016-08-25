using UnityEngine;

public class LureBehaviors : MonoBehaviour {

    [HideInInspector] public PlayerComponents playerComponents;
    [HideInInspector] public ThrowLure throwLure;

    public Rigidbody rb;
    public GameObject self;

    private Collider selfCol;
    private Collider playerCol;
    private bool lureActive;

    [HideInInspector] public float despawnTime = 15f;

    void Start()
    {
        playerComponents = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerComponents>();
        throwLure = GameObject.FindGameObjectWithTag("Player").GetComponent<ThrowLure>();
        lureActive = true;
        rb = GetComponent<Rigidbody>();

        selfCol = GetComponent<Collider>();
        playerCol = GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>();
    }

    void Update()
    {
        if (lureActive == true)
        {
            //With these numbers, and Mass = 1 and Drag = 2, the lure will land almost EXACTLY where the cursor points. :D
            rb.AddForce((throwLure.calculateBestThrowSpeed(playerComponents.throwPoint.transform.position, throwLure.adjustedHit, 1f) * 100f), ForceMode.Force);
            Physics.IgnoreCollision(selfCol, playerCol);
            lureActive = false;
        }

        despawnTime -= Time.deltaTime;

        if (despawnTime <= 0)
        {
            Destroy(self);
        }

    }

}
