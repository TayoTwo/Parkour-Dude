using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    InputManager im;

    //Movement stats
    public float movAcc;
    public float airAcc;
    public float grav = 9.81f;
    public float currentSpeed;
    public float jumpForce;
    public float jumpRatio = 1.85f;
    public float wallJumpForce;
    public float maxMoveSpeed;

    //Markers used to define where to detect a wall from
    public BoxTrigger left;
    public BoxTrigger right;
    public BoxTrigger ground;

    public bool isGrounded;
    public bool isJumping;

    bool wasGrounded;

    public bool isTouchingLeft;
    public bool isTouchingRight;
    public bool isWallJumping;

    bool wasTouchingLeft;
    bool wasTouchingRight;
    float wallJumpDir = 0;

    Rigidbody rb;
    SphereCollider playerCol;
    Camera cam;

    int layerMask = 1 << 8;

    AudioSource jumpClip;
    
    // Assign our mouse sensitivity, walljump hitsphere locations and cursor visibilty
    void Awake(){

        //Assign the components we'll be using to variables
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

    //Because the gravity of the player will change depending on whether we are touching a wall we run a gravity function ourselves
    void Gravity(){

        rb.AddForce(Vector3.down * grav,ForceMode.Acceleration);

    }

    //Take the player's inputs and move the player accordingly 
    void Move(){

            // Check if we are currently touching any walls or the ground
            GroundCheck();
            WallCheck();

            //Recieve input from the player
            Vector3 input = im.input;
            Vector3 desiredMove = new Vector3();

            
            if(!isGrounded){

                //If the player is touching walld decrease the gravity
                if((isTouchingLeft || isTouchingRight)){

                    grav = 0.1f;

                    if(isTouchingLeft){

                        wallJumpDir = 1;

                    } else if(isTouchingRight){

                        wallJumpDir = -1;

                    }

                    //If jump is pressed and we aren't currently wall jumping then perform a wall jump
                    if(input.z > 0 && !isWallJumping){

                        rb.AddRelativeForce(new Vector3(wallJumpForce * wallJumpDir, wallJumpForce * jumpRatio, 0), ForceMode.Impulse);

                        isWallJumping = true;

                        if(!jumpClip.isPlaying) {

                            jumpClip.Play();

                        }

                    }

                } else {

                    wallJumpDir = 0;
                    grav = 9.81f;

                }

                //If not grounded then move the player using airSpeed instead of movSpeed
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * airAcc;
                desiredMove.z = desiredMove.z * airAcc;
                
                rb.AddForce(desiredMove, ForceMode.Acceleration);

            } else if (isGrounded){

                RaycastHit hit;
                Rigidbody plat;

                if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down), out hit, playerCol.radius + 0.1f, layerMask)){

                    plat = hit.collider.GetComponent<Rigidbody>();

                    if(plat.velocity != Vector3.zero){

                        // Debug.Log(plat.velocity - rb.velocity);

                        // rb.AddForce(plat.velocity - rb.velocity, ForceMode.VelocityChange);

                    }


                }
                
                //If grounded accelerate the player left-right,forward-back depending on where they are facing
                isJumping = false;
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove.Normalize();

                desiredMove.x = desiredMove.x * movAcc;
                desiredMove.z = desiredMove.z * movAcc;
                
                rb.AddForce(plat.velocity - rb.velocity, ForceMode.VelocityChange);
                rb.AddForce(desiredMove,ForceMode.Acceleration);

                //If the jump button is pressed and we are not touching a wall jump normally
                if (!isJumping && input.z > 0 && (!isTouchingLeft || !isTouchingRight) ){

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

    void PlatDrag(){

        if(isGrounded){

            RaycastHit hit;
            Rigidbody plat;

            if(Physics.Raycast(transform.position,transform.TransformDirection(Vector3.down), out hit, playerCol.radius + 0.1f, layerMask)){

                plat = hit.collider.GetComponent<Rigidbody>();

                if(plat.velocity != Vector3.zero){

                    // Debug.Log(plat.velocity - rb.velocity);

                    // rb.AddForce(plat.velocity - rb.velocity, ForceMode.VelocityChange);

                }

                rb.velocity = 

            }

        }

    }

    void GroundCheck(){

        wasGrounded = isGrounded;

        isGrounded = ground.isTrue;
        
        //if they weren't touching the ground before but now they are give the player the ability to jump
        if(!wasGrounded && isGrounded){

            isJumping = false;

        }

    }

    void WallCheck(){

        wasTouchingLeft = isTouchingLeft;
        wasTouchingRight = isTouchingRight;

        //Bruh just use hitboxes    
        isTouchingLeft = left.isTrue;
        isTouchingRight = right.isTrue;

        if((!wasTouchingLeft && isTouchingLeft) || 
            (!wasTouchingRight && isTouchingRight)){

            isWallJumping = false;

        }

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
