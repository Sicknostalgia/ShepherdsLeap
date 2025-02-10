using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmnt : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    float horizontalInput;
    float verticalInput;

    public float jumpForce;
    public float jumpCD;
    public float airMultiplier;
    bool isReady2jump;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Drag & speed control")]
    public float playerHeight;
    public float grounddrag;
    public LayerMask ground;
    public bool isGrounded;
    Vector3 moveDiretion;
    public Transform orientation;

    Rigidbody rb;
    public MovementState state;
    public enum MovementState
    {
        restricted,
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }
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

        if (Input.GetKey(jumpKey) && isGrounded)
        {
            isReady2jump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCD); //able to continously jump if jumpkey is hold
        }
    }
    void MovePlayer()
    {
        state = MovementState.restricted;
        moveDiretion = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (isGrounded)
        {
            rb.AddForce(moveDiretion.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDiretion.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
        }

    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .5f, ground); //#2

        MyInput();
        SpeedControl();
        if (isGrounded)
        {
            rb.drag = grounddrag; //apply drag
        }
        else
            rb.drag = 0;
    }
    private void FixedUpdate()
    {
        /* if(state != MovementState.restricted)
         {*/
        MovePlayer();
        //  }
    }
    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //make sure to reset Y velocity.

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //applying the force once
    }
    private void ResetJump()
    {
        isReady2jump = true;
    }
}
