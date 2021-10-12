using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis
{
    X = 0,
    Y = 1,
    Z = 2
}


public class MovingWall : MonoBehaviour
{

    public bool isPlain;
    public static float platForce = 0.5f;
    public Axis axis;

    public float distance;
    public Vector3 orign;
    Vector3 positionToMoveTo;
    public float duration = 5;

    public int direction = 1;

    public float currentProgress;

    private Vector3 velocity = Vector3.zero;

    Rigidbody rb;
    Vector3 startPosition;
    float t;
    void Start()
    {
        if(transform.rotation.z == 90f) {

            isPlain = false;

        } else {

            isPlain = true;

        }

        rb = GetComponent<Rigidbody>();
        orign = transform.position;
        startPosition = orign;

        //Set the target based on the selected axis
        if(axis == Axis.X){

            positionToMoveTo = orign + (new Vector3(distance,0,0) * direction);

        } else if(axis == Axis.Y){

            positionToMoveTo = orign + (new Vector3(0,distance,0) * direction);

        } else if(axis == Axis.Z){

            positionToMoveTo = orign + (new Vector3(0,0,distance) * direction);
            
        }

    }

    void FixedUpdate(){

        MovePlat(positionToMoveTo);

    }

    void SwitchAxis(){

        //Swap the target destination
        direction *= -1;
        startPosition = transform.position;

        if(axis == Axis.X){

            positionToMoveTo = orign + (new Vector3(distance,0,0) * direction);

        } else if(axis == Axis.Y){

            positionToMoveTo = orign + (new Vector3(0,distance,0) * direction);

        } else if(axis == Axis.Z){

            positionToMoveTo = orign + (new Vector3(0,0,distance) * direction);
            
        }

    }

    void OnCollisionStay(Collision col) {

        Debug.Log("Touching :flushed:");
        Debug.Log(rb.velocity);

        if(col.gameObject.tag == "Player" && isPlain) {

            col.gameObject.GetComponent<Rigidbody>().AddForce(rb.velocity * platForce);

        }

    }

    void MovePlat(Vector3 targetPosition)
    {
        t = currentProgress / duration;

        //Interpolate the platform using smoothstep movement https://en.wikipedia.org/wiki/Smoothstep
        t = t * t * (3f - 2f * t);

        if (currentProgress < duration){
            
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));
            currentProgress += Time.fixedDeltaTime;

        } else {

            //When we reach the end swap the direction the platform is moving
            rb.MovePosition(targetPosition);
            currentProgress = 0;
            t = 0;
            SwitchAxis();

        }

        
    }
}
