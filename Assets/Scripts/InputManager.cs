using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour{


    public static float sens = 4;
    public static float fov;

    public float jump;
    public float x;
    public float y;
    public Vector3 input;

    GameObject UI;
    bool toggle;

    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("InputManager");
        //Debug.Log(objs.Length);

        if(objs.Length > 1 || SceneManager.GetActiveScene().buildIndex == 0) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    void Start(){

        GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().ResetVars();
        UI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().pauseUI;

    }

    // Update is called once per frame
    void Update(){

        Vector3 i = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),Input.GetAxis("Jump"));

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        input = i;

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
