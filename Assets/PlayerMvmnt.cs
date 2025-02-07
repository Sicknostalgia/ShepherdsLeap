using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmnt : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float horizontalInput;
    float verticalInput;
    [Header("Drag & speed control")]
    public float playerHeight;
    public float grounddrag;
    public LayerMask ground;
    public bool isGrounded;
    Vector3 moveDiretion;
    public Transform orientation;

    Rigidbody rb;
    // Start is called before the first frame update
    void OnDrawGizmos()
    {
        // Calculate the ray distance
        float rayDistance = playerHeight * 0.5f + 0.2f;

        // Set the Gizmo color (you can change the color as you like)
        Gizmos.color = Color.green;

        // Draw the ray from the player position downwards
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayDistance);

        // Optionally, you can draw a sphere at the end of the ray to show where it hits
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.down * rayDistance, 0.1f);  // Adjust size of the sphere as needed
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
    void MovePlayer()
    {
        moveDiretion = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDiretion.normalized * moveSpeed * 10f, ForceMode.Acceleration);
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, ground); //#2

        MyInput();
        if (isGrounded)
        {
            rb.drag = grounddrag; //apply drag
        }
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
}
