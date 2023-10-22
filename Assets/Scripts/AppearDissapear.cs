using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearDissapear : MonoBehaviour
{
    public DialogueManager dialogueManager;

    [Header("Apper and Disapear")]
    public bool actorLeftVisibleBool; 
    public bool actorRightVisibleBool, actorMidleVisibleBool;
    Vector4 v4Visibility= new Vector4(1,1,1,1);
    float timerVisibility=0; public float intervalVisibility = 0.01f;
    public bool actorleftAppearingBool, actorLeftDisapearingBool, holdLeftTippingBool, 
        actorRightAppearingBool, actorRightDisapearingBool, holdRightTippingBool,
        actorMidleAppearingBool, actorMidleDisapearingBool, holdMiddleTippingBool;

    [Header("Informative Black Screen")]
    public bool infoBSlDisapearingBool;
    Vector4 v4InfoBS= new Vector4(1,1,1,1), v4IText;
    public Image infoBSImage;
    float timerInfoBS=0;
    


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
                    if(actorMidleAppearingBool && actorMidleDisapearingBool) actorMidleDisapearingBool = false;
                    
                    if(holdLeftTippingBool) if(!dialogueManager.typingBool) holdLeftTippingBool = false;
                    if(holdRightTippingBool) if(!dialogueManager.typingBool) holdRightTippingBool = false;
                    if(holdMiddleTippingBool) if(!dialogueManager.typingBool) holdMiddleTippingBool = false;
                    
                    
                    if(!holdLeftTippingBool)
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
                        dialogueManager._actorTagLeftSprite.color = v4Visibility;
                    }

                    
                    if(!holdRightTippingBool)
                    if(actorRightAppearingBool || actorRightDisapearingBool){
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
                        dialogueManager._actorTagRightSprite.color = v4Visibility;
                    }


                    if(!holdMiddleTippingBool)
                    if(actorMidleAppearingBool || actorMidleDisapearingBool){
                        v4Visibility = (Vector4)dialogueManager._actorMiddleSprite.color;
                        if(actorMidleDisapearingBool) {
                            v4Visibility.w -= 0.01f;
                            if(v4Visibility.w <= 0){
                                v4Visibility.w = 0;
                                actorMidleDisapearingBool = false;
                            }
                        }
                        if(actorMidleAppearingBool) {
                            v4Visibility.w += 0.01f;
                            if(v4Visibility.w >= 1){
                                v4Visibility.w = 1;
                                actorMidleAppearingBool = false;
                                actorMidleVisibleBool = true;
                            }   
                        }
                        dialogueManager._actorMiddleSprite.color = v4Visibility;
                        dialogueManager._actorTagMiddleSprite.color = v4Visibility;
                    }

                }
            }
    
        if(infoBSlDisapearingBool){
            timerInfoBS += Time.deltaTime;
            if(timerInfoBS > intervalVisibility){
                timerInfoBS = 0;
                v4InfoBS = (Vector4) infoBSImage.color;
                v4InfoBS.w -= 0.01f;
                v4IText = (Vector4) dialogueManager._informativeBSText.color;
                v4IText.w -= 0.01f;
                if(v4InfoBS.w <= 0){
                    v4Visibility.w = 0;
                    v4IText.w = 0;
                    infoBSlDisapearingBool = false;
                    OrderForBSPanel(false, false, true);
                } 
                infoBSImage.color = v4InfoBS;
                dialogueManager._informativeBSText.color = v4IText;                
            }
        }
    }


    public void OrderForBSPanel(bool open, bool proces, bool close){
        if(open){
            dialogueManager.menuManager.informativeBSPanelBool = true;
            dialogueManager._informativeBSText.text = "";
            dialogueManager._informativeBSButton.gameObject.SetActive(true);
            dialogueManager._informativeBlackScreenPanel.gameObject.SetActive(true);
            v4InfoBS = (Vector4) infoBSImage.color;
            v4InfoBS.w = 1;
            infoBSImage.color = v4InfoBS;
            v4IText = (Vector4) dialogueManager._informativeBSText.color;
            v4IText.w = 1;
            dialogueManager._informativeBSText.color = v4IText;
        }
        if(proces){
            infoBSlDisapearingBool = true;            
            dialogueManager._informativeBSButton.gameObject.SetActive(false);
            dialogueManager.audioManager.PlayMenuButtonClick(true);
            }
        if(close){
            dialogueManager._informativeBlackScreenPanel.gameObject.SetActive(false);
            dialogueManager.menuManager.informativeBSPanelBool = false;
            dialogueManager.OrderFromInfoBlackScreen();
        }
    }

    public void ButtonOnInfoBS(){OrderForBSPanel(false, true, false);}

    public void Clear () {
        v4Visibility = new Vector4(1,1,1, 0);
        dialogueManager._actorLeftSprite.color = v4Visibility;
        dialogueManager._actorTagLeftSprite.color = v4Visibility;
        actorLeftVisibleBool = false;
        dialogueManager._actorRightSprite.color = v4Visibility;
        dialogueManager._actorTagRightSprite.color = v4Visibility;
        actorRightVisibleBool = false;
        dialogueManager._actorMiddleSprite.color = v4Visibility;
        dialogueManager._actorTagMiddleSprite.color = v4Visibility;
        actorMidleVisibleBool = false;
    }

    public void OrderToAppearDisaper(bool itsLeftBool, bool itsRightBool, bool itsMiddleBool, bool appearBool){
        if(itsLeftBool){
            if(appearBool){
                actorleftAppearingBool = true; 
                actorLeftVisibleBool = false;
                v4Visibility = (Vector4) dialogueManager._actorLeftSprite.color;
                v4Visibility.w = 0;
                dialogueManager._actorLeftSprite.color = v4Visibility;
                dialogueManager._actorTagLeftSprite.color = v4Visibility;
            }else{
                actorLeftVisibleBool = false;
                actorLeftDisapearingBool = true;
            }
        }
        
        if(itsRightBool){
            if(appearBool){
                actorRightAppearingBool = true;
                actorRightVisibleBool = false;
                v4Visibility = (Vector4) dialogueManager._actorRightSprite.color;
                v4Visibility.w = 0;
                dialogueManager._actorRightSprite.color = v4Visibility;
                dialogueManager._actorTagRightSprite.color = v4Visibility;
            }else{
                actorRightVisibleBool = false;
                actorRightDisapearingBool = true;
            }
        }

        if(itsMiddleBool){
            if(appearBool){
                actorMidleAppearingBool = true;
                actorMidleVisibleBool = false;
                v4Visibility = (Vector4) dialogueManager._actorMiddleSprite.color;
                v4Visibility.w = 0;
                dialogueManager._actorMiddleSprite.color = v4Visibility;
                dialogueManager._actorTagMiddleSprite.color = v4Visibility;
            }else{
                actorMidleVisibleBool = false;
                actorMidleDisapearingBool = true;
            }
        }
        
    }

    public void OrderAlpha0(bool itsLeftBool, bool itsRightBool, bool itsMiddleBool){
        if(itsLeftBool){
            v4Visibility = (Vector4) dialogueManager._actorLeftSprite.color;
            v4Visibility.w = 0;
            dialogueManager._actorLeftSprite.color = v4Visibility;            
            dialogueManager._actorTagLeftSprite.color = v4Visibility;
            actorLeftVisibleBool = false;
        }
        
        if(itsRightBool){
            v4Visibility = (Vector4) dialogueManager._actorRightSprite.color;
            v4Visibility.w = 0;
            dialogueManager._actorRightSprite.color = v4Visibility;
            dialogueManager._actorTagRightSprite.color = v4Visibility;
            actorRightVisibleBool = false;
        }

        if(itsMiddleBool){
            v4Visibility = (Vector4) dialogueManager._actorMiddleSprite.color;
            v4Visibility.w = 0;
            dialogueManager._actorMiddleSprite.color = v4Visibility;
            dialogueManager._actorTagMiddleSprite.color = v4Visibility;
            actorMidleVisibleBool = false;
        }
    }

}
