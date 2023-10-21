using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FrontCoverScript : MonoBehaviour
{
    float timerTyping=0;
    public float typpingSpeed = 0.05f;
    public string mesageString;
    int mesageLenght=0;
    public TMP_Text mesageText;

    
    void Update()
    {
        timerTyping += Time.deltaTime;
        if(timerTyping > typpingSpeed){
            timerTyping = 0;
        }
    }
}
