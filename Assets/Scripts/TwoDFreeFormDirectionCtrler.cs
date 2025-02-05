using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDFreeFormDirectionCtrler : MonoBehaviour
{
    Animator animator;

    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    int VelocityHash;
    float maximumWalkVel = .5f;//value in blend tree
    float maximumRunVel = 2.0f;  //value in blend tree
    void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");
    }
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwrdPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);


        //set current maxVel
        float curMaxVel = runPressed ? maximumRunVel : maximumWalkVel; // if is pressed the lshift then set curMaxvel to maxRunvel otherwise maxWalkVel
        if (forwardPressed && velocityZ < curMaxVel)//set minimum value of 0.5
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (backwrdPressed && velocityZ > -curMaxVel)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (leftPressed && velocityX > -curMaxVel)//set minimum value of -0.5
        {
            velocityX -= Time.deltaTime * acceleration;  //operator -= since opposite side
        }
        if (rightPressed && velocityX < curMaxVel)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        //decreased velocityZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        if (!backwrdPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        //decrease velocityx if right is not pressed and velocityx  > 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        //reset if both right ad left is not pressed
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        animator.SetFloat("VelocityZ", velocityZ);
        animator.SetFloat("VelocityX", velocityX);
    }
}
