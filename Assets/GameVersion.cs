using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TextCore;
using TMPro;

public class GameVersion : MonoBehaviour
{

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Awake(){

        text = GetComponent<TextMeshProUGUI>();

        if(Application.isEditor){

            text.text = "Editor";
 
        } else {

            text.text = Application.version;

        }
        
    }

}
