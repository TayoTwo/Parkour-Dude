using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour{

    public static float time;
    public static bool stop = false;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {

        text = GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update(){

        if(!stop) {

            time += Time.deltaTime;
            text.text = time.ToString("F2");

        }
        
    }

    public static void Stop() {

        stop = true;

    }


}
