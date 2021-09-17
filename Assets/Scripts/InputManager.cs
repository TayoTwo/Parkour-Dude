using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour{


    public static float sens = 4;
    public static float fov;

    public float jump;
    public float x;
    public float y;
    public Vector3 input;

    // Start is called before the first frame update
    void Start(){


    }

    // Update is called once per frame
    void Update(){

        Vector3 i = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),Input.GetAxis("Jump"));

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        input = i;
        
    }

    public void ChangeSens(float s){

        sens = s;

    }

    public void ChangeFOV(float f){

        fov = f;

    }

}
