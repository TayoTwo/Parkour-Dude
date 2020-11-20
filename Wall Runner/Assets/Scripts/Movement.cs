using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    public Camera cam;
    public float sens;
    public float movSpeed;
    public float jumpForce;

    public Rigidbody rb;

    public bool isGrounded;
    public bool wasGrounded;
    
    public bool isJumping;

    CapsuleCollider playerCol;
    Vector3 groundCheckNormal;

    private Quaternion charTargetRot;
    private Quaternion cameraTargetRot;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        playerCol = GetComponent<CapsuleCollider>();
        
    }

    // Update is called once per frame
    void FixedUpdate(){

        Move();
        RotateView();
        
    }
    
    private void Move(){

        GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && isGrounded)
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward*input.y + cam.transform.right*input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, groundCheckNormal).normalized;

                desiredMove.x = desiredMove.x * movSpeed;
                desiredMove.z = desiredMove.z * movSpeed;
                desiredMove.y = desiredMove.y * movSpeed;
                 rb.AddForce(desiredMove, ForceMode.Impulse);
            }

            if (isGrounded)
            {
                rb.drag = 5f;

                if (isJumping)
                {
                    rb.drag = 0f;
                    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
                    rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
                    isJumping = true;
                }

                if (!isJumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && rb.velocity.magnitude < 1f)
                {
                    rb.Sleep();
                }
            }
            else
            {
                rb.drag = 0f;
                
            }
            isJumping = false;
    }   

    private Vector2 GetInput(){
            
        Vector2 input = new Vector2
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            };

        if(Input.GetKeyUp(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else if(Input.GetMouseButtonUp(0)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }


        return input;
    }
    
    private void RotateView() {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            LookRotation();

            if (isGrounded)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                rb.velocity = velRotation*rb.velocity;
            }
        }

    public void LookRotation() {

            Transform camTransform = cam.transform;

            float yRot = Input.GetAxis("Mouse X") * sens;
            float xRot = Input.GetAxis("Mouse Y") * sens;

            charTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

            transform.localRotation = charTargetRot;
            camTransform.localRotation = cameraTargetRot;

        }

    private void GroundCheck(){

            wasGrounded = isGrounded;
            RaycastHit hitInfo;
            if (  
            Physics.SphereCast(
                
                transform.position, 
                playerCol.radius, 
                Vector3.down, 
                out hitInfo,
                (playerCol.height/2f) + 0.1f, 
                Physics.AllLayers, 
                QueryTriggerInteraction.Ignore
                
            ))
            {
                isGrounded = true;
                groundCheckNormal = hitInfo.normal;
            }
            else
            {
                isGrounded = false;
                groundCheckNormal = Vector3.up;
            }
            if (!wasGrounded && isGrounded && isJumping)
            {
                isJumping = false;
            }
        }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, -90F, 90F);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

}
