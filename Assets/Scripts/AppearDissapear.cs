using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearDissapear : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Apper and Disapear")]
    public bool actorLeftVisibleBool; public bool actorRightVisibleBool;
    Vector4 v4Visibility= new Vector4(1,1,1,1);
    float timerVisibility=0; public float intervalVisibility = 0.01f;
    public bool actorleftAppearingBool, actorLeftDisapearingBool, 
        actorRightAppearingBool, actorRightDisapearingBool;


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(actorleftAppearingBool || actorLeftDisapearingBool ||
            actorRightAppearingBool || actorRightDisapearingBool){
                timerVisibility += Time.deltaTime;
                if(timerVisibility >= intervalVisibility){
                    timerVisibility = 0;
                    if(actorleftAppearingBool && actorLeftDisapearingBool) actorLeftDisapearingBool = false;
                    if(actorRightAppearingBool && actorRightDisapearingBool) actorRightDisapearingBool = false;
                    
                    if(actorleftAppearingBool || actorLeftDisapearingBool){
                        //v4Visibility.w = dialogueManager._actorLeftSprite.color.a;
                        v4Visibility = (Vector4)dialogueManager._actorLeftSprite.color;
                        if(actorLeftDisapearingBool) {
                            v4Visibility.w -= 0.01f;
                            if(v4Visibility.w <= 0){
                                v4Visibility.w = 0;
                                actorLeftDisapearingBool = false;                                
                            }
                        }
                        if(actorleftAppearingBool) {
                            v4Visibility.w += 0.01f;
                            if(v4Visibility.w >= 1){
                                v4Visibility.w = 1;
                                actorleftAppearingBool = false;
                                actorLeftVisibleBool = true;
                            }   
                        }
                        dialogueManager._actorLeftSprite.color = v4Visibility;
                    }

                    if(actorRightAppearingBool || actorRightDisapearingBool){
                        //v4Visibility.w = dialogueManager._actorRightSprite.color.a;
                        v4Visibility = (Vector4)dialogueManager._actorRightSprite.color;
                        if(actorRightDisapearingBool) {
                            v4Visibility.w -= 0.01f;
                            if(v4Visibility.w <= 0){
                                v4Visibility.w = 0;
                                actorRightDisapearingBool = false;
                            }
                        }
                        if(actorRightAppearingBool) {
                            v4Visibility.w += 0.01f;
                            if(v4Visibility.w >= 1){
                                v4Visibility.w = 1;
                                actorRightAppearingBool = false;
                                actorRightVisibleBool = true;
                            }   
                        }
                        dialogueManager._actorRightSprite.color = v4Visibility;
                    }
                }
            }
    }


    public void Clear () {
        v4Visibility = new Vector4(1,1,1, 0);
        dialogueManager._actorLeftSprite.color = v4Visibility;
        actorLeftVisibleBool = false;
        dialogueManager._actorRightSprite.color = v4Visibility;
        actorRightVisibleBool = false;
    }

    public void OrderToAppearDisaper(bool itsLeftBool, bool appearBool){
        if(itsLeftBool){
            if(appearBool){
                if(actorLeftVisibleBool == false) actorleftAppearingBool = true; 
            }else{
                actorLeftVisibleBool = false;
                actorLeftDisapearingBool = true;
            }
        }else{
            if(appearBool){
                if(actorRightVisibleBool == false) actorRightAppearingBool = true;
            }else{
                actorRightVisibleBool = false;
                actorRightDisapearingBool = true;
            }
        }
        
    }
}
