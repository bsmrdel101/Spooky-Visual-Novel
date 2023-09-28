using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System.Runtime.CompilerServices;
using System.Data.Common;


public class DialogueManager : MonoBehaviour
{
    [Header("Vital Data")]
    public bool spaceIsDownBool;
    public bool textVariantOneBool, textVatiantTwoBool;
    public bool soundEfectHoldingTippingBool, voiceActorReadyBool=true, typingBool;
    public bool haveOptionsBool, optionsAreDisplayedBool, clearToTypeBool;    
    public Dialogue _curentActiveDialog;
    [SerializeField] private Sprite emptyTransparentSprite;
    
    
    
    
    


    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    public StoryScene startingStoryScene;

    [Header("Dialogue")]
    [SerializeField] private float _textReadDelay = 0.01f;
    public bool stopReadingText = false;
    [SerializeField] private List<Dialogue> _blockedDialogue = new List<Dialogue>();
    [SerializeField] private List<Dialogue> _listOfDialogOptions = new List<Dialogue>();
    int textLenght = 0, textPostition=0;
    string textString = "", textTarget="";
    float timerTyping=0; public float typpingSpeed =0.05f;
    float uiMenuSelectorTimer=0, uiMenuSelectorInterval=0.5f;
    Vector4 v4Visibility= new Vector4(1,1,1,1); 
    float valueW=0;
    float timerSpace=0;

    


    [Header("References")]
    [SerializeField] public Image _actorLeftSprite, _actorRightSprite;
    [SerializeField] private Image _bgImage;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private TextMeshProUGUI _dialogueBodyText;
    [SerializeField] private Transform _dialogueOptionsBox;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private TMP_Text _reputationNumberText;
    public Transform _informativeBlackScreenPanel, _informativeBSButton;
    public TMP_Text _informativeBSText;
    public Image _informativeBSImage;
    public MoveToPosition movabelItemScript;
    public Image movableItemImage;
    public Transform _computerDialogPanel, _computerButtonsPanel;
    public TMP_Text _computerDialogText;
    public List<ComputerDialogButton> _listOfCDButtons;
    [SerializeField] private GameObject _computerDialogButtonPrefab;

    public Transform _diodTyping, _diodOptions, _diodSpaceBar;
    

    [Header("Managers")]
    public MenuPanelManager menuManager;
    public ReputationManager reputationManager;
    public AudioManager audioManager;
    public AppearDissapear appearDissapear;




    private void Start()
    {
        //operating from main menu
        //ChangeStoryScene(startingStoryScene);
        if(_dialogueOptionsBox.gameObject.activeInHierarchy)
            _dialogueOptionsBox.gameObject.SetActive(false);
        OperateDiodes(0, false, true);
    }

    private void Update()
    {
        if(spaceIsDownBool){
            timerSpace += Time.deltaTime;
            if(timerSpace >= 0.75f){
                timerSpace = 0;
                spaceIsDownBool = false;
                OperateDiodes(3, false, false);
            }
        }
        
        // Stop text from reading if player presses spacebar
        if (Input.GetKeyDown(KeyCode.Space)) 
        if (!spaceIsDownBool)
        {
            stopReadingText = true; 
            OperateSpace();
            
        }        
        if (Input.GetKeyDown(KeyCode.Mouse0))
            if(typingBool == true){
                stopReadingText = true;
                OperateSpace();
            }

        if (Input.GetKeyDown(KeyCode.Return)){
            if (!spaceIsDownBool){
                OperateSpace();

                if(optionsAreDisplayedBool){
                    if(_listOfDialogOptions.Count == 1){
                        Debug.Log("Space triger Next on dialog.");
                        OnClickSelectDialogueOption(_listOfDialogOptions[0]);
                    }
                }

                if(menuManager.creditPanelOnBool)
                if(menuManager.creditsManager.typingBool == false)
                if(typingBool == false){
                    Debug.Log("Space triger next button.");
                    menuManager.creditsManager.NextButton();
                }

                if(menuManager.informativeBSPanelBool)
                if(typingBool == false)
                if(_informativeBSButton.gameObject.activeInHierarchy)
                    appearDissapear.OrderForBSPanel(false, true, false);

            }
        } 
        
        
        if(haveOptionsBool && optionsAreDisplayedBool){
            if(_listOfDialogOptions.Count > 0)
            for(int i=0; i< _listOfDialogOptions.Count; i++){
                if(_listOfDialogOptions[i] != null){
                    if(i==0) if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[0]);
                    } 
                    if(i==1) if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[1]);
                    } 
                    if(i==2) if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[2]);
                    } 
                    if(i==3) if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[3]);
                    }
                    if(i==4) if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[4]);
                    }
                    if(i==5) if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[5]);
                    } 
                }
            }

            
        }

        if(!clearToTypeBool){
            uiMenuSelectorTimer += Time.deltaTime;
            if(uiMenuSelectorTimer >= uiMenuSelectorInterval){
                uiMenuSelectorTimer = 0;
                clearToTypeBool = true;
            }
        }

        //if(audioManager._uiPlayer.isPlaying == false)
        if(clearToTypeBool)
        if(soundEfectHoldingTippingBool){
            if(audioManager._sfxPlayer.isPlaying == false){
                soundEfectHoldingTippingBool = false;
                if(textVariantOneBool)
                    StartCoroutine(ReadText(_dialogueBodyText, _curentActiveDialog.BodyText, _curentActiveDialog));
                
                
                audioManager.PlayTyppingSound();
                OperateDiodes(1, true, false);

                if(voiceActorReadyBool) {
                    audioManager.PlayVoiceActorAudio(null, false, false);
                    voiceActorReadyBool = false;
                }
            }            
        }

        if(textVatiantTwoBool && typingBool){
            if(!soundEfectHoldingTippingBool)
                if(clearToTypeBool){
                    timerTyping += Time.deltaTime;
                    if(timerTyping > typpingSpeed){
                        timerTyping = 0;
                        textString = textTarget.Substring(0, textPostition);
                        textString += "<color=#00000000>" + textTarget.Substring(textPostition);
                        if (stopReadingText){
                            typingBool = false;
                            textString = textTarget;      
                        }

                        if(menuManager.computerDialogPanelOnBool){
                            _computerDialogText.text = textString;                            
                        }else if(menuManager.informativeBSPanelBool){
                            _informativeBSText.text = textString; //black screen
                        }else{
                            _dialogueBodyText.text = textString; //normal
                        }

                        textPostition++;
                        if(textPostition >= textLenght) typingBool = false;
                        if(!typingBool){
                            RevealDialougeOptions(_curentActiveDialog);
                            OperateDiodes(1, false, false);
                        }
                    }
                }
        }

        
    

    }// update






    public void StartTheGame(){        
        ChangeStoryScene(startingStoryScene);
        clearToTypeBool = true;
    }

    public void ClearTheGame(){
        _blockedDialogue.Clear();
        audioManager._musicPlayer.Stop();
        ResetDialogueOptions();
        reputationManager._reputation = 0;
        menuManager.gameIsOnBool = false;
        clearToTypeBool = true;
        typingBool = false;
        voiceActorReadyBool = false;
        soundEfectHoldingTippingBool = false;        
        _reputationNumberText.text = "";
        appearDissapear.Clear();
        movabelItemScript.OrderToMove(true, false, false, 0);
        OperateDiodes(0, false, true);
    }


    private void ChangeStoryScene(StoryScene storyScene)
    {
        ActiveStoryScene = storyScene;
        ChangeBackgroundImage(storyScene.BackgroundImage);
        if(storyScene.sceneMusicAudioClip != null)
            audioManager.ChangeBacgroundMusic(storyScene.sceneMusicAudioClip);
        UpdateDialogueBox(storyScene.StartingDialogue);
    }




    private void UpdateDialogueBox(Dialogue dialogue)
    {
        ResetDialogueOptions();
        HandleDialogueBlocking(dialogue.DialogueBlockers);

        _curentActiveDialog = dialogue;
        audioManager.StopActorsAndSfxAudio();

        // Set the speaker and text body values for the dialogue box
        if(dialogue.actorOnLeftBool) {
            ChangeNpcImage(dialogue.speakerOneNewImage, true);            
        }
        else GrayNpcImage(true);
        if(dialogue.actorOnRightBool) ChangeNpcImage(dialogue.speakerTwoNewImage, false);
        else GrayNpcImage(false);

        if(dialogue.actorOnLeftAppearBool) appearDissapear.OrderToAppearDisaper(true, true);
        if(dialogue.actorOnLeftDisapearBool) appearDissapear.OrderToAppearDisaper(true, false);
        if(dialogue.actorOnRightAppearBool) appearDissapear.OrderToAppearDisaper(false, true);
        if(dialogue.actorOnRightDisapearBool) appearDissapear.OrderToAppearDisaper(false, false);
        
        _speakerName.text = dialogue.speakerName;
        _speakerName.color = dialogue.speakerColor;
        _dialogueBodyText.text = "";
        _reputationNumberText.text = reputationManager._reputation.ToString();

        if(dialogue.soundEffectAudioClip != null)
        {            
            soundEfectHoldingTippingBool = true;
            audioManager.PlaySfxAudio(dialogue.soundEffectAudioClip);
        }

        if(dialogue.voiceActorAudioClip != null){
            voiceActorReadyBool = true;
            if(dialogue.actorOnLeftBool)
                audioManager.InsertVoiceAudioClip(dialogue.voiceActorAudioClip, true);
            if(dialogue.actorOnRightBool)
                audioManager.InsertVoiceAudioClip(dialogue.voiceActorAudioClip, false);
        }

        if(dialogue.musicAudioClip != null){
            audioManager.ChangeBacgroundMusic(dialogue.musicAudioClip);
        }

        if(soundEfectHoldingTippingBool == false){
            if(textVariantOneBool)
                StartCoroutine(ReadText(_dialogueBodyText, dialogue.BodyText, dialogue));
            
            if(voiceActorReadyBool) {
                audioManager.PlayVoiceActorAudio(null, false, false);
                voiceActorReadyBool = false;
            }
            OperateDiodes(1, true, false);
            audioManager.PlayTyppingSound();
        }else{}
        if(textVatiantTwoBool){
            ReadTextVariantTwo();
        }
        

        // Renders the dialogue options
        _listOfDialogOptions.Clear();
        _listOfCDButtons.Clear();
        foreach (DialogueOption option in dialogue.DialogueOptions)
        {
            if (!_blockedDialogue.Contains(option.Dialogue))
            {
                GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
                Button button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue));
                _listOfDialogOptions.Add(option.Dialogue);
                haveOptionsBool = true;
                if(dialogue.computerDialogBool){
                    buttonObj = Instantiate(_computerDialogButtonPrefab, _computerButtonsPanel);
                    button = buttonObj.GetComponent<Button>();
                    buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                    button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue));
                    _listOfCDButtons.Add(buttonObj.GetComponent<ComputerDialogButton>());
                }
            }
        }
                
        if(_listOfDialogOptions.Count == 0)
        if(dialogue.DialogueWhenNoneAviable.Dialogue != null){
            GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
            Button button = buttonObj.GetComponent<Button>();
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.DialogueWhenNoneAviable.OptionName;
            button.onClick.AddListener(() => OnClickSelectDialogueOption(dialogue.DialogueWhenNoneAviable.Dialogue));
            _listOfDialogOptions.Add(dialogue.DialogueWhenNoneAviable.Dialogue);
            haveOptionsBool = true;
            if(dialogue.computerDialogBool){
                buttonObj = Instantiate(_computerDialogButtonPrefab, _computerButtonsPanel);
                button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.DialogueWhenNoneAviable.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(dialogue.DialogueWhenNoneAviable.Dialogue));
                _listOfCDButtons.Add(buttonObj.GetComponent<ComputerDialogButton>());
            }
        }//else{Debug.Log("There is no safety line[dialog] to put in when there are no options.");}


        if(dialogue.informativeBSBol){
            appearDissapear.OrderForBSPanel(true, false, false);
            if(dialogue.informativeSuportSprite != null){
                _informativeBSImage.sprite = dialogue.informativeSuportSprite;
            }else{
                if(_informativeBSImage.sprite != emptyTransparentSprite)
                    _informativeBSImage.sprite = emptyTransparentSprite;
            }
        }

        if(dialogue.computerDialogBool){
            menuManager.computerDialogPanelOnBool = true;
            _computerDialogPanel.gameObject.SetActive(true);
            _computerButtonsPanel.gameObject.SetActive(false);
            _computerDialogText.text = "";
        }


        if(dialogue.itemPlaceLeft) movabelItemScript.OrderToMove(true, false, false, 0);
        if(dialogue.itemPlaceRight) movabelItemScript.OrderToMove(false, true, false, 0);
        if(dialogue.itemMoveBool)
            movabelItemScript.OrderToMove(false, false, true, dialogue.itemNewCordinates);
        if(dialogue.ItemNewSprite != null) movableItemImage.sprite = dialogue.ItemNewSprite;


        if(dialogue.redirectionOnStoryScene != null){
            ChangeStoryScene(dialogue.redirectionOnStoryScene);
        }

        if(dialogue.endingTheStoryBool){
            Debug.Log("ending scene");
            menuManager.creditsManager.endingNumber = _curentActiveDialog.endigNumber;
            ClearTheGame();
            menuManager.creditsManager.OpenStartEndCreditsPanel(false, true);
        }
    }





    // Deletes all buttons inside dialogue options box
    private void ResetDialogueOptions()
    {
        if (_dialogueOptionsBox.childCount != 0)
        foreach (Button button in _dialogueOptionsBox.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
        haveOptionsBool = false;
        if (_computerButtonsPanel.childCount != 0) 
        foreach (Button button in _computerButtonsPanel.GetComponentsInChildren<Button>()){
            Destroy(button.gameObject);
        }
    }

    public void ChangeNpcImage(Sprite image, bool itsLeftBool)
    {
        if(itsLeftBool){
            if(image != null){
                _actorLeftSprite.sprite = image;
                if(_actorLeftSprite.color != Color.white) {
                    valueW = _actorLeftSprite.color.a;
                    v4Visibility = (Vector4)Color.white;
                    v4Visibility.w = valueW;
                    _actorLeftSprite.color = v4Visibility;
                }
            }
            else
            _actorLeftSprite.sprite = emptyTransparentSprite;  
        }else{
            if(image != null){
                _actorRightSprite.sprite = image; 
                if(_actorRightSprite.color != Color.white) {
                    valueW = _actorRightSprite.color.a;
                    v4Visibility = (Vector4)Color.white;
                    v4Visibility.w = valueW;
                    _actorRightSprite.color = v4Visibility;
                }
            }            
            else
            _actorRightSprite.sprite = emptyTransparentSprite;
        }
    }

    private void GrayNpcImage(bool itsLeftBool){
        if(itsLeftBool){
            if(_actorLeftSprite.color != Color.grey) {
                    valueW = _actorLeftSprite.color.a;
                    v4Visibility = (Vector4)Color.grey;
                    v4Visibility.w = valueW;
                    _actorLeftSprite.color = v4Visibility;
                }           
        }else{
            if(_actorRightSprite.color != Color.grey) {
                    valueW = _actorRightSprite.color.a;
                    v4Visibility = (Vector4)Color.grey;
                    v4Visibility.w = valueW;
                    _actorRightSprite.color = v4Visibility;
                } 
        }
    }

   

    private void ChangeBackgroundImage(Sprite bgImage)
    {
        if(bgImage != null)
        _bgImage.sprite = bgImage;
        else
        _bgImage.sprite = emptyTransparentSprite;
    }

    


    // Creates the typing effect for text
    private IEnumerator ReadText(TextMeshProUGUI element, string textContent, Dialogue dialogue)
	{
        stopReadingText = false;
        typingBool = true;
        
        audioManager.PlayTyppingSound();

        if(textVariantOneBool){
            element.text = "";
            foreach (char c in textContent) 
            {
                element.text += c;
                if (stopReadingText)
                {
                    element.text = textContent;
                    typingBool = false;
                    RevealDialougeOptions(dialogue);
                    yield break;
                }
                yield return new WaitForSeconds(_textReadDelay);
            }    
        }

        //obsolete
        if(textVatiantTwoBool){
            textLenght = textContent.Length;
            for(int position=0; position < textLenght; position++){
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
                    position = textLenght -1;
                    typingBool = false;
                    element.text = textContent;                    
                    RevealDialougeOptions(dialogue);
                    yield break;
                }
                if(position >= textLenght ) typingBool = false;
                yield return new WaitForSeconds(_textReadDelay);
            }
        }
        

        typingBool = false;
        // Makes the dialogue options visible if they exist
        RevealDialougeOptions(dialogue);
	}


    private void ReadTextVariantTwo(){
        stopReadingText = false;
        typingBool = true;        
        textLenght = _curentActiveDialog.BodyText.Length +1;
        textTarget = _curentActiveDialog.BodyText +" ";
        textPostition = 0;
    }

    private void RevealDialougeOptions(Dialogue dialogue)
    {
        if (dialogue.DialogueOptions.Count > 0){
        if (_dialogueOptionsBox.gameObject.activeInHierarchy == false)
            _dialogueOptionsBox.gameObject.SetActive(true);

            _computerButtonsPanel.gameObject.SetActive(true);
            OperateDiodes(2, true, false);
        }
        optionsAreDisplayedBool = true;
        OperateDiodes(1, false, false);
    }

    public void OnClickSelectDialogueOption(Dialogue dialogue)
    {
        audioManager.PlayUiAudio(audioManager.selectDialogAC);
        _dialogueOptionsBox.gameObject.SetActive(false);
        if(menuManager.computerDialogPanelOnBool){
            menuManager.computerDialogPanelOnBool = false;
            _computerDialogText.text = "";
            _computerDialogPanel.gameObject.SetActive(false);
        }
        optionsAreDisplayedBool = false;
        clearToTypeBool = false;
        OperateDiodes(2, false, false);
        ReputationManager.IncreaseReputationAction(dialogue.reputationIncrease);
        ReputationManager.DecreaseReputationAction(dialogue.reputationDecrease);        
        UpdateDialogueBox(dialogue);
    }

    private void HandleDialogueBlocking(List<Dialogue> dialogueList)
    {
        foreach (Dialogue dialogue in dialogueList)
        {
            _blockedDialogue.Add(dialogue);
        }
    }


    public void StopTheMusic(){audioManager._musicPlayer.Stop();}

    public void SetReputation(int number, string addon){
        _reputationNumberText.text = number.ToString();
        if(addon != "") _reputationNumberText.text += addon; 

        if(number == 0) if(_reputationNumberText.color !=Color.white)
            _reputationNumberText.color =Color.white;
        if(number > 0) if(_reputationNumberText.color !=Color.green)
            _reputationNumberText.color =Color.green;
        if(number < 0) if(_reputationNumberText.color !=Color.magenta)
            _reputationNumberText.color =Color.magenta;
    }


    public void OrderFromInfoBlackScreen(){
        UpdateDialogueBox(_curentActiveDialog.informativeLeadsToDialogue);
    }

    

    void OperateDiodes(int diodeNumber, bool option, bool clearAllBool){
        if(clearAllBool){
            _diodTyping.gameObject.SetActive(false);
            _diodOptions.gameObject.SetActive(false);
            _diodSpaceBar.gameObject.SetActive(false);
        }else{
            if(diodeNumber == 1)
            if(!option) _diodTyping.gameObject.SetActive(false);
            else _diodTyping.gameObject.SetActive(true);

            if(diodeNumber == 2)
            if(!option) _diodOptions.gameObject.SetActive(false);
            else _diodOptions.gameObject.SetActive(true);

            if(diodeNumber == 3)
            if(!option) _diodSpaceBar.gameObject.SetActive(false);
            else _diodSpaceBar.gameObject.SetActive(true);
        }
    }

    void OperateSpace(){
        spaceIsDownBool = true;
        OperateDiodes(3, true, false);
    }

}
