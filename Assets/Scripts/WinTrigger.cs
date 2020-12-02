using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinTrigger : MonoBehaviour
{
    public GameObject winScreen;
    void OnTriggerEnter(Collider col){

        if(col.gameObject.tag == "Player"){

            if(SceneManager.GetActiveScene().buildIndex < SceneManager.GetAllScenes().Length) {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

            } else {

                Destroy(col.gameObject.GetComponent<Movement>());
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                winScreen.SetActive(true);

            }

        }

    }
}
