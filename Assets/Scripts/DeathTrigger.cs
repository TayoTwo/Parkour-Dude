using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour{

    void OnTriggerEnter(Collider col){

        if(col.gameObject.tag == "Player"){

            SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );

        } else if (col.gameObject.tag == "FallingP"){

            Destroy(col.gameObject);

        }

    }
}
