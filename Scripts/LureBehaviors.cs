using UnityEngine;

public class LureBehaviors : MonoBehaviour {

    private GameObject player;
    public GameObject thing;
    public Rigidbody rb;
    public ThrowLure throwLure;

    private bool lureActive;

    [HideInInspector] public float despawnTime = 15f;

    void Start()
    {
        lureActive = true;
        rb = GetComponent<Rigidbody>();
        throwLure = FindObjectOfType(typeof(ThrowLure)) as ThrowLure;
    }

    void Update()
    {
        if (lureActive == true)
        {
            //With these numbers, and Mass = 1 and Drag = 2, the lure will land almost EXACTLY where the cursor points. :D
            rb.AddForce((throwLure.calculateBestThrowSpeed(throwLure.throwPoint.transform.position, throwLure.adjustedHit, 1f) * 100f), ForceMode.Force);
            lureActive = false;
        }

        despawnTime -= Time.deltaTime;

        if (despawnTime <= 0)
        {
            Destroy(thing);
        }

    }

}
