using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreen;
    public GameObject nextLevelScreen;
    public float rotSpeed;

    private void Update() {
        //Rotate the coin at a set speed
        transform.Rotate(0, rotSpeed * Time.deltaTime,0);

    }

    void OnTriggerEnter(Collider col){

        if(col.gameObject.tag == "Player"){

            Timer.Stop();
            //Debug.Log(SceneManager.GetActiveScene().buildIndex);

            //If its not the last level go to the next!
            if(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) {

                Destroy(col.gameObject.GetComponent<Movement>());
                col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().nextLevelScreen.SetActive(true);

            } else {

                //End the game,show winscreen
                Destroy(col.gameObject.GetComponent<Movement>());
                col.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>().winScreen.SetActive(true);

            }

        }

    }
}
