using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToStart : MonoBehaviour
{

    AudioSource audioSource;

    public void Awake() {

        audioSource = GetComponent<AudioSource>();


    }
    public void OnSelect() {

        Debug.Log("I've been clicked");
        //Wait till the sound effect is done before going back to the main menu
        audioSource.Play();
        StartCoroutine(LoadScene(audioSource.clip.length));

    }
    public IEnumerator LoadScene(float waitTime) {

        yield return new WaitForSeconds(waitTime);
        Destroy(GameObject.FindGameObjectWithTag("MainUI"));
        SceneManager.LoadScene(0);

    }


}
