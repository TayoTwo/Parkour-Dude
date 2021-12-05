using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour{


    public static float sens = 4;
    public static float fov;

    public float x;
    public float y;
    public Vector3 inputs;
    float jump;
    GameObject UI;
    bool toggle;

    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InputManager");
        //Debug.Log(objs.Length);

        if(objs.Length > 1 || SceneManager.GetActiveScene().buildIndex == 0) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;


    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode){

        Debug.Log("OnSceneLoaded: " + scene.name);
         
        GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().ResetVars();
        UI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().pauseUI;

    }

    // Update is called once per frame
    void Update(){


        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 
                            Input.GetAxisRaw("Vertical"),
                            Input.GetAxis("Jump"));

        ToggleUI();
        
    }

    public void ChangeSens(float s){

        sens = s;

    }

    public void ChangeFOV(float f){

        fov = f;

    }

    public void ToggleUI() {

        //Press Escape to toggle cursor lock and the UI
        if(Input.GetKeyUp(KeyCode.Escape)){

            toggle = !toggle;

            if(toggle) {

                ChangeMouseState(CursorLockMode.None,true);
                UI.SetActive(true);

            } else {

                ChangeMouseState(CursorLockMode.Locked,false);
                UI.SetActive(false);

            }

        }

        

    }

    
    public void ChangeMouseState(CursorLockMode cursorMode, bool visible){

        Cursor.lockState = cursorMode;
        Cursor.visible = visible;

    }

}
