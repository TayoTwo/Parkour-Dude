using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour{

    public bool isTrue;

    void OnTriggerEnter(Collider other) {

        if(other.tag != "Player"){

            isTrue = true;

        } else {

            isTrue = false;

        }
        
    }

    void OnTriggerStay(Collider other){

        if(other.tag != "Player"){

            isTrue = true;

        } else {

            isTrue = false;

        }

    }

    void OnTriggerExit(Collider other) {

        isTrue = false;
        
    }
}
