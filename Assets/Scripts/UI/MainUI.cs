using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI:MonoBehaviour {

    public GameObject winScreen;
    public GameObject timer;
    public GameObject nextLevelScreen;
    public GameObject pauseUI;

    void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MainUI");
        //Debug.Log(objs.Length);

        if(objs.Length > 1 || SceneManager.GetActiveScene().buildIndex == 0) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);



    }

    void Start(){

        ResetVars();

    }

    public void ResetVars() {

        Timer.stop = false;
        Timer.time = 0f;
        nextLevelScreen.SetActive(false);
        winScreen.SetActive(false);

    }
}
