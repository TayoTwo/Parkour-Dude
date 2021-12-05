using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTrigger : MonoBehaviour{

    public bool isTrue;
    public string objTag;

    void OnTriggerEnter(Collider other) {

        tag = other.tag;

        if(other.tag != "Player"){

            isTrue = true;

        } else {

            isTrue = false;

        }
        
    }

    void OnTriggerStay(Collider other){

        tag = other.tag;

        if(other.tag != "Player"){

            isTrue = true;

        } else {

            isTrue = false;

        }

    }

    void OnTriggerExit(Collider other) {
        
        tag = "";
        isTrue = false;
        
    }
}
