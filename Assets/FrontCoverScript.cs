using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FrontCoverScript : MonoBehaviour
{
    public bool appearingBool, disappearingBool, waitingBool, showedBool;
    float timerTyping=0, timerWait=0;
    public float typpingSpeed = 0.05f, waitInterval=1;
    public string mesageString;
    string mesageActual="";
    int mesageLenght=0, mesageTextPosition=0;
    [TextArea]
    public TMP_Text mesageText;

    
    void Update()
    {
        timerTyping += Time.deltaTime;
        if(timerTyping > typpingSpeed){
            timerTyping = 0;

            mesageActual = "";
            if(appearingBool){
                mesageActual = mesageString.Substring(0, mesageTextPosition);
                mesageActual += "<color=#00000000>" + mesageString.Substring(mesageTextPosition);
            }
            if(disappearingBool){
                mesageActual = "<color=#00000000>" + mesageString.Substring(0, mesageTextPosition);
                mesageActual += mesageString.Substring(mesageTextPosition);
            }

            mesageTextPosition++;
            if(mesageTextPosition >= mesageLenght){
                waitingBool = true;
                mesageTextPosition = 0;
                CheckTheMesage();
                if(appearingBool){
                    appearingBool = false;
                    showedBool = true;
                }
                if(disappearingBool){
                    disappearingBool = false;
                    showedBool = false;
                }
            }

            mesageText.text = mesageActual;
        }

        if(waitingBool){
            timerWait += Time.deltaTime;
            if(timerWait > waitInterval){
                timerWait = 0;
                if(showedBool){disappearingBool = true;}
                else{appearingBool = true;}
                waitingBool = false;
            }
        }
    }

    void Start(){
        mesageText.text = "";
        CheckTheMesage();
        appearingBool= true;
    }

    void CheckTheMesage(){
        mesageLenght = mesageString.Length;
        mesageTextPosition = 0;
    }
}
