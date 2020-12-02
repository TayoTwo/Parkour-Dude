using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public Slider slider;

    AudioSource audioSource;

    public void Start() {

        audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void OnClick() {

        //Set the user's sensitivity globally
        MouseSensitivity.sens = slider.value;
        //Play a sound effect before starting the game
        audioSource.Play();
        StartCoroutine(LoadScene(audioSource.clip.length));

    }
    //Start the game
    public IEnumerator LoadScene(float waitTime) {

        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

}
