using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolicMotion : MonoBehaviour
{
     Rigidbody rb;
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
    }
}
