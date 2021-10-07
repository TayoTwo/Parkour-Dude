using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstPersonCam : MonoBehaviour{

    InputManager im;

    //Variables used for the wall confirm animation
    public float wallRotAngle;
    public float currentAngle;
    public float targetAngle;
    public float animDuration;

    Camera cam;
    float sens;
    float x;
    float y;

    void Awake(){

        sens = InputManager.sens;
        im = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        im.ChangeMouseState(CursorLockMode.Locked, false);
        cam = GetComponentInChildren<Camera>();
        

    }

    void Update(){

        LookRotation();

    }



    public void LookRotation() {



        //Rotate the camera based on sens
        x += im.x * sens;
        y += im.y * sens;

        //Debug.Log(xRot + " " + yRot);
        y = Mathf.Clamp (y, -90F, 90F);

        //cam.transform.localRotation = Quaternion.Euler(new Vector3(cam.transform.localRotation.eulerAngles.x,0,currentAngle));
        cam.transform.localRotation = Quaternion.Euler (-y, 0f, cam.transform.localRotation.eulerAngles.z);
        transform.rotation = Quaternion.Euler (0f, x, 0f);

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

}
