using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour
{
    [Header("Main stuff")]
    [TextArea] public string openMesage; public Sprite openSprite; public AudioClip openSfxAudiClip;
    [TextArea] public string endMesage; public Sprite endSprite; public AudioClip endAudioClip;
    [TextArea] public string[] creditsMesages;

    [Header("References")]
    public DialogueManager dialogueManager;
    public MenuPanelManager menuPanelManager;

    public Transform thisPanel, smallTextPanel, creditPanel;
    public Image displayedImage;
    public TMP_Text smallText, creditText;

    [Header("Values")]
    public float tippingSpeed =0.05f;
    float timer=0;
    int numberOfCredits=0, actualCreditsPage=0;
    bool typingBool=false, creditsBool=false;
    string actualMesage="", targetMesage="";
    int mesageStep=0, mesageLenght=0;

    void Update()
    {
        if(typingBool){
            timer += Time.deltaTime;
            if(timer > tippingSpeed){
                timer = 0;
                actualMesage = targetMesage.Substring(0, mesageStep);
                actualMesage += "<color=#00000000>" + targetMesage.Substring(mesageStep);
                if(!creditsBool){smallText.text = actualMesage;}
                else{creditText.text = actualMesage;}
                mesageStep++;
                if(mesageStep > mesageLenght) typingBool = false;
            }
        }
    }


    public void NextButton(){
        dialogueManager.PlaySfxAudio(menuPanelManager.uiButtonPositiveAC);
        if(menuPanelManager.gameIsOnBool){
            thisPanel.gameObject.SetActive(false);
            dialogueManager.StartTheGame();
        }else{
            if(!creditsBool){
                smallTextPanel.gameObject.SetActive(false);
                displayedImage.gameObject.SetActive(false);
                creditPanel.gameObject.SetActive(true);
                creditsBool = true;
                numberOfCredits = creditsMesages.Length;
                actualCreditsPage = 0;
                SetTypping(creditsMesages[0]);
            }else{
                actualCreditsPage++;
                if(actualCreditsPage >= numberOfCredits){
                    thisPanel.gameObject.SetActive(false);
                    menuPanelManager.creditPanelOnBool = false;
                    menuPanelManager.OrderToOperateMainMenu();
                    menuPanelManager.PlayMainMenuMusic();
                }else{
                    SetTypping(creditsMesages[actualCreditsPage]);
                }
            }
        }
    }

    public void OpenStartEndCreditsPanel(){
            thisPanel.gameObject.SetActive(true);
            smallTextPanel.gameObject.SetActive(true);
            displayedImage.gameObject.SetActive(true);
            creditPanel.gameObject.SetActive(false);
            menuPanelManager.creditPanelOnBool = true;
        if(menuPanelManager.gameIsOnBool){
            displayedImage.sprite = openSprite;
            dialogueManager.PlaySfxAudio(openSfxAudiClip);
            SetTypping(openMesage);
        }else{
            
            displayedImage.sprite = endSprite;
            dialogueManager.PlaySfxAudio(endAudioClip);
            SetTypping(endMesage);
        }
    }


    private void SetTypping(string mesageString){
        smallText.text = "";
        targetMesage = mesageString;
        mesageLenght = targetMesage.Length;
        mesageStep = 0;
        typingBool = true;
    }


    
}
