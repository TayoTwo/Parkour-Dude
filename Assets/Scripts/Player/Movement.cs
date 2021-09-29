using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    InputManager im;

    //Movement stats
    public float movSpeed;
    public float verAirSpeed;
    public float horAirSpeed;
    public float grav;
    public float currentSpeed;
    public float jumpForce;
    public float jumpRatio = 1.85f;
    public float wallJumpForce;
    public float maxSpeed;
    public float drag;

    //Markers used to define where to detect a wall from
    public BoxTrigger left;
    public BoxTrigger right;
    public BoxTrigger ground;

    public bool isGrounded;
    bool wasGrounded;
    public bool isJumping;

    public bool isGroundedWallL;
    public bool isGroundedWallR;
    bool wasWallGrounded;
    bool isWallJumping;

    Rigidbody rb;
    SphereCollider playerCol;
    Camera cam;

    int layerMask = 1 << 8;

    AudioSource jumpClip;
    
    // Assign our mouse sensitivity, walljump hitsphere locations and cursor visibilty
    void Awake(){

        im = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        cam = GetComponentInChildren<Camera>();
        layerMask = ~layerMask;
        rb = GetComponent<Rigidbody>();
        jumpClip = GetComponent<AudioSource>();
        playerCol = GetComponent<SphereCollider>();

    }

    //Update all physics related events in FixedUpdate
    void FixedUpdate(){

        Gravity();
        Move();
        
    }

    //Assign custom gravity to the player
    void Gravity(){

        rb.AddForce(Vector3.down * grav,ForceMode.Acceleration);

    }

    void Move(){

            // Check if we are currently touching any walls or the ground
            GroundCheck();
            WallCheck();

            //Recieve input from the player
            Vector3 input = im.input;
            Vector3 desiredMove = new Vector3();

            if (isGrounded){
                
                //If grounded accelerate the player left-right,forward-back depending on where they are facing
                isJumping = false;
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * movSpeed;
                desiredMove.z = desiredMove.z * movSpeed;
                
                rb.AddForce(desiredMove,ForceMode.Acceleration);

                if (!isJumping && input.z > 0){

                    //If jump is pressed and we aren't currently jumping then jump
                    rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                    isJumping = true;

                    if(!jumpClip.isPlaying) {

                        jumpClip.Play();

                    }

                }

                //If we're above the maxSpeed start applying a drag force
                if(currentSpeed > maxSpeed){

                    rb.AddForce(new Vector3(-rb.velocity.normalized.x * drag,0,-rb.velocity.normalized.z * drag),ForceMode.Acceleration);

                }


            } else {

                //Wall Jump
                if((isGroundedWallL || isGroundedWallR)){

                    rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);

                    //If jump is pressed and we aren't currently wall jumping then perform a wall jump
                    if(!isWallJumping){

                        //If left or right is pressed while jumping then perform the jump diagonally in the direction
                        if(input.z > 0 && (input.x > 0 || input.y > 0)){

                            //rb.velocity = Vector3.zero;
                            rb.velocity = new Vector3(rb.velocity.x * 0.25f,0,rb.velocity.z* 0.25f);
                            rb.AddRelativeForce(new Vector3(wallJumpForce * input.x, wallJumpForce * jumpRatio, wallJumpForce), ForceMode.Impulse);
                            isWallJumping = true;

                            if(!jumpClip.isPlaying) {

                                jumpClip.Play();

                            }

                        } else if(input.z > 0 && (input.x == 0 && input.y == 0)){

                            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
                            rb.AddRelativeForce(new Vector3(0, wallJumpForce * jumpRatio, 0), ForceMode.Impulse);
                            isWallJumping = true;

                            if(!jumpClip.isPlaying) {

                                jumpClip.Play();

                            }

                    }

                    } 

                }

                //If not grounded then move the player using airSpeed instead of movSpeed
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * horAirSpeed;
                desiredMove.z = desiredMove.z * verAirSpeed;
                
                rb.AddForce(desiredMove, ForceMode.Acceleration);

            } 

            

            //Base our speed on the x and z axis only
            currentSpeed = new Vector3(rb.velocity.x,0,rb.velocity.z).magnitude;

            
    }   
    void GroundCheck(){

        wasGrounded = isGrounded;

        //Use hitboxes I beg
        if(ground.isTrue == true){

            isGrounded = true;

        } else if(ground.isTrue == false){

            isGrounded = false;

        }
        
        //if they weren't touching the ground before but now they are give the player the ability to jump
        if(!wasGrounded && isGrounded){

            isJumping = false;

        }

    }

    void WallCheck(){

        //Bruh just use hitboxes    
        if(left.isTrue == true){

            isGroundedWallL = true;

        }

        if(right.isTrue == true){

            isGroundedWallR = true;

        }

        // if(!wasWallGrounded && (isGroundedWallL || isGroundedWallR)){

        //     isJumping = false;

        // }
        

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
