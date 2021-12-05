using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    [Header("Components")]
    public InputManager im;
    public WallMovement wallMovement;
    public BoxTrigger ground;
    public Rigidbody rb;
    public AudioSource jumpClip;
    [Header("Movement Stats")]
    public float movAcc;
    public float airAcc;
    public float currentSpeed;
    public float maxMoveSpeed;
    public float decelForce;
    [Header("Jump Stats")]
    public float jumpForce;
    public float jumpRatio = 1.85f;
    public float grav = 9.81f;

    //Varibles that dictate if the player can jump
    bool isGrounded;
    bool wasGrounded;
    string objTag;
    bool isJumping;
    bool onPlatform;
    Camera cam;

    // Assign our mouse sensitivity, walljump hitsphere locations and cursor visibilty
    void Awake(){

        //Assign the components we'll be using to variables
        im = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        jumpClip = GetComponent<AudioSource>();
        wallMovement = GetComponent<WallMovement>();

    }

    //Update all physics related events in FixedUpdate
    void FixedUpdate(){

        GroundCheck();
        Move();
        Drag();
        Gravity();
        
    }

    //Because the gravity of the player will change depending on whether we are touching a wall we run a gravity function ourselves
    void Gravity(){

        rb.AddForce(Vector3.down * grav,ForceMode.Acceleration);

    }

    void Drag(){

        if(im.inputs.x == 0 && im.inputs.y == 0 && objTag != "MovingP"){

            Vector3 dragDir = new Vector3(-rb.velocity.normalized.x * decelForce,
                                        0,
                                        -rb.velocity.normalized.z * decelForce);
            rb.AddForce(dragDir);

        }

    }

    //Take the player's inputs and move the player accordingly 
    void Move(){

            //Recieve input from the player
            Vector3 inputs = im.inputs;
            Vector3 desiredMove = new Vector3();

            if(!isGrounded){

                objTag = "";
                //If the player is touching walld decrease the gravity
                
                //If not grounded then move the player using airSpeed instead of movSpeed
                desiredMove = cam.transform.forward*inputs.y + cam.transform.right*inputs.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * airAcc;
                desiredMove.z = desiredMove.z * airAcc;
                
                rb.AddForce(desiredMove, ForceMode.Acceleration);

            } else if (isGrounded){

                //If grounded accelerate the player left-right,forward-back depending on where they are facing
                isJumping = false;
                desiredMove = cam.transform.forward*inputs.y + cam.transform.right*inputs.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * movAcc;
                desiredMove.z = desiredMove.z * movAcc;
                
                rb.AddForce(desiredMove,ForceMode.Acceleration);

                //If the jump button is pressed and we are not touching a wall jump normally
                if (!isJumping && inputs.z > 0 && (!wallMovement.isTouchingLeft || !wallMovement.isTouchingRight) ){

                    //If jump is pressed and we aren't currently jumping then jump
                    rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                    isJumping = true;

                    if(!jumpClip.isPlaying) {

                        jumpClip.Play();

                    }

                }

            }

            currentSpeed = (new Vector3(rb.velocity.x,0,rb.velocity.z)).magnitude;

            //If the player is above the max speed cap them
            if(currentSpeed > maxMoveSpeed){

                rb.velocity = new Vector3(rb.velocity.normalized.x * maxMoveSpeed ,rb.velocity.y ,rb.velocity.normalized.z * maxMoveSpeed);

            }

            
    }   

    void GroundCheck(){

        wasGrounded = isGrounded;
        isGrounded = ground.isTrue;
        
        //if they weren't touching the ground before but now they are give the player the ability to jump
        if(!wasGrounded &&  isGrounded){

            isJumping = false;

        }

    }

    public void UpdateObjTag(string tag){
        Debug.Log(objTag);
        objTag = tag;

    }

    // void OnDrawGizmos() {

    //     //Draw our ground detection hitsphere
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - playerCol.radius, transform.position.z), playerCol.radius/4f);

    //     //Draw our wall detection hitspheres
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
    //     Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
    //     Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
    //     Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
        
    // }

}
