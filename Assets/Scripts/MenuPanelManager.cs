using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuPanelManager : MonoBehaviour
{
    [Header("Variables")]
    public bool dontOpenCoverPageBool;
    public bool menuIsOnBool; 
    public bool coverPageOnBool, settingPanleOnBool, creditsPanelOnBool;
    public bool gameIsOnBool, creditPanelOnBool, informativeBSPanelBool, 
        computerDialogPanelOnBool;
    public bool muteMusicBool;

    [Header("Version display")]  
    public string versionString;

    [Header("references")]    
    public Transform _menuPanel;
    public Transform _menuButtonsPanel, _coverPagePanel, _creditsPanel;
    public Transform _settingsPanel;
    public TMP_Text[] versionDisplayTextsArray;

    [Header("Mute Music")]
    public TMP_Text mmbText;
    public Image mmbImageUnMute, mmbImageMute;
    
    
    
    [Header("Managers")]
    public DialogueManager dialogueManager;
    public CreditsManager creditsManager;
    public AudioManager audioManager;
    public SettingsManager settingsManager;



    private void Start() {
        if(!dontOpenCoverPageBool) OrderToOpenCloseCoverPage(true);
        if(versionDisplayTextsArray.Length > 0){
            for (int i = 0; i < versionDisplayTextsArray.Length; i++)
            {
                versionDisplayTextsArray[i].text = versionString;
            }
        }     
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)){
            if(coverPageOnBool){ OrderToOpenCloseCoverPage(false);}
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            if(coverPageOnBool){ OrderToOpenCloseCoverPage(false);}
            //if()
        }
    }


    public void OrderToOpenCloseCoverPage(bool option){
        if(option){
            _coverPagePanel.gameObject.SetActive(true);
            coverPageOnBool = true;
            audioManager.MainMenuMusicDominator(false, true, false);
        }else{
            _coverPagePanel.gameObject.SetActive(false);
            coverPageOnBool = false;
            //audioManager.MainMenuMusicDominator(true, false, true);
            //OrderToOperateMainMenu();
            OrderToStartTheNewGame();
        }
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
            dialogueManager.ChangeNpcImage(true, false, false, null, 0);
            dialogueManager.ChangeNpcImage(false, true, false, null, 0);
            dialogueManager.ChangeNpcImage(false, false, true, null, 0);
            dialogueManager.movableItemImage.sprite = dialogueManager.emptyTransparentSprite;
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

    public void OrderToOpencloseSettingsPanel(){
        if(!settingPanleOnBool ){
            settingPanleOnBool = true;
            _settingsPanel.gameObject.SetActive(true);
            settingsManager.OpenMe();
        }else{
            settingPanleOnBool = false;
            _settingsPanel.gameObject.SetActive(false);
        }
    }


    public void OrderToOpenCloseCreditsPanel(bool option){
        if(option){
            creditPanelOnBool = true;
            _creditsPanel.gameObject.SetActive(true);
        }else{
            creditPanelOnBool = false;
            _creditsPanel.gameObject.SetActive(false);
        }
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

    
    public void MuteMusicButton(bool option){
        if(option){
            mmbText.text = "Muted";
            mmbImageMute.gameObject.SetActive(true);
            mmbImageUnMute.gameObject.SetActive(false);
        }else{
            mmbText.text = "Laud";
            mmbImageMute.gameObject.SetActive(false);
            mmbImageUnMute.gameObject.SetActive(true);
        }
    }

    


}
