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

        rb = GetComponent<Rigidbody>();
        orign = transform.position;
        startPosition = orign;
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

    void MovePlat(Vector3 targetPosition)
    {
        t = currentProgress / duration;

        t = t * t * (3f - 2f * t);

        //Debug.Log(currentProgress / duration);

        if (currentProgress < duration){
            
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, t));
            currentProgress += Time.fixedDeltaTime;

        } else {

            rb.MovePosition(targetPosition);
            currentProgress = 0;
            t = 0;
            SwitchAxis();

        }

        // if(currentProgress >= 0.01f){

        //     transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, duration);
        //     currentProgress = Vector3.Distance(transform.position,targetPosition);

        // } else {

        //     rb.MovePosition(targetPosition);
        //     currentProgress = 0.01f;
        //     SwitchAxis();

        // }

        


    }
}
