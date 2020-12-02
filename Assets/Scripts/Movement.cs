using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public GameObject UI;

    //Camera and rotation settings
    public Camera cam;
    public float sens;
    float xRot;
    float yRot;

    //Variables used for the wall confirm animation
    public float wallRotAngle;
    public float currentAngle;
    public float targetAngle;
    public float animDuration;

    //Movement stats
    public float movSpeed;
    public float airSpeed;
    public float grav;
    public float currentSpeed;
    public float jumpForce;
    public float wallJumpForce;
    public float maxSpeed;
    public float drag;

    //Markers used to define where to detect a wall from
    public Transform left;
    public Transform right;

    public bool isGrounded;
    public bool wasGrounded;

    public bool isGroundedWallL;
    public bool wasGroundedWallL;

    public bool isGroundedWallR;
    public bool wasGroundedWallR;

    public bool isJumping;

    public bool isWallJumping;

    Rigidbody rb;
    SphereCollider playerCol;
    bool toggle;

    int layerMask = 1 << 8;

    AudioSource jumpClip;
    
    // Assign our mouse sensitivity, walljump hitsphere locations and cursor visibilty
    void Awake(){

        sens = MouseSensitivity.sens;
        layerMask = ~layerMask;
        rb = GetComponent<Rigidbody>();
        jumpClip = GetComponent<AudioSource>();
        playerCol = GetComponent<SphereCollider>();
        left.localPosition = new Vector3(playerCol.radius,0,0);
        right.localPosition = new Vector3(-playerCol.radius,0,0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    //Update all physics related events in FixedUpdate
    void FixedUpdate(){

        Gravity();
        Move();
        
    }

    //Update UI and camera rotation to ensure everything looks smooth to the user
    void Update(){

        ToggleUI();
        LookRotation();

    }
    
    //Assign custom gravity to the player
    void Gravity(){

        rb.AddForce(Vector3.down * grav,ForceMode.Acceleration);

    }
    private void Move(){

            // Check if we are currently touching any walls or the ground
            GroundCheck();
            WallCheck();

            //Define what why the camera should tilt depending on what side the player is touching a wall from   
            if(isGroundedWallL && !isGrounded && !isWallJumping){

                //Debug.Log("Rotating right");
                targetAngle = wallRotAngle;

            } else if(isGroundedWallR  && !isGrounded && !isWallJumping){

                //Debug.Log("Rotating left");
                targetAngle = -wallRotAngle;

            } else if(!isGroundedWallL && !isGroundedWallR){

                //Debug.Log("Centering");
                targetAngle = 0;

            }

            //Recieve input from the player
            Vector3 input = GetInput();
            Vector3 desiredMove = new Vector3();
            

            if (isGrounded){
                
                //If grounded accelerate the player left-right,forward-back depending on where they are facing
                isJumping = false;
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;

                desiredMove.x = desiredMove.x * movSpeed;
                desiredMove.y = rb.velocity.y;
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

                if(isGroundedWallL || isGroundedWallR){

                    //If jump is pressed and we aren't currently wall jumping then perform a wall jump
                    if(!isWallJumping){

                        //If left or right is pressed while jumping then perform the jump diagonally in the direction
                        if(input.z > 0 && (input.x > 0 || input.y > 0)){

                            //rb.velocity = Vector3.zero;
                            rb.velocity = new Vector3(rb.velocity.x * 0.25f,0,rb.velocity.z* 0.25f);
                            rb.AddRelativeForce(new Vector3(wallJumpForce * input.x, wallJumpForce * 1.85f, wallJumpForce), ForceMode.Impulse);
                            isWallJumping = true;

                            if(!jumpClip.isPlaying) {

                                jumpClip.Play();

                            }

                        } else if(input.z > 0 && (input.x == 0 && input.y == 0)){

                            rb.velocity = new Vector3(rb.velocity.x,0,rb.velocity.z);
                            rb.AddRelativeForce(new Vector3(0, wallJumpForce * 1.75f, 0), ForceMode.Impulse);
                            isWallJumping = true;

                            if(!jumpClip.isPlaying) {

                                jumpClip.Play();

                            }

                    }

                    } 

                }

                //If not grounded then move the player using airSpeed instead of movSpeed
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;

                desiredMove.x = desiredMove.x * airSpeed;
                desiredMove.z = desiredMove.z * airSpeed;
                rb.AddForce(desiredMove, ForceMode.Acceleration);

            } 

            //Base our speed on the x and z axis only
            currentSpeed = new Vector3(rb.velocity.x,0,rb.velocity.z).magnitude;

            
    }   
    private Vector3 GetInput(){
        
        //AD on X axis
        //WS on Y axis
        //Space on Z axis
        Vector3 input = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical"),
                z = Input.GetAxisRaw("Jump")
            };

        return input;
    }    

    public void ToggleUI() {

        //Press Escape to toggle cursor lock and the UI
        if(Input.GetKeyUp(KeyCode.Escape)){

            toggle = !toggle;

        }

        if(toggle) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UI.SetActive(true);
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UI.SetActive(false);
        }

    }

    public void LookRotation() {

            //Rotate the camera based on sens
            xRot += Input.GetAxis("Mouse X") * sens;
            yRot += Input.GetAxis("Mouse Y") * sens;

            //Debug.Log(xRot + " " + yRot);
            yRot = Mathf.Clamp (yRot, -90F, 90F);

            //cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x,0,currentAngle));
            cam.transform.localRotation = Quaternion.Euler (-yRot, 0f, cam.transform.localRotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler (0f, xRot, 0f);

        }

        public void WallRotate(float target){
        //Because the wall confirm animation takes time we must run it as a coroutine 
        StartCoroutine(

                RotateToTarget(
                        cam.transform.localRotation.eulerAngles.z,
                        target,
                        animDuration
                        )
        
            );
        
    }

    IEnumerator RotateToTarget(float start,float end,float dur){

        float timeElapsed = 0;

        currentAngle = cam.transform.localRotation.z;
        //if the animation hasn't hit the end keep running
        while(timeElapsed < animDuration) {

            //linearly Lerp from one angle to another
            currentAngle = Mathf.LerpAngle(start, end, timeElapsed / animDuration);
            timeElapsed += Time.deltaTime;
            cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x, 0, currentAngle));
            yield return null;
        }

        //Once the animation ends set the angle to its target
        cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x, 0, end));


    }
    void OnDrawGizmos() {

        //Draw our ground detection hitsphere
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - playerCol.radius, transform.position.z), playerCol.radius/4f);

        //Draw our wall detection hitspheres
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
        
    }
    private void GroundCheck(){

            wasGrounded = isGrounded;

            //Look for any colliders within the hitsphere
            if (Physics.CheckSphere(new Vector3(transform.position.x,transform.position.y - playerCol.radius,transform.position.z), playerCol.radius/4f,layerMask)){

                isGrounded = true;

            } else {

                isGrounded = false;

            }


            //if they weren't touching the ground before but now they are give the player the ability to jump
            if(!wasGrounded && isGrounded){

                isJumping = false;

            }
        }
    private void WallCheck(){

            if(isGroundedWallL != wasGroundedWallL || isGroundedWallR != wasGroundedWallR){

                //Debug.Log("Starting Rotate Animtion");
                WallRotate(targetAngle);

            } 

            wasGroundedWallL = isGroundedWallL;
            wasGroundedWallR = isGroundedWallR;

            //Look for any colliders within the left hitcapsule
            if(Physics.CheckCapsule(
            new Vector3(left.position.x,left.position.y,left.position.z + playerCol.radius)
            ,new Vector3(left.position.x,left.position.y,left.position.z - playerCol.radius)
            ,playerCol.radius * 0.75f
            ,layerMask)){

                isGroundedWallL = true;


            } else {

                isGroundedWallL = false;

            }

            //Look for any colliders within the right  hitcapsule
            if(Physics.CheckCapsule(
           new Vector3(right.position.x ,right.position.y,right.position.z + playerCol.radius)
            ,new Vector3(right.position.x ,right.position.y,right.position.z - playerCol.radius)
            ,playerCol.radius * 0.75f
            ,layerMask)){

                isGroundedWallR = true;

            } else {

                isGroundedWallR = false;

            }

            //if we aren't touching any walls give the player the ability to wall jump
            if(!isGroundedWallL && !isGroundedWallR){

                isWallJumping = false;

            }


        }

}
