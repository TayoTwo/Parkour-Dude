using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowSens : MonoBehaviour
{

    public Slider slider;
    public TMP_Text text;

    // Update is called once per frame
    void Update(){

        text.text = "Sens: " + slider.value.ToString();
        
    }
}
