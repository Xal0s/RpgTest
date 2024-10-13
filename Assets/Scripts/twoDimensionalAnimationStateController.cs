using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class twoDomensionalAnimationStateController : MonoBehaviour
{
    private Animator animator;

    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    public float maximumWalkVelocity = 0.5f;
    public float maximumRunVelocity = 2.0f;
    public float moveSpeed = 2.0f; 


    public string moveForward;
    public string strafeLeft;
    public string strafeRight;
    public string moveBackwards;
    public string sprint;

    private int VelocityXHash;
    private int VelocityZHash;

    private Rigidbody rb;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        VelocityXHash = Animator.StringToHash("Velocity X");
        VelocityZHash = Animator.StringToHash("Velocity Z");
        animator.applyRootMotion = false;
    }
    void changeVelocity(bool forwardPressed,  bool leftPressed, bool rightPressed, bool sprintPressed, float currentMaxVelocity)
        {
            //acceleration of the player when he walks forward
            if (forwardPressed && velocityZ < currentMaxVelocity)
            {
                velocityZ += Time.deltaTime * acceleration;
            }
            // acceleration of the player when he strafe left
            if (leftPressed && velocityX > -currentMaxVelocity)
            {
                velocityX -= Time.deltaTime * acceleration;
            }
            // acceleration of the player when he strafe right
            if (rightPressed  && velocityX < currentMaxVelocity)
            {
                velocityX += Time.deltaTime * acceleration;
            }
            // when player release the key to walk forward the character is decelerating
            if (!forwardPressed && velocityZ > 0.0f)
            {
                velocityZ -= Time.deltaTime * deceleration;
            } 
            // when player release the key to strafe left the character is decelerating
            if (!leftPressed && velocityX < 0.0f)
            {
                velocityX += Time.deltaTime * deceleration;
            }
            // when player release the key to strafe right the character is decelerating
            if (!rightPressed && velocityX > 0.0f)
            {
                velocityX -= Time.deltaTime * deceleration;
            }
        }

    void lockOrResetVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool sprintPressed, float currentMaxVelocity)
        {
             // lock the minimum of the velocity on the Z axis
        if (!forwardPressed && velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }
        // lock the minimum of velocity when the player is not strafing
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        //lock the velocity at the maximum when the player is running
        if (forwardPressed && sprintPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //if the player release the sprint key the character will decelerate
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            //bring the velocity at max velocity whatever if he is running or walking
            if (velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }
        //lcoking left velocity
        if (leftPressed && sprintPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //decelerate the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            // round the velocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        //round the currentMaxVelocity if withing the offset
        else if (leftPressed && velocityX > - currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
        //locking right velocity
        if (rightPressed && sprintPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        // decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            //round the currentMaxVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        // round the currentMaxVelocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
        }
        

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(moveForward);
        bool leftPressed = Input.GetKey(strafeLeft);
        bool rightPressed = Input.GetKey(strafeRight);
        bool backwardPressed = Input.GetKey(moveBackwards);
        bool sprintPressed = Input.GetKey(sprint);
        
        float currentMaxVelocity = sprintPressed ? maximumRunVelocity : maximumWalkVelocity;
        
        changeVelocity(forwardPressed,  leftPressed, rightPressed, sprintPressed, currentMaxVelocity);
        lockOrResetVelocity(forwardPressed, leftPressed, rightPressed, sprintPressed, currentMaxVelocity);
        Debug.Log("Position: " + rb.position);
        animator.SetFloat(VelocityZHash, velocityZ);
        animator.SetFloat(VelocityXHash, velocityX);
        
        Vector3 movement = new Vector3(-velocityX, 0, -velocityZ) * moveSpeed;
        rb.MovePosition(rb.position + movement * Time.deltaTime);

    }
}
