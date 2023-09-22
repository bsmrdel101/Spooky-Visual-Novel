using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuPanelManager : MonoBehaviour
{
    [Header("Variables")]
    public bool menuIsOnBool; 
    public bool gameIsOnBool, creditPanelOnBool;

    [Header("references")]
    public DialogueManager dialogueManager;
    public Transform _menuPanel, _menuButtonsPanel;

    //public Transform 
    public AudioClip mainMenuMusic;
    public AudioClip uiButtonPositiveAC, uiButtonNegativeAC;
    public CreditsManager creditsManager;



    private void Start() {
        OrderToOperateMainMenu();
        PlayMainMenuMusic();
    }


    public void OrderToOperateMainMenu(){
        if(!menuIsOnBool){MenuPanelONOff(true);
        }else{MenuPanelONOff(false);}
        dialogueManager.PlayUiAudio(uiButtonPositiveAC);     
    }


    public void OrderToStartTheNewGame(){
        if(!gameIsOnBool){
            MenuPanelONOff(false);
            gameIsOnBool = true;
            ShakeTheMenuButtonsPanel();
            //dialogueManager.StartTheGame();
            dialogueManager.StopTheMusic();
            creditsManager.OpenStartEndCreditsPanel();
            dialogueManager.PlayUiAudio(uiButtonPositiveAC);
        }else{dialogueManager.PlayUiAudio(uiButtonNegativeAC);}
    }

    public void OrderToClearTheGame(){
        if(gameIsOnBool){
            dialogueManager.ClearTheGame();
            ShakeTheMenuButtonsPanel();
            PlayMainMenuMusic();
            dialogueManager.PlayUiAudio(uiButtonPositiveAC);
        }else{dialogueManager.PlayUiAudio(uiButtonNegativeAC);}
    }

    public void OrderToSaveTheGame(){
        if(gameIsOnBool){}
    }

    public void OrderToLoadTheGame(){
        if(!gameIsOnBool){}
    }

    public void OrderToConinueOnCurrentGame(){
        if(gameIsOnBool){
            OrderToOperateMainMenu();
            dialogueManager.PlayUiAudio(uiButtonPositiveAC);
        }else{dialogueManager.PlayUiAudio(uiButtonNegativeAC);}
    }

    public void OrderToExitTheGame(){
        OrderToSaveTheGame();
        OrderToClearTheGame();
        Application.Quit();
    }



    void MenuPanelONOff(bool option){
        if(option){
            menuIsOnBool = true;
            _menuPanel.gameObject.SetActive(true);
        }else{
            menuIsOnBool = false;
            _menuPanel.gameObject.SetActive(false);
        }
    }

    void ShakeTheMenuButtonsPanel(){
        _menuButtonsPanel.gameObject.SetActive(false);
        _menuButtonsPanel.gameObject.SetActive(true);
    }

    public void PlayMainMenuMusic(){
        if(mainMenuMusic!= null){
            dialogueManager.ChangeBacgroundMusic(mainMenuMusic);
        }
    }

    


}
