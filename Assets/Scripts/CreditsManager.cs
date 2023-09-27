using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Linq;

public class CreditsManager : MonoBehaviour
{
    [Header("Main stuff")]
    [TextArea(10, 100)] public string openMesage; public Sprite openSprite; 
    public AudioClip openVoiceActorClip, openSfxAudiClip;
    public int endingNumber=0;
    public List<Ending> listOfEndings;
    [System.Serializable]
    public class Ending{
        [SerializeField] public string name;
        [SerializeField, TextArea] public string endMesage;
        [SerializeField, TextArea] public string[] additionalCreditsPages;
        [SerializeField] public Sprite endSprite;
        [SerializeField] public AudioClip endVoiceActorClip, endAudioClip;
    }
    
    [TextArea] public string[] creditsMesages;
    public List<String> listOfCreditsPages = new List<string>();


    [Header("References")]
    public Transform thisPanel, smallTextPanel, creditPanel;
    public Image displayedImage;
    public TMP_Text smallText, creditText;

    [Header("Values")]
    public bool openingBool, endingBool;
    //public float typpingSpeed =0.05f;
    float timer=0;
    int numberOfCredits=0, actualCreditsPage=0;
    public bool typingBool=false, creditsBool=false;
    string actualMesage="", targetMesage="";
    int mesageStep=0, mesageLenght=0;

    [Header("Managers")]
    public DialogueManager dialogueManager;
    public MenuPanelManager menuPanelManager;
    public AudioManager audioManager;
    
    
    void Update()
    {
        if(typingBool){
            timer += Time.deltaTime;
            if(timer > dialogueManager.typpingSpeed){
                timer = 0;
                actualMesage = targetMesage.Substring(0, mesageStep);
                actualMesage += "<color=#00000000>" + targetMesage.Substring(mesageStep);
                if(dialogueManager.stopReadingText){
                    actualMesage = targetMesage;
                    typingBool = false;
                    dialogueManager.stopReadingText = false;
                }

                if(menuPanelManager.settingPanleOnBool){
                    menuPanelManager.settingsManager.typingSpeedSampleText.text = actualMesage;
                }

                if(!creditsBool){smallText.text = actualMesage;}
                else{creditText.text = actualMesage;}
                mesageStep++;
                if(mesageStep > mesageLenght) typingBool = false;
            }
        }
    }


    public void NextButton(){
        Debug.Log("Next button on credits: opening " +openingBool + " ending "+ endingBool);
        audioManager.PlayMenuButtonClick(true);
        if(openingBool){
            thisPanel.gameObject.SetActive(false);
            menuPanelManager.creditPanelOnBool = false;
            openingBool = false;
            dialogueManager.StartTheGame();
        }
        
        if(endingBool){
            if(!creditsBool){
                smallTextPanel.gameObject.SetActive(false);
                displayedImage.gameObject.SetActive(false);
                creditPanel.gameObject.SetActive(true);
                creditsBool = true;
                numberOfCredits = listOfCreditsPages.Count;
                actualCreditsPage = 0;
                SetTypping(listOfCreditsPages[0]);
            }else{
                actualCreditsPage++;
                if(actualCreditsPage >= numberOfCredits){
                    thisPanel.gameObject.SetActive(false);
                    endingBool = false;
                    creditsBool = false;
                    menuPanelManager.creditPanelOnBool = false;
                    audioManager.StopActorsAndSfxAudio();
                    audioManager.MainMenuMusicDominator(true, false, false);
                    menuPanelManager.OrderToOperateMainMenu();
                    audioManager.MainMenuMusicDominator(false, false, true);
                }else{
                    SetTypping(listOfCreditsPages[actualCreditsPage]);
                }
            }
        }
    }

    public void OpenStartEndCreditsPanel(bool openingB, bool endingB){
        if(endingNumber >= listOfEndings.Count){
            Debug.Log("Warning! Trying to acess non exsisting ending!");
            endingNumber = 0;
        }
        if(listOfEndings[endingNumber] == null){
            Debug.Log("Warning! Trying to acess non exsisting, not created ending!");
            endingNumber = 0;
        }

            mesageStep = 0;
            mesageLenght = 0;
            smallText.text = "";
            creditText.text = "";


            thisPanel.gameObject.SetActive(true);
            smallTextPanel.gameObject.SetActive(true);
            displayedImage.gameObject.SetActive(true);
            creditPanel.gameObject.SetActive(false);
            menuPanelManager.creditPanelOnBool = true;
        
        if(openingB){
            openingBool = true;
            displayedImage.sprite = openSprite;
            if(openVoiceActorClip != null)
                audioManager.PlayVoiceActorAudio(openVoiceActorClip, true, true);
            audioManager.PlaySfxAudio(openSfxAudiClip);            
            SetTypping(openMesage);
        }
        
        if(endingB){
            endingBool = true;
            listOfCreditsPages.Clear();
            if(listOfEndings[endingNumber] != null){
                displayedImage.sprite = listOfEndings[endingNumber].endSprite;
                if(listOfEndings[endingNumber].endVoiceActorClip != null)
                    audioManager.PlayVoiceActorAudio(listOfEndings[endingNumber].endVoiceActorClip, true, true);
                if(listOfEndings[endingNumber].endAudioClip != null)
                    audioManager.PlaySfxAudio(listOfEndings[endingNumber].endAudioClip);
                SetTypping(listOfEndings[endingNumber].endMesage);
                int operationValue = listOfEndings[endingNumber].additionalCreditsPages.Count();
                if(operationValue > 0){
                    listOfCreditsPages.AddRange(listOfEndings[endingNumber].additionalCreditsPages); 
                }
                listOfCreditsPages.AddRange(creditsMesages);  
            }else{
                Debug.Log("Warning, searching ending not found!");
            }
            
        }
    }


    public void SetTypping(string mesageString){
        dialogueManager.stopReadingText = false;
        smallText.text = "";
        targetMesage = mesageString;
        mesageLenght = targetMesage.Length;
        mesageStep = 0;
        typingBool = true;
        Debug.Log("Setting typping");
    }


    
}
