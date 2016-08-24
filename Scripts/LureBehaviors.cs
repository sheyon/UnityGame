using UnityEngine;

public class LureBehaviors : MonoBehaviour {

    [HideInInspector] public PlayerComponents playerComponents;
    [HideInInspector] public ThrowLure throwLure;

    public Rigidbody rb;
    public GameObject thing;

    private bool lureActive;

    [HideInInspector] public float despawnTime = 15f;

    void Start()
    {
        playerComponents = FindObjectOfType(typeof(PlayerComponents)) as PlayerComponents;
        throwLure = FindObjectOfType(typeof(ThrowLure)) as ThrowLure;
        lureActive = true;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (lureActive == true)
        {
            //With these numbers, and Mass = 1 and Drag = 2, the lure will land almost EXACTLY where the cursor points. :D
            rb.AddForce((throwLure.calculateBestThrowSpeed(playerComponents.throwPoint.transform.position, throwLure.adjustedHit, 1f) * 100f), ForceMode.Force);
            lureActive = false;
        }

        despawnTime -= Time.deltaTime;

        if (despawnTime <= 0)
        {
            Destroy(thing);
        }

    }

}
