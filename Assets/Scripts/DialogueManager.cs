using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [Header("Vital Data")]
    public bool textVariantOneBool;
    public bool textVatiantTwoBool;
    public bool soundEfectHoldingTippingBool, voiceActorReadyBool=true;
    public Dialogue _curentActiveDialog;
    [SerializeField] private Sprite emptyTransparentSprite;
    
    
    
    
    


    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    public StoryScene startingStoryScene;

    [Header("Dialogue")]
    [SerializeField] private float _textReadDelay = 0.01f;
    private bool stopReadingText = false;
    [SerializeField] private List<Dialogue> _blockedDialogue = new List<Dialogue>();
    [SerializeField] private List<Dialogue> _listOfDialogOptions = new List<Dialogue>();
    int textLenght = 0;
    string textString = "";

    [Header("References")]
    [SerializeField] private Image _speakerSprite;
    [SerializeField] private Image _bgImage;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private TextMeshProUGUI _dialogueBodyText;
    [SerializeField] private Transform _dialogueOptionsBox;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private AudioSource _musicPlayer, _sfxPlayer, _voiceActorPlayer, _uiPlayer;

    public MenuPanelManager menuManager;
    public ReputationManager reputationManager;

    private void Start()
    {
        //ChangeStoryScene(startingStoryScene);
    }

    private void Update()
    {
        // Stop text from reading if player presses spacebar
        if (Input.GetKeyDown(KeyCode.Space)) stopReadingText = true;

        if(soundEfectHoldingTippingBool){
            if(_sfxPlayer.isPlaying == false){
                soundEfectHoldingTippingBool = false;
                StartCoroutine(ReadText(_dialogueBodyText, _curentActiveDialog.BodyText, _curentActiveDialog));
                if(voiceActorReadyBool) {
                    PlayVoiceActorAudio(null, false);
                    voiceActorReadyBool = false;
                }
            }            
        }
    }


    public void StartTheGame(){        
        ChangeStoryScene(startingStoryScene);
    }

    public void ClearTheGame(){
        _blockedDialogue.Clear();
        _musicPlayer.Stop();
        ResetDialogueOptions();
        reputationManager._reputation = 0;
        menuManager.gameIsOnBool = false;
    }


    private void ChangeStoryScene(StoryScene storyScene)
    {
        ActiveStoryScene = storyScene;
        ChangeBackgroundImage(storyScene.BackgroundImage);
        if(storyScene.sceneMusicAudioClip != null)
            ChangeBacgroundMusic(storyScene.sceneMusicAudioClip);
        UpdateDialogueBox(storyScene.StartingDialogue);
    }




    private void UpdateDialogueBox(Dialogue dialogue)
    {
        ResetDialogueOptions();
        HandleDialogueBlocking(dialogue.DialogueBlockers);

        _curentActiveDialog = dialogue;
        if(_voiceActorPlayer.isPlaying == true){_voiceActorPlayer.Stop();}
        if(_sfxPlayer.isPlaying == true){_sfxPlayer.Stop();}

        // Set the speaker and text body values for the dialogue box
        ChangeNpcImage(dialogue.SpeakerImage);
        _speakerName.text = dialogue.Speaker;
        _speakerName.color = dialogue.SpeakerColor;

        if(dialogue.soundEffectAudioClip != null)
        {            
            soundEfectHoldingTippingBool = true;
            PlaySfxAudio(dialogue.soundEffectAudioClip);
        }

        if(dialogue.voiceActorAudioClip != null){
            voiceActorReadyBool = true;
            _voiceActorPlayer.clip = dialogue.voiceActorAudioClip;
        }

        if(dialogue.musicAudioClip != null){
            ChangeBacgroundMusic(dialogue.musicAudioClip);
        }

        if(soundEfectHoldingTippingBool == false){
            StartCoroutine(ReadText(_dialogueBodyText, dialogue.BodyText, dialogue));
            if(voiceActorReadyBool) {
                PlayVoiceActorAudio(null, false);
                voiceActorReadyBool = false;
            }
        }else{
            _dialogueBodyText.text = "";
        }
        
        
        // Renders the dialogue options
        _listOfDialogOptions.Clear();
        foreach (DialogueOption option in dialogue.DialogueOptions)
        {
            if (!_blockedDialogue.Contains(option.Dialogue))
            {
                GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
                Button button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue));
                _listOfDialogOptions.Add(option.Dialogue);
            }
        }


        if(dialogue.redirectionOnStoryScene != null){
            ChangeStoryScene(dialogue.redirectionOnStoryScene);
        }

        if(dialogue.endingTheStoryBool){
            Debug.Log("ending scene");
            ClearTheGame();
            menuManager.creditsManager.OpenStartEndCreditsPanel();
        }
    }





    // Deletes all buttons inside dialogue options box
    private void ResetDialogueOptions()
    {
        if (_dialogueOptionsBox.childCount == 0) return;
        foreach (Button button in _dialogueOptionsBox.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
    }

    private void ChangeNpcImage(Sprite image)
    {
        if(image != null)
        _speakerSprite.sprite = image;
        else
        _speakerSprite.sprite = emptyTransparentSprite;
    }

    private void ChangeBackgroundImage(Sprite bgImage)
    {
        if(bgImage != null)
        _bgImage.sprite = bgImage;
        else
        _bgImage.sprite = emptyTransparentSprite;
    }

    public void ChangeBacgroundMusic(AudioClip newAudio){
        if(_musicPlayer.isPlaying) {_musicPlayer.Stop();}
            _musicPlayer.clip = newAudio;
            _musicPlayer.Play();
    }

    public void PlayUiAudio(AudioClip newAdudio){
        _uiPlayer.clip = newAdudio;
        _uiPlayer.Play();
    }

    public void PlaySfxAudio(AudioClip newAudio){
        _sfxPlayer.clip = newAudio;
        _sfxPlayer.Play();
    }

    public void PlayVoiceActorAudio(AudioClip newAudio, bool useThisClipBool){
        if(useThisClipBool) _voiceActorPlayer.clip = newAudio;
        _voiceActorPlayer.Play();
    }


    // Creates the typing effect for text
    private IEnumerator ReadText(TextMeshProUGUI element, string textContent, Dialogue dialogue)
	{
        stopReadingText = false;

        if(textVariantOneBool){
            element.text = "";
            foreach (char c in textContent) 
            {
                element.text += c;
                if (stopReadingText)
                {
                    element.text = textContent;
                    RevealDialougeOptions(dialogue);
                    yield break;
                }
                yield return new WaitForSeconds(_textReadDelay);
            }    
        }

        if(textVatiantTwoBool){
            textLenght = textContent.Length;
            for(int position=0; position <= textLenght; position++){
                if(position == 0) {
                    element.text = "";
                }else
                {
                    textString = textContent.Substring(0, position);
                    textString += "<color=#00000000>" + textContent.Substring(position);
                    element.text = textString;
                }
                if (stopReadingText)
                {
                    position = textLenght;
                    element.text = textContent;
                    RevealDialougeOptions(dialogue);
                    yield break;
                }
                yield return new WaitForSeconds(_textReadDelay);
            }
        }
        

        // Makes the dialogue options visible if they exist
        RevealDialougeOptions(dialogue);
	}

    private void RevealDialougeOptions(Dialogue dialogue)
    {
        if (dialogue.DialogueOptions.Count > 0)
        if (_dialogueOptionsBox.gameObject.activeInHierarchy == false)
            _dialogueOptionsBox.gameObject.SetActive(true);
    }

    private void OnClickSelectDialogueOption(Dialogue dialogue)
    {
        _dialogueOptionsBox.gameObject.SetActive(false);
        ReputationManager.IncreaseReputationAction(dialogue.ReputationIncrease);
        ReputationManager.DecreaseReputationAction(dialogue.ReputationDecrease);
        UpdateDialogueBox(dialogue);
    }

    private void HandleDialogueBlocking(List<Dialogue> dialogueList)
    {
        foreach (Dialogue dialogue in dialogueList)
        {
            _blockedDialogue.Add(dialogue);
        }
    }


    public void StopTheMusic(){_musicPlayer.Stop();}
}
