using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepellTo : MonoBehaviour
{
    public Transform player;
    public float sheepSpeed;
    public float detectionRadius = 2f;
    public LayerMask detectionLayer;
    public float rotSpeed;
    private Rigidbody rb;
    private void Start()
    {
        if(!TryGetComponent<Rigidbody>(out rb))
        {
            Debug.LogWarning("rigidbody not found on " + gameObject.name);
        }
        else
        {
            rb.freezeRotation = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if(player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.position);    
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (player != null) //not empty as reference
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);
            if (hitColliders != null) //player not null as target
            {
                //Debug.Log(hitColliders[0].name);
                Vector3 dir = (transform.position - player.position).normalized; // points away from the player 
                /*   transform.position += dir * sheepSpeed * Time.deltaTime;*/

                rb.velocity = dir * sheepSpeed;
                //rotate to face away from the player
                //replace Slerp
                //transform.rotation = Quaternion.LookRotation(dir);
                transform.forward = Vector3.Slerp(transform.forward, dir.normalized, Time.deltaTime * rotSpeed);
              /*  Quaternion targRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targRot, Time.deltaTime * rotSpeed);*/
            }
            else
            {
                return;
            }
        }
    }
}
