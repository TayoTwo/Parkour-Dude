using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour{


    public Movement movement;
    public BoxTrigger left;
    public BoxTrigger right;

    public bool isTouchingLeft;
    public bool isTouchingRight;
    public bool isWallJumping;

    bool wasTouchingLeft;
    bool wasTouchingRight;

    public float wallJumpForce;
    float wallJumpDir = 0;

    Rigidbody plat;
    Rigidbody rb;
    SphereCollider playerCol;
    int layerMask = 1 << 8;
    void Awake() {

        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody>();
        layerMask = ~layerMask;
        playerCol = GetComponent<SphereCollider>();
        
    }

    void FixedUpdate() {

        WallCheck();
        WallMove();
        
    }

    void WallMove(){

            if((isTouchingLeft || isTouchingRight)){

                //movement.grav = 0.1f;

                if(isTouchingLeft){

                    wallJumpDir = 1;

                } else if(isTouchingRight){

                    wallJumpDir = -1;

                }

                    //If jump is pressed and we aren't currently wall jumping then perform a wall jump
                if(movement.im.inputs.z > 0 && !isWallJumping){

                    rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
                    rb.AddRelativeForce(new Vector3(wallJumpForce * wallJumpDir, wallJumpForce * movement.jumpRatio, 0), ForceMode.Impulse);

                    isWallJumping = true;

                    if(!movement.jumpClip.isPlaying) {

                        movement.jumpClip.Play();

                    }

                }

            } else {

                wallJumpDir = 0;
                movement.grav = 9.81f;

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
}
