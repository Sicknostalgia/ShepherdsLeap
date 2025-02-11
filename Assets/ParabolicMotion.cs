using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion : MonoBehaviour
{
    /*  Rigidbody rb;
     public Vector3 launchDirection = new Vector3(1, 1, 0);
     public float launchForce = 10f;
     // Start is called before the first frame update
     void Start()
     {

        if(!TryGetComponent<Rigidbody>(out rb))
         {
             Debug.Log("No rb");
         }


     }

     // Update is called once per frame
     void Update()
     {
         if(Input.GetKey(KeyCode.Alpha0))
         rb.AddForce(launchDirection.normalized * launchForce, ForceMode.Impulse);
     }*/

    /* public Transform pendulumTip; // The bottom tip of the pendulum
     public float swingRadius = 2f; // Radius of pivot swing
     public float swingSpeed = 1f; // Speed of top movement
     public float pendulumLength = 3f; // Distance from pivot to tip
     public float gravity = 9.81f;

     private float time;

     void Update()
     {
         time += Time.deltaTime * swingSpeed;
         if (Input.GetKey(KeyCode.Backspace))
         {
             time = Mathf.Clamp(time, 0, Mathf.PI);
             // Move the top pivot in a circular motion
             Vector3 pivotPosition = new Vector3(swingRadius * Mathf.Cos(time), swingRadius * Mathf.Sin(time), 0);
             transform.forward = pivotPosition; // Move the top pivot

             // Calculate bottom tip position
             Vector3 tipOffset = new Vector3(0, -pendulumLength, 0);
             pendulumTip.position = transform.position + tipOffset;

             // Debug: Draw lines
             //  Debug.DrawLine(transform.position, pendulumTip.position, Color.red);
         }

     }
     private void OnDrawGizmos()
     {
         Gizmos.color = Color.blue;
         pendulumTip.TryGetComponent<SphereCollider>(out SphereCollider penCol);
         Gizmos.DrawWireSphere(pendulumTip.position,penCol.radius);

         Gizmos.color = Color.yellow;
         Gizmos.DrawLine(transform.position, pendulumTip.position);
     }*/

    public Rigidbody playerRb;
    public Rigidbody poleRb;
    public float swingForce = 10f;
    public float detachForce = 5f;
    public bool isSwinging = true;
    public PlayerMvmnt playerMvmnt;
    Vector3 input;
    public float time;
    float initialTime = 1f;
    private void Update()
    {
        input = new Vector3(playerMvmnt.horizontalInput, 0, playerMvmnt.verticalInput).normalized;

        if (isSwinging)
        {
            time += Time.deltaTime; // Increment timer only while swinging

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                // Apply force relative to the player's movement input
                playerRb.AddForce((transform.up + input) * detachForce, ForceMode.Impulse);

                DetachFromPole();
            }
        }
    }

    void DetachFromPole()
    {
        if (time >= initialTime)
        {
            poleRb.gameObject.SetActive(false);
        }

        isSwinging = false; // Ensure we stop swinging
        time = initialTime; // Reset the timer after detaching
    }
}
