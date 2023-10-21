using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public MenuPanelManager menuPanelManager;
    public AudioManager audioManager;

    public TMP_Text musicVolumeText, sfxVolumeText, uiVolumeText, voiceVolumeText;
    public TMP_Text typingSpeedText, typingSpeedSampleText;
    float valueTechnical, valueForText;
    int intValueForText;
    string theString; public string stingSample;


    public void OpenMe(){
        valueTechnical = audioManager._musicPlayer.volume;
        intValueForText = (int)(valueTechnical * 100f);
        musicVolumeText.text = intValueForText +"%";

        valueTechnical = audioManager._sfxPlayer.volume;
        intValueForText = (int)(valueTechnical * 100f);
        sfxVolumeText.text = intValueForText +"%";

        valueTechnical = audioManager._uiPlayer.volume;
        intValueForText = (int)(valueTechnical * 100f);
        uiVolumeText.text = intValueForText +"%";

        valueTechnical = audioManager._voiceActorLeftPlayer.volume;
        intValueForText = (int)(valueTechnical * 100f);
        voiceVolumeText.text = intValueForText +"%";

        valueTechnical = menuPanelManager.dialogueManager.typpingSpeed;
        typingSpeedText.text = valueTechnical + " W/s";
    }

    public void MusicVolume(bool option){
        valueTechnical = audioManager._musicPlayer.volume;
        if(option){if(valueTechnical < 1) valueTechnical += 0.1f;}
        else{if(valueTechnical > 0) valueTechnical -= 0.1f;} 
        intValueForText = (int)(valueTechnical * 100f);
        musicVolumeText.text = intValueForText +"%";
        audioManager._musicPlayer.volume = valueTechnical;
        audioManager._mainMenuMusicPlayer.volume = valueTechnical;
    }

    public void SfxVolume(bool option){
        valueTechnical = audioManager._sfxPlayer.volume;
        if(option){if(valueTechnical < 1) valueTechnical += 0.1f;}
        else{if(valueTechnical > 0) valueTechnical -= 0.1f;} 
        intValueForText = (int)(valueTechnical * 100f);
        sfxVolumeText.text = intValueForText +"%";
        audioManager._sfxPlayer.volume = valueTechnical;
        audioManager._sfxSecondaryPlayer.volume = valueTechnical;
        audioManager._sfxDelayedPlayer.volume = valueTechnical;
        audioManager._typpingSoundPlayer.volume = valueTechnical;
        audioManager.PlaySfxAudio(audioManager.selectDialogAC);
    }

    public void UiVolume(bool option){
        valueTechnical = audioManager._uiPlayer.volume;
        if(option){if(valueTechnical < 1) valueTechnical += 0.1f;}
        else{if(valueTechnical > 0) valueTechnical -= 0.1f;} 
        intValueForText = (int)(valueTechnical * 100f);
        uiVolumeText.text = intValueForText +"%";
        audioManager._uiPlayer.volume = valueTechnical;
        audioManager.PlayUiAudio(audioManager.selectDialogAC);
    }

    public void VoiceVolume(bool option){
        valueTechnical = audioManager._voiceActorLeftPlayer.volume;
        if(option){if(valueTechnical < 1) valueTechnical += 0.1f;}
        else{if(valueTechnical > 0) valueTechnical -= 0.1f;} 
        intValueForText = (int)(valueTechnical * 100f);
        voiceVolumeText.text = intValueForText +"%";
        audioManager._voiceActorLeftPlayer.volume = valueTechnical;
        audioManager._voiceActorMiddlePlayer.volume = valueTechnical;
        audioManager._voiceActorRightPlayer.volume = valueTechnical;
        //audioManager.PlayVoiceActorAudio(audioManager.selectDialogAC, true, true);
        audioManager.PlayTestVoiceActor();
    }

    public void TyppingSpeed(bool option){
        valueTechnical = menuPanelManager.dialogueManager.typpingSpeed;
        if(option){
            if(valueTechnical < 0.10f) valueTechnical += 0.01f;
            else valueTechnical = 0.1f;
            }
        else{
            if(valueTechnical > 0.01f) valueTechnical -= 0.01f;
            else valueTechnical = 0.01f;
            }
        intValueForText = (int) (valueTechnical * 100f);
        typingSpeedText.text = "0.0"+ intValueForText + " W/s";
        menuPanelManager.dialogueManager.typpingSpeed = valueTechnical;
        menuPanelManager.creditsManager.SetTypping(stingSample);
    }

    
}
