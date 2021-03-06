﻿using UnityEngine;


public class ThrowLure : MonoBehaviour {

    [HideInInspector] public PlayerComponents playerComponents;
    public LureBehaviors lureBehaviors;
    public GameObject lure;
    [HideInInspector] public Vector3 adjustedHit;

    public float maxDistance = 10f;
    private RaycastHit hit;
    private float cooldownTime = 0f;
    //private float distance;

    void Awake()
    {
        playerComponents = GetComponent<PlayerComponents>();
    }

    public Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors

        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y

        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t

        float t = timeToTarget;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds

        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }


    void Update()
    {
        cooldownTime -= Time.deltaTime;

        if (Input.GetMouseButtonDown(1))
        {
            if (cooldownTime > 0)
            {
                print("You cannot throw another lure yet!");
            }

            else if (cooldownTime <= 0)
            {
                Physics.Raycast(playerComponents.crosshair.transform.position, playerComponents.crosshair.transform.forward, out hit, maxDistance);

                if (hit.collider == null)
                {
                    print("You did not hit anything!");
                }
                else
                {
                    //distance = hit.distance;
                    Quaternion rotation = Quaternion.identity;

                    if (hit.distance <= maxDistance)
                    {
                        adjustedHit = new Vector3(hit.point.x, (hit.point.y + 0.5f), hit.point.z);
                        Instantiate(lure, playerComponents.throwPoint.transform.position, rotation);
                        cooldownTime = 3f;
                    }
                    else
                    {
                        //Do nothing.
                    }
                }
            }
        }
    }
}
