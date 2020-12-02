using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public GameObject UI;
    public Camera cam;
    public float sens;
    public float wallRotAngle;
    public float currentAngle;
    public float targetAngle;
    public float lerpDuration;
    public float movSpeed;
    public float airSpeed;
    public float grav;
    public float currentSpeed;
    public float jumpForce;
    public float wallJumpForce;

    public float maxSpeed;

    public float drag;

    public Transform left;
    public Transform right;

    public Rigidbody rb;

    public bool isGrounded;
    public bool wasGrounded;

    public bool isGroundedWallL;
    public bool wasGroundedWallL;

    public bool isGroundedWallR;
    public bool wasGroundedWallR;
    
    public bool isJumping;

    public bool isWallJumping;

    SphereCollider playerCol;
    private Quaternion charTargetRot;
    private Quaternion cameraTargetRot;

    float xRot = 0; 
    float yRot = 0;

    int layerMask = 1 << 8;

    AudioSource jumpClip;
    
    // Start is called before the first frame update
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

    // Update is called once per frame
    void FixedUpdate(){

        Gravity();
        Move();
        
    }

    void Update(){

        ToggleUI();
        LookRotation();

    }
    
    void Gravity(){

        rb.AddForce(Vector3.down * grav,ForceMode.Acceleration);

    }
    private void Move(){

           
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

            GroundCheck();
            WallCheck();

            Vector3 input = GetInput();
            Vector3 desiredMove = new Vector3();
            

            if (isGrounded){

                isJumping = false;
                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;

                desiredMove.x = desiredMove.x * movSpeed;
                desiredMove.y = rb.velocity.y;
                desiredMove.z = desiredMove.z * movSpeed;
                
                rb.AddForce(desiredMove,ForceMode.Acceleration);

                // always move along the camera forward as it is the direction that it being aimed at

                if (!isJumping && input.z > 0){

                    // rb.drag = 0;
                    //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    rb.AddRelativeForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
                    isJumping = true;

                    if(!jumpClip.isPlaying) {

                        jumpClip.Play();

                    }

                }
                
                if(currentSpeed > maxSpeed){

                    rb.AddForce(new Vector3(-rb.velocity.normalized.x * drag,0,-rb.velocity.normalized.z * drag),ForceMode.Acceleration);

                }


            } else {

                if(isGroundedWallL || isGroundedWallR){

                    if (!isWallJumping){

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

                desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;

                desiredMove.x = desiredMove.x * airSpeed;
                desiredMove.z = desiredMove.z * airSpeed;
                rb.AddForce(desiredMove, ForceMode.Acceleration);

            } 


            currentSpeed = new Vector3(rb.velocity.x,0,rb.velocity.z).magnitude;

            
    }   
    private Vector3 GetInput(){
            
        Vector3 input = new Vector3
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical"),
                z = Input.GetAxisRaw("Jump")
            };

        return input;
    }    

    public void ToggleUI() {

        if(Input.GetKey(KeyCode.Escape)) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            UI.SetActive(true);
        } else if(Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            UI.SetActive(false);
        }

    }

    public void LookRotation() {

            xRot += Input.GetAxis("Mouse X") * sens;
            yRot += Input.GetAxis("Mouse Y") * sens;

            //Debug.Log(xRot + " " + yRot);
            yRot = Mathf.Clamp (yRot, -90F, 90F);

            //cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x,0,currentAngle));
            cam.transform.localRotation = Quaternion.Euler (-yRot, 0f, cam.transform.localRotation.eulerAngles.z);
            transform.rotation = Quaternion.Euler (0f, xRot, 0f);

        }

    public void WallRotate(float target){

        StartCoroutine(

                LerpToTarget(
                        cam.transform.localRotation.eulerAngles.z,
                        target,
                        lerpDuration
                        )
        
            );

        //cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x,0,currentAngle));
        // The step size is equal to speed times frame time.
        // Rotate our transform a step closer to the target's.
        

    }

    IEnumerator LerpToTarget(float start,float end,float dur){

        float timeElapsed = 0;

        //Debug.Log("Start:" + start + " End " + end);
        if(currentAngle != end){

            currentAngle = cam.transform.localRotation.z;

            while (timeElapsed < lerpDuration)
            {
                
                currentAngle = Mathf.LerpAngle(start, end, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x,0,currentAngle));
                yield return null;
            }


        }


    }
    void OnDrawGizmos() {

        Vector3 groundCheckPos = new Vector3(transform.position.x,transform.position.y - playerCol.radius,transform.position.z);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheckPos, playerCol.radius/4f);

        Gizmos.color = Color.red;

        

        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(left.localPosition.x,left.localPosition.y,left.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z + playerCol.radius)), playerCol.radius * 0.75f);
        Gizmos.DrawSphere(transform.TransformPoint(new Vector3(right.localPosition.x ,right.localPosition.y,right.localPosition.z - playerCol.radius)), playerCol.radius * 0.75f);
        
    }
    private void GroundCheck(){

            wasGrounded = isGrounded;
            if (Physics.CheckSphere(new Vector3(transform.position.x,transform.position.y - playerCol.radius,transform.position.z), playerCol.radius/4f,layerMask)){

                isGrounded = true;

            } else {

                isGrounded = false;

            }
            if (!wasGrounded && isGrounded && isJumping){

                isJumping = false;

            }
        }
    private void WallCheck(){

            if(isGroundedWallL != wasGroundedWallL || isGroundedWallR != wasGroundedWallR){

                Debug.Log("Starting Rotate");
                WallRotate(targetAngle);

            } 

            wasGroundedWallL = isGroundedWallL;
            wasGroundedWallR = isGroundedWallR;
            if (Physics.CheckCapsule(
            new Vector3(left.position.x,left.position.y,left.position.z + playerCol.radius)
            ,new Vector3(left.position.x,left.position.y,left.position.z - playerCol.radius)
            ,playerCol.radius * 0.75f
            ,layerMask)){

                isGroundedWallL = true;


            } else {

                isGroundedWallL = false;

            }

            if (Physics.CheckCapsule(
           new Vector3(right.position.x ,right.position.y,right.position.z + playerCol.radius)
            ,new Vector3(right.position.x ,right.position.y,right.position.z - playerCol.radius)
            ,playerCol.radius * 0.75f
            ,layerMask)){

                isGroundedWallR = true;

            } else {

                isGroundedWallR = false;

            }

            if (!isGroundedWallL && !isGroundedWallR){

                isWallJumping = false;

            }


        }

}
