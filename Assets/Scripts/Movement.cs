using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    //Animations
    Animator animations;
    private int isWalkingHash;
    private int isRunningHash;
    private int isStraffingLeftHash;
    private int isStraffingRightHash;
    
    //speed
    public float walkSpeed;
    public float runSpeed;
    //public float turnSpeed;
    
    //inputs
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public string inputRun;

    private Vector2 movement;            
    public Vector3 jumpSpeed;
    
    private CapsuleCollider playerCollider;
    
    
    void Start()
    {
        animations = gameObject.GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isStraffingRightHash = Animator.StringToHash("isStraffingRight");
        isStraffingLeftHash =  Animator.StringToHash("isStraffingLeft");  
    }

    void Update()
    {
        bool isWalking = animations.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey(inputFront);
        bool runPressed = Input.GetKey(inputRun);
        bool straffRightPressed = Input.GetKey(inputRight);
        bool straffLeftPressed = Input.GetKey(inputLeft);
        
        
        //animator trigger walk animation if the key is pressed
        if (!isWalking && forwardPressed)
        {
            animations.SetBool(isWalkingHash, true);
        }
        
        //animator goes back to idle
        if (isWalking && !forwardPressed)
        {
            animations.SetBool(isWalkingHash, false);
        }
        
        // check if the player is pressing forward and run inputs
        if (forwardPressed && runPressed)
        {
            animations.SetBool(isRunningHash, true);
        }
        
        // if player stop pressing forward or run the running animation stops
        if (!runPressed || !forwardPressed)
        {
            animations.SetBool(isRunningHash, false );
        }
        
        if (straffRightPressed)
        {
            animations.SetBool(isStraffingRightHash, true);
        }
        else
        {
            animations.SetBool(isStraffingRightHash, false);

        }
        if (straffLeftPressed)
        {
            animations.SetBool(isStraffingLeftHash, true);
        }
        else
        {
            animations.SetBool(isStraffingLeftHash, false);
        }
        
        
        
        
        if (Input.GetKey(inputBack))
        {
            transform.Translate(0,0,-(walkSpeed/2) * Time.deltaTime);
        }
        
        
    }
}
