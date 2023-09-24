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
    public bool muteMusicBool;

    [Header("references")]    
    public Transform _menuPanel, _menuButtonsPanel;

    
    
    
    [Header("Managers")]
    public DialogueManager dialogueManager;
    public CreditsManager creditsManager;
    public AudioManager audioManager;



    private void Start() {
        OrderToOperateMainMenu();
        audioManager.MainMenuMusicDominator(false, true, false);
    }


    public void OrderToOperateMainMenu(){
        if(!menuIsOnBool){MenuPanelONOff(true);
        }else{MenuPanelONOff(false);}
        audioManager.PlayMenuButtonClick(true);     
    }


    public void OrderToStartTheNewGame(){
        if(!gameIsOnBool){
            MenuPanelONOff(false);
            gameIsOnBool = true;
            ShakeTheMenuButtonsPanel();
            //dialogueManager.StartTheGame();
            dialogueManager.StopTheMusic();
            dialogueManager.ChangeNpcImage(null, true);
            dialogueManager.ChangeNpcImage(null, false);
            creditsManager.OpenStartEndCreditsPanel(true, false);
            audioManager.PlayMenuButtonClick(true);   
        }else{
            audioManager.PlayMenuButtonClick(false);   
            }
    }

    public void OrderToClearTheGame(){
        if(gameIsOnBool){
            dialogueManager.ClearTheGame();
            ShakeTheMenuButtonsPanel();
            audioManager.MainMenuMusicDominator(false, false, true);
            audioManager.PlayMenuButtonClick(true);   
        }else{audioManager.PlayMenuButtonClick(false);   }
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
            audioManager.PlayMenuButtonClick(true);   
        }else{audioManager.PlayMenuButtonClick(false);   }
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
            audioManager.MainMenuMusicDominator(false, false, true);
        }else{
            menuIsOnBool = false;
            _menuPanel.gameObject.SetActive(false);
            audioManager.MainMenuMusicDominator(true, false, false);
        }
    }

    void ShakeTheMenuButtonsPanel(){
        _menuButtonsPanel.gameObject.SetActive(false);
        _menuButtonsPanel.gameObject.SetActive(true);
    }

    

    


}
