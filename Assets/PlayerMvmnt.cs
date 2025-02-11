using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMvmnt : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float horizontalInput;
    public float verticalInput;

    public float jumpForce;
    public float jumpCD;
    public float airMultiplier;
    bool isReady2jump;
    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeRayHit;
    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

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

        //on slope
        if (OnSlope())
        {
            rb.AddForce(GetSlope() * moveSpeed * 20f, ForceMode.Force);
        }
        if (isGrounded)
        {
            rb.AddForce(moveDiretion.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!isGrounded)
        {
            rb.AddForce(moveDiretion.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); // add force downwards the slope to prevent bumpy effect
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .5f, ground); //#2

        MyInput();
        SpeedControl();
        StateHandler();
        if (isGrounded)
        {
            rb.drag = grounddrag; //apply drag
        }
        else
            rb.drag = 0;
    }
    void SpeedControl()
    {
        //limiting speed on slope
        if (OnSlope())
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //limiting speed on ground and air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitVel.x, rb.velocity.y, limitVel.z);
            }
        }
        

    }
    void StateHandler()
    {
        if(isGrounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if (isGrounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;

        }
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
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeRayHit, playerHeight * .5f + .3f)){
            float angle = Vector3.Angle(Vector3.up, slopeRayHit.normal);
            return angle< maxSlopeAngle && angle != 0; //return value if angle is less than the maxSlopeAngle
        }
        return false;
    }

    private Vector3 GetSlope()
    {
        return Vector3.ProjectOnPlane(moveDiretion, slopeRayHit.normal.normalized); //since its a  direction you should make a habit to always normalized it.
    }
}
