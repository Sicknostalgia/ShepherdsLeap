using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public Transform firePoint;
    public float hookSpeed = 20f;
    public float pullForce = 50f;
    public float maxDistance = 10f;
    public LayerMask grappleLayer;
    public float drag = 2f; // Controls how quickly the movement stops

    private Rigidbody rb;
    private Vector3 hookTarget;
    private bool isHooking = false;
    private Vector3 pullDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = drag; // Adds some resistance to movement
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FireHook();
        }
    }

    void FireHook()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxDistance, grappleLayer))
        {
            hookTarget = hit.point;
            pullDirection = (hookTarget - transform.position).normalized;
            isHooking = true;
        }
    }

    void FixedUpdate()
    {
        if (isHooking)
        {
            rb.AddForce(pullDirection * pullForce, ForceMode.Acceleration);

            // Optional: Stop when close enough
            if (Vector3.Distance(transform.position, hookTarget) < 0.5f)
            {
                isHooking = false;
            }
        }
    }
}
