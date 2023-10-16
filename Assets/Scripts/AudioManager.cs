using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool secondarySfxPreparedBool, reputationSfxPreparedBool;
    public bool typingBool;

    [Header("Players")]
    public AudioSource _musicPlayer;
    public AudioSource _mainMenuMusicPlayer, 
        _sfxPlayer, _sfxSecondaryPlayer, _reputationSfxPlayer, _uiPlayer, _typpingSoundPlayer, 
        _voiceActorLeftPlayer, _voiceActorRightPlayer, _voiceActorMiddlePlayer;

    [Header("clips")]
    public AudioClip mainMenuMusicAC;
    public AudioClip  startScreenAC, mainActivityAC,
        creditsACBadEnding, reputationUpAC, reputationDownAC;
    public AudioClip uiButtonPositiveAC, uiButtonNegativeAC;
    public AudioClip selectDialogAC;
    public AudioClip[] typingACArray; int typingCurrentInteger=0;
    public AudioClip[] dialougueBeepACArray, PCShutingDownACArray, selectMenuACArray;
    


    [Header("Managers")]
    public MenuPanelManager menuManager;
    public DialogueManager dialogueManager;

    void Update()
    {
        if(secondarySfxPreparedBool){
            if(!dialogueManager.soundEfectHoldingTippingBool)
            if(!dialogueManager.typingBool){
                _sfxSecondaryPlayer.Play();
                secondarySfxPreparedBool = false;
            }
        }

        if(reputationSfxPreparedBool){
            if(!secondarySfxPreparedBool)
            if(!dialogueManager.soundEfectHoldingTippingBool)
            if(!dialogueManager.typingBool){
                _reputationSfxPlayer.Play();
                reputationSfxPreparedBool = false;
            }
        }

        if(typingBool){
            if(!dialogueManager.typingBool){
                _typpingSoundPlayer.Stop();
                typingBool = false;
            }
        }
    }

    public void ChangeBacgroundMusic(AudioClip newAudio){
        if(newAudio != _musicPlayer.clip){
            if(_musicPlayer.isPlaying) {_musicPlayer.Stop();}
                _musicPlayer.clip = newAudio;
            if(_mainMenuMusicPlayer.isPlaying) _mainMenuMusicPlayer.Pause();
        }

        if(!_musicPlayer.isPlaying)
        if(!menuManager.muteMusicBool) _musicPlayer.Play();
    }

    public void MainMenuMusicDominator(bool deactivateBool, bool startScreeBool, bool menuBool ){
        if(deactivateBool){
            _mainMenuMusicPlayer.Pause();
            if(_musicPlayer.clip != null) _musicPlayer.Play();
        }
        if(startScreeBool || menuBool) if(_musicPlayer.isPlaying) _musicPlayer.Stop();
        if(startScreeBool){            
            _mainMenuMusicPlayer.clip = startScreenAC;
            _mainMenuMusicPlayer.Play();
        }
        if(menuBool){
            if(_mainMenuMusicPlayer.clip != mainMenuMusicAC) _mainMenuMusicPlayer.clip = mainMenuMusicAC;
            _mainMenuMusicPlayer.Play();
        }
    }

    public void StopActorsAndSfxAudio(){
        if(_voiceActorLeftPlayer.isPlaying == true){_voiceActorLeftPlayer.Stop();}
        if(_voiceActorRightPlayer.isPlaying == true){_voiceActorRightPlayer.Stop();}
        if(_voiceActorMiddlePlayer.isPlaying == true){_voiceActorMiddlePlayer.Stop();}
        if(_sfxPlayer.isPlaying == true){_sfxPlayer.Stop();}
        if(_sfxSecondaryPlayer.isPlaying == true){_sfxSecondaryPlayer.Stop();}
        //I let reputation to slide even if is order to stop
        if(_typpingSoundPlayer.isPlaying == true) _typpingSoundPlayer.Stop();
        if(secondarySfxPreparedBool) secondarySfxPreparedBool = false;
        //Debug.Log("Stopping sfx audio.");
    }

    public void PlayUiAudio(AudioClip newAdudio){
        _uiPlayer.clip = newAdudio;
        _uiPlayer.Play();
    }

    public void PlaySfxAudio(AudioClip newAudio){
        _sfxPlayer.clip = newAudio;
        if(newAudio != null)
            _sfxPlayer.Play();
    }

    public void PlayVoiceActorAudio(AudioClip newAudio, bool useThisClipBool, bool nonDialogBool){
        if(nonDialogBool){
            _voiceActorMiddlePlayer.clip = newAudio;
            _voiceActorMiddlePlayer.Play();
        }else{
            if(dialogueManager._curentActiveDialog.characterOnLeftWhiteBool){
                if(useThisClipBool) _voiceActorLeftPlayer.clip = newAudio;
                _voiceActorLeftPlayer.Play();    
            }
            if(dialogueManager._curentActiveDialog.characterOnRightWhiteBool){
                if(useThisClipBool) _voiceActorRightPlayer.clip = newAudio;
                _voiceActorRightPlayer.Play();    
            }    
            if(dialogueManager._curentActiveDialog.characterOnMiddleWhiteBool){
                if(useThisClipBool) _voiceActorMiddlePlayer.clip = newAudio;
                _voiceActorMiddlePlayer.Play();    
            }   
        }
    }

    public void InsertVoiceAudioClip(AudioClip newAudio, bool itsLeftBool, bool itsRightBool, bool itsMiddleBool){
        if(itsLeftBool){
            //_actorMiddleSpritePlay();
            _voiceActorLeftPlayer.clip = newAudio;
        }
        //warning what the heck i have here?
        if(itsMiddleBool){
            //_voiceActorRightPlayer.Play();
            _voiceActorRightPlayer.clip = newAudio;
        }

        if(itsMiddleBool){
            _voiceActorMiddlePlayer.clip = newAudio;
        }
        
    }

    public void PlayMenuButtonClick(bool option){
        if(option){ _uiPlayer.clip = uiButtonPositiveAC;
        }else{_uiPlayer.clip = uiButtonNegativeAC;}
        _uiPlayer.Play(); 
    }

    public void PlaySFXatEnd(bool option){
        //after finishing typing
        if(option){_sfxSecondaryPlayer.clip = reputationUpAC;
        }else{_sfxSecondaryPlayer.clip = reputationDownAC;}
        secondarySfxPreparedBool = true;
    }

    public void PlayReputationSound(bool option){
        //after finishing typing and after the second sfx
        if(option){_reputationSfxPlayer.clip = reputationUpAC;
        }else{_reputationSfxPlayer.clip = reputationDownAC;}
        reputationSfxPreparedBool = true;
    }

    public void PlayTyppingSound(){
        if(_typpingSoundPlayer.isPlaying) _typpingSoundPlayer.Stop();
        _typpingSoundPlayer.clip = typingACArray[typingCurrentInteger];
        _typpingSoundPlayer.Play(); //.PlayDelayed(0.5f);
        typingBool = true;
        typingCurrentInteger++;
        if(typingCurrentInteger >= typingACArray.Length)
            typingCurrentInteger = 0;
    }

    
}
