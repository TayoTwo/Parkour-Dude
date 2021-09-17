using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour{

    //When the player touches me restart the level, if it is a following platform destroy it
    void OnTriggerEnter(Collider col){

        if(col.gameObject.tag == "Player"){

            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );

        } else if (col.gameObject.tag == "FallingP"){

            Destroy(col.gameObject);

        }

    }
}
