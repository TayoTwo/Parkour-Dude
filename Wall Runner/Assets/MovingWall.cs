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
    public float lerpDur = 5;

    public int direction = 1;

    public float currentTime;

    Rigidbody rb;
    Vector3 startPosition;
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

        Debug.Log(currentTime / lerpDur);

        if (currentTime < lerpDur){
            
            rb.MovePosition(Vector3.Lerp(startPosition, targetPosition, currentTime / lerpDur));
            currentTime += Time.fixedDeltaTime;

        } else {

            rb.MovePosition(targetPosition);
            currentTime = 0;
            SwitchAxis();

        }
       

    }
}
