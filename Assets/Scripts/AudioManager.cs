using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool secondarySfxPreparedBool, reputationSfxPreparedBool, delayedSfxPreparedBool;
    public bool typingBool;
    public bool muteAllBool;

    float timerDelayedSfx=0, intervalDelayedSfx=0;

    [Header("Players")]
    public AudioSource _musicPlayer;
    public AudioSource _mainMenuMusicPlayer, 
        _sfxPlayer, _sfxSecondaryPlayer, _sfxDelayedPlayer,
        _reputationSfxPlayer, _uiPlayer, _typpingSoundPlayer, 
        _voiceActorLeftPlayer, _voiceActorRightPlayer, _voiceActorMiddlePlayer;

    [Header("clips")]
    public AudioClip mainMenuMusicAC;
    public AudioClip  startScreenAC, mainActivityAC,
        creditsACBadEnding, reputationUpAC, reputationDownAC;
    public AudioClip uiButtonPositiveAC, uiButtonNegativeAC;
    public AudioClip selectDialogAC;
    public AudioClip[] typingACArray; int typingCurrentInteger=0;
    public AudioClip[] dialougueBeepACArray, PCShutingDownACArray, selectMenuACArray;
    public AudioClip norvalVoiceAudioClip;
    public AudioClip testVoiceAudioClip;
    

    


    [Header("Managers")]
    public MenuPanelManager menuManager;
    public DialogueManager dialogueManager;

    void Update()
    {
        if(secondarySfxPreparedBool){
            if(!dialogueManager.soundEfectHoldingTippingBool)
            if(!dialogueManager.typingBool){
                if(!muteAllBool) _sfxSecondaryPlayer.Play();
                secondarySfxPreparedBool = false;
            }
        }

        if(reputationSfxPreparedBool){
            if(!secondarySfxPreparedBool)
            if(!dialogueManager.soundEfectHoldingTippingBool)
            if(!dialogueManager.typingBool){
                if(!muteAllBool) _reputationSfxPlayer.Play();
                reputationSfxPreparedBool = false;
            }
        }

        if(typingBool){
            if(!dialogueManager.typingBool){
                _typpingSoundPlayer.Stop();
                typingBool = false;
            }
        }

        if(delayedSfxPreparedBool){
            timerDelayedSfx += Time.deltaTime;
            if(timerDelayedSfx > intervalDelayedSfx){
                timerDelayedSfx = 0;
                delayedSfxPreparedBool = false;
                if(!muteAllBool) _sfxDelayedPlayer.Play();
            }
        }

    }

    public void ChangeBacgroundMusic(AudioClip newAudio){
        if(newAudio != _musicPlayer.clip){
            if(_musicPlayer.isPlaying) {_musicPlayer.Stop();}
                _musicPlayer.clip = newAudio;
            if(_mainMenuMusicPlayer.isPlaying) _mainMenuMusicPlayer.Pause();
        }

        if(!muteAllBool) 
        if(!_musicPlayer.isPlaying)
        if(!menuManager.muteMusicBool) _musicPlayer.Play();
    }

    public void MainMenuMusicDominator(bool deactivateBool, bool startScreeBool, bool menuBool ){
        if(deactivateBool){
            _mainMenuMusicPlayer.Pause();
            if(!muteAllBool) if(_musicPlayer.clip != null) _musicPlayer.Play();
        }
        if(startScreeBool || menuBool) if(_musicPlayer.isPlaying) _musicPlayer.Stop();
        if(startScreeBool){            
            _mainMenuMusicPlayer.clip = startScreenAC;
            if(!muteAllBool) _mainMenuMusicPlayer.Play();
        }
        if(menuBool){
            if(_mainMenuMusicPlayer.clip != mainMenuMusicAC) _mainMenuMusicPlayer.clip = mainMenuMusicAC;
            if(!muteAllBool) _mainMenuMusicPlayer.Play();
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
        if(delayedSfxPreparedBool) delayedSfxPreparedBool = false;
        if(_sfxDelayedPlayer.isPlaying == true) _sfxDelayedPlayer.Stop();
        //Debug.Log("Stopping sfx audio.");
    }

    public void StopMusicAudio(){
        _musicPlayer.Stop();
        _mainMenuMusicPlayer.Stop();
    }

    public void PlayUiAudio(AudioClip newAdudio){
        _uiPlayer.clip = newAdudio;
        if(!muteAllBool) _uiPlayer.Play();
    }

    public void PlaySfxAudio(AudioClip newAudio){
        _sfxPlayer.clip = newAudio;
        if(!muteAllBool) 
        if(newAudio != null)
            _sfxPlayer.Play();
    }

    public void PlayDelayedSfx(AudioClip newAudio, string theString){
        _sfxDelayedPlayer.clip = newAudio;
        intervalDelayedSfx = (float)theString.Length * dialogueManager.typpingSpeed;
        timerDelayedSfx = 0;
        delayedSfxPreparedBool = true;
    }

    


    public void PlayNormalVoiceActor(int side){
        if(side == -1){
            _voiceActorLeftPlayer.clip = norvalVoiceAudioClip;
            _voiceActorLeftPlayer.Play();
        }
        if(side == 0){
            _voiceActorMiddlePlayer.clip = norvalVoiceAudioClip;
            _voiceActorMiddlePlayer.Play();
        }
        if(side == 1){
            _voiceActorRightPlayer.clip = norvalVoiceAudioClip;
            _voiceActorRightPlayer.Play();
        }
    }

    public void PlayTestVoiceActor(){
        _voiceActorMiddlePlayer.clip = testVoiceAudioClip;
        _voiceActorMiddlePlayer.Play();
    }

    public void JustPlayVoiceActor(AudioClip theClip){
        _voiceActorMiddlePlayer.clip = theClip;
        _voiceActorMiddlePlayer.Play();
    }


    public void InsertVoiceAudioClip(AudioClip newAudio, bool itsLeftBool, bool itsRightBool, bool itsMiddleBool){
        if(itsLeftBool){
            _voiceActorLeftPlayer.clip = newAudio;
        }

        if(itsMiddleBool){
            _voiceActorRightPlayer.clip = newAudio;
        }

        if(itsMiddleBool){
            _voiceActorMiddlePlayer.clip = newAudio;
        }        
    }

    public void PlayMenuButtonClick(bool option){
        if(option){ _uiPlayer.clip = uiButtonPositiveAC;
        }else{_uiPlayer.clip = uiButtonNegativeAC;}
        if(!muteAllBool) _uiPlayer.Play(); 
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
        Debug.Log("Reputation sfx played!");
    }

    public void PlayTyppingSound(){
        if(_typpingSoundPlayer.isPlaying) _typpingSoundPlayer.Stop();
        _typpingSoundPlayer.clip = typingACArray[typingCurrentInteger];
        if(!muteAllBool) _typpingSoundPlayer.Play(); //.PlayDelayed(0.5f);
        typingBool = true;
        typingCurrentInteger++;
        if(typingCurrentInteger >= typingACArray.Length)
            typingCurrentInteger = 0;
    }


    public void MuteButton(){
        if(muteAllBool){muteAllBool = false;}else{muteAllBool = true;}

        if(muteAllBool){
            StopActorsAndSfxAudio();
            StopMusicAudio();
        }

        if(!muteAllBool){
            _musicPlayer.Play();
        }

        menuManager.MuteMusicButton(muteAllBool);
    }

    
}
