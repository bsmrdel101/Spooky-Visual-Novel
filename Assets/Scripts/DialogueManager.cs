using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{
    [Header("Vital Data")]
    public bool spaceIsDownBool; 
    public bool technicalStepsPanelOpenBool;
    public bool enterIsDownBool, mouseIsDownBool;
    
    public bool soundEfectHoldingTippingBool, voiceActorReadyBool=true
        , typingBool, bodyTextDisplayedBool;
    public bool haveOptionsBool, optionsAreDisplayedBool, clearToTypeBool, normalDialogBool;
    public bool nextDialogButtonOnBool; 
    public bool reputationEnabledBool;  
    public Sprite emptyTransparentSprite;
    public bool preparedForTheNextDialogueBool;
    private Dialogue preparedForTheNextDialogueValue=null;
    public bool waitInputBool, waitProtectTippingBool;
    public bool stepBackBool;
    
    
    
    [Header("Characters")]
    public CharacterSheet[] charactersInGame;
    


    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    public StoryScene startingStoryScene;
    public Dialogue _curentActiveDialog, _lastActiveDialog, _lastDialogChoice;

    [Header("Dialogue")]    
    public bool stopReadingTextBool = false;
    [SerializeField] private List<Dialogue> _blockedDialogue = new List<Dialogue>();
    [SerializeField] private List<Dialogue> _listOfDialogOptions = new List<Dialogue>();
    int textLenght = 0, textPostition=0;
    string textString = "", textTarget="";
    float timerTyping=0; public float typpingSpeed =0.05f;
    float uiMenuSelectorTimer=0, uiMenuSelectorInterval=0.5f;
    Vector4 v4Visibility= new Vector4(1,1,1,1); 
    float valueW=0;
    float timerSpace=0, timerEnter=0, timerMouse=0, timerWaitInput=0, timerProtectTyping=0;
    public TMP_FontAsset standartSpeakingFont;
    

    


    [Header("References")]
    public Image _actorLeftSprite; public Image _actorRightSprite, _actorMiddleSprite;
    public Image _actorTagLeftSprite, _actorTagRightSprite, _actorTagMiddleSprite;
    [SerializeField] private Image _bgImage;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private TextMeshProUGUI _dialogueBodyText;
    [SerializeField] private Transform _dialogueOptionsBox;
    [SerializeField] private GameObject _buttonPrefab;
    public Transform _nextDialogButtonTransform;
    [SerializeField] private TMP_Text _reputationNumberText;
    public Transform _reputationTransform;
    public Transform _informativeBlackScreenPanel, _informativeBSButton;
    public TMP_Text _informativeBSText;
    public Image _informativeBSImage;
    public Transform _informativeBSCloudImage;
    public MoveToPosition movabelItemScript;
    public Image movableItemImage;
    public Transform _computerDialogPanel, _computerButtonsPanel;
    public TMP_Text _computerDialogText;
    public List<ComputerDialogButton> _listOfCDButtons;
    [SerializeField] private GameObject _computerDialogButtonPrefab;

    public Transform _diodTyping, _diodOptions, _diodSpaceBar, _diodNextBtn,
        _diodEnter, _diodMouse, _diodTextDisplayed, _diodSFXHold;
    

    [Header("Managers")]
    public MenuPanelManager menuManager;
    public ReputationManager reputationManager;
    public AudioManager audioManager;
    public CreditsManager creditsManager;
    public AppearDissapear appearDissapear;








    private void Start()
    {
        //operating from main menu
        //ChangeStoryScene(startingStoryScene);
        if(_dialogueOptionsBox.gameObject.activeInHierarchy)
            _dialogueOptionsBox.gameObject.SetActive(false);
        _nextDialogButtonTransform.gameObject.SetActive(false);
        ReputationTransformEnable(false);
        OperateDiodes(0, false, true);
    }

    private void Update()
    {
        if(spaceIsDownBool){
            timerSpace += Time.deltaTime;
            if(timerSpace >= 0.50f){
                timerSpace = 0;
                spaceIsDownBool = false;
                OperateDiodes(3, false, false);
            }
        }

        if(enterIsDownBool){
            timerEnter += Time.deltaTime;
            if(timerEnter >= 0.50f){
                timerEnter = 0;
                enterIsDownBool = false;
                OperateDiodes(5, false, false);
            }
        }

        if(mouseIsDownBool){
            timerMouse += Time.deltaTime;
            if(timerMouse >= 0.50f){
                timerMouse = 0;
                mouseIsDownBool = false;
                OperateDiodes(6, false, false);
            }
        }

        if(waitInputBool){
            timerWaitInput += Time.deltaTime;
            if(timerWaitInput >= 0.40f){
                timerWaitInput = 0;
                waitInputBool = false;
                //OperateDiodes(3, false, false);
            }
        }
        
        if(waitProtectTippingBool){
            timerProtectTyping += Time.deltaTime;
            if(timerProtectTyping >= 1f){
                timerProtectTyping = 0;
                waitProtectTippingBool = false;
                //OperateDiodes(3, false, false);
            }
        }

        

        // Stop text from reading if player presses spacebar
        if (Input.GetKeyDown(KeyCode.Space)) 
            if (!spaceIsDownBool)
            if (!technicalStepsPanelOpenBool)
            if (!waitProtectTippingBool)
            if (!stopReadingTextBool)
            {
                stopReadingTextBool = true;
                if(menuManager.creditPanelOnBool) creditsManager.stopReadingTextBoll = true;
                OperateSpace();            
            }        
        
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        if (!IsMouseOverUIButton())
        if (!technicalStepsPanelOpenBool)
        {                        
            if (typingBool)
            {
                if (!waitProtectTippingBool)
                if (!soundEfectHoldingTippingBool)
                if (clearToTypeBool)
                if (!mouseIsDownBool)
                {
                    mouseIsDownBool = true;
                    stopReadingTextBool = true;
                    //Debug.Log("Mouse 0 typping off!");
                }
            }



            if (nextDialogButtonOnBool == true)
            {
                if (!menuManager.informativeBSPanelBool)
                if (!menuManager.computerDialogPanelOnBool)
                if (!typingBool)
                if (bodyTextDisplayedBool)
                if (!waitProtectTippingBool)
                if (!mouseIsDownBool)
                {
                    mouseIsDownBool = true;
                    nextDialogButtonOnBool = false;
                    //Debug.Log("Mouse OrderFromNextDialogueButton");
                    OrderFromNextDialogueButton();
                }
            }


            if(menuManager.creditPanelOnBool)
            {
                if(creditsManager.typingBool)
                if (!waitProtectTippingBool)
                if(!mouseIsDownBool)
                {
                    mouseIsDownBool = true;
                    creditsManager.stopReadingTextBoll = true;
                    //Debug.Log("Mouse0 triger typing stop on credits manager.");
                }
                

                if(menuManager.creditsManager.bodyTextDisplayedBool)
                if(!mouseIsDownBool)
                {
                    mouseIsDownBool = true;
                    Debug.Log("Mouse0 triger next button on credits manager.");
                    menuManager.creditsManager.NextButton();
                }
            }


            
            if(menuManager.informativeBSPanelBool)
            if(typingBool == false)
            if(bodyTextDisplayedBool)
            if(_informativeBSButton.gameObject.activeInHierarchy)
            if(!mouseIsDownBool)
            {
                mouseIsDownBool = true;
                //Debug.Log("Mouse0 triger next button on informative panel.");
                appearDissapear.OrderForBSPanel(false, true, false);
            }
            



            if(mouseIsDownBool) OperateDiodes(6, true, false);
        }// MOUSE 0


        if (Input.GetKeyDown(KeyCode.Return))
        if (!technicalStepsPanelOpenBool){
            if (!enterIsDownBool){
                OperateSpace();

                if(nextDialogButtonOnBool){
                    enterIsDownBool = true;
                    nextDialogButtonOnBool = false;
                    OrderFromNextDialogueButton();
                }

                if(menuManager.creditPanelOnBool)
                if(menuManager.creditsManager.typingBool == false)
                if(typingBool == false){
                    enterIsDownBool = true;
                    //Debug.Log("Enter triger next button on credits manager.");
                    menuManager.creditsManager.NextButton();
                }

                if(menuManager.informativeBSPanelBool)
                if(typingBool == false)
                if(_informativeBSButton.gameObject.activeInHierarchy){
                    enterIsDownBool = true;
                    //Debug.Log("Enter triger next button on informative panel.");
                    appearDissapear.OrderForBSPanel(false, true, false);
                }

            }
            if(enterIsDownBool) OperateDiodes(5, true, false);
        } 
        
        
        //dialogue options
        if(haveOptionsBool && optionsAreDisplayedBool)
        if (!technicalStepsPanelOpenBool){
            if(_listOfDialogOptions.Count > 0)
            for(int i=0; i< _listOfDialogOptions.Count; i++){
                if(_listOfDialogOptions[i] != null){
                    if(i==0) if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[0], true);
                    } 
                    if(i==1) if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[1], true);
                    } 
                    if(i==2) if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[2], true);
                    } 
                    if(i==3) if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[3], true);
                    }
                    if(i==4) if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[4], true);
                    }
                    if(i==5) if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6) ){
                        OnClickSelectDialogueOption(_listOfDialogOptions[5], true);
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
                
                
                audioManager.PlayTyppingSound();
                OperateDiodes(1, true, false);
                OperateDiodes(8, false, false);

                if(voiceActorReadyBool) {
                    audioManager.PlayNormalVoiceActor(0);
                    //audioManager.PlayVoiceActorAudio(null, false, false);
                    voiceActorReadyBool = false;
                }
            }            
        }


        if(typingBool)
        if (!technicalStepsPanelOpenBool){
            if(!soundEfectHoldingTippingBool)
                if(clearToTypeBool){
                    timerTyping += Time.deltaTime;
                    if(timerTyping > typpingSpeed){
                        timerTyping = 0;
                        textString = textTarget.Substring(0, textPostition);
                        textString += "<color=#00000000>" + textTarget.Substring(textPostition);
                        if (stopReadingTextBool){
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
                            if(_curentActiveDialog.reputation != 0){
                                Debug.Log("Aditional change to reputation! "
                                    + _curentActiveDialog.reputation);
                                reputationManager.ReputationChange(_curentActiveDialog.reputation);
                            }

                            if(menuManager.computerDialogPanelOnBool)
                                RevealDialougeOptions(_curentActiveDialog);
                            bodyTextDisplayedBool = true; 
                            OperateDiodes(1, false, false);
                            OperateDiodes(7, true, false);
                            RevealNextButton();
                        }
                    }
                }
        }


        if(preparedForTheNextDialogueBool)
        if (!technicalStepsPanelOpenBool){
            if(preparedForTheNextDialogueValue != null){
                Dialogue nextDialogue = preparedForTheNextDialogueValue;
                preparedForTheNextDialogueValue = null;
                preparedForTheNextDialogueBool = false;
                UpdateDialogueBox(nextDialogue);
            }else{
                preparedForTheNextDialogueBool = false;
                Debug.Log("Problem, trying to lauch a non exsisten dialogue!");
            }
        }
    

    }// update





    private bool IsMouseOverUIButton(){
        bool resultBool = false;

        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> raycastResultsList = new List<RaycastResult>();
        
        EventSystem.current.RaycastAll(pointerEventData, raycastResultsList);

        for (int i = 0; i < raycastResultsList.Count; i++)
        {
            if(raycastResultsList[i].gameObject.activeInHierarchy)
            if(raycastResultsList[i].gameObject.GetComponent<Button>() != null){
                resultBool = true;
            }
        }

        return resultBool;
    }




    public void PrepareToUpdateDialogueBox(Dialogue dialogue){
        preparedForTheNextDialogueValue = dialogue;
        preparedForTheNextDialogueBool = true;
    }












    private void UpdateDialogueBox(Dialogue dialogue)
    {
        ResetDialogueOptions();
        HandleDialogueBlocking(dialogue.DialogueBlockers);
        bodyTextDisplayedBool = false;
        

        if(_curentActiveDialog != null) _lastActiveDialog = _curentActiveDialog;
        _curentActiveDialog = dialogue;
        audioManager.StopActorsAndSfxAudio();

        if(dialogue.newSpriteForScene != null) ChangeBackgroundImage(dialogue.newSpriteForScene);

        
        WhiteOrGrayNpcImage(true, false, false, dialogue.characterOnLeftWhiteBool);
        ChangeNpcImage(true, false, false, dialogue.leftCharacterSheet, dialogue.characterOnLeftSpriteNumber);            
        if(dialogue.actorOnLeftAppearBool) appearDissapear.OrderToAppearDisaper(true, false, false, true);
        if(dialogue.actorOnLeftDisapearBool) appearDissapear.OrderToAppearDisaper(true, false, false, false);
        if(dialogue.actorOnLeftAlpha0Bool) appearDissapear.OrderAlpha0(true, false, false);
        if(dialogue.editLeftActorOnEndBool) appearDissapear.holdLeftTippingBool = true;


        WhiteOrGrayNpcImage(false, true, false, dialogue.characterOnRightWhiteBool);
        ChangeNpcImage(false, true, false, dialogue.rightCharacterSheet, dialogue.characterOnRightSpriteNumber);
        if(dialogue.actorOnRightAppearBool) appearDissapear.OrderToAppearDisaper(false, true, false, true);
        if(dialogue.actorOnRightDisapearBool) appearDissapear.OrderToAppearDisaper(false, true, false, false);
        if(dialogue.actorOnRightAlpha0Bool) appearDissapear.OrderAlpha0(false, true, false);
        if(dialogue.editRightActorOnEndBool) appearDissapear.holdRightTippingBool = true;

        
        WhiteOrGrayNpcImage(false, false, true, dialogue.characterOnMiddleWhiteBool);
        ChangeNpcImage(false, false, true, dialogue.middleCharacterSheet, dialogue.characterOnRightSpriteNumber);
        if(dialogue.actorOnMiddleAppearBool) appearDissapear.OrderToAppearDisaper(false, false, true, true);
        if(dialogue.actorOnMiddleDisapearBool) appearDissapear.OrderToAppearDisaper(false, false, true, false);
        if(dialogue.actorOnMiddleAlpha0Bool) appearDissapear.OrderAlpha0(false, false, true);
        if(dialogue.editMiddleActorOnEndBool) appearDissapear.holdMiddleTippingBool = true;

       
        _speakerName.text = charactersInGame[dialogue.characterInUse].characterName;
        if(dialogue.overwriteNameText != "") _speakerName.text = dialogue.overwriteNameText;
        _speakerName.color = charactersInGame[dialogue.characterInUse].nameColor;
        _dialogueBodyText.text = "";
        OperateDiodes(7, false, false);
        if(charactersInGame[dialogue.characterInUse].speakingFont == null)
            _dialogueBodyText.font = standartSpeakingFont;
        else
            _dialogueBodyText.font = charactersInGame[dialogue.characterInUse].speakingFont;
        _reputationNumberText.text = reputationManager._reputation.ToString();



        if(dialogue.soundEffectAudioClip != null)
        {            
            soundEfectHoldingTippingBool = true;
            audioManager.PlaySfxAudio(dialogue.soundEffectAudioClip);
            OperateDiodes(8, true, false);
        }

        if(dialogue.soundEffectAtEndClip != null){
            audioManager.PlaySFXatEnd(dialogue.soundEffectAtEndClip);
        }

        if(dialogue.delayedSoundEffectClip != null){
            audioManager.PlayDelayedSfx(dialogue.delayedSoundEffectClip, dialogue.delayedSoundEffectString);
        }

        if(dialogue.voiceActorAudioClip != null){
            voiceActorReadyBool = true;
            audioManager.norvalVoiceAudioClip = dialogue.voiceActorAudioClip;
        }

        if(dialogue.musicAudioClip != null){
            audioManager.ChangeBacgroundMusic(dialogue.musicAudioClip);
        }

        if(soundEfectHoldingTippingBool == false){
            
            if(voiceActorReadyBool) {
                //audioManager.PlayVoiceActorAudio(null, false, false);
                audioManager.PlayNormalVoiceActor(0);
                voiceActorReadyBool = false;
            }

            //shall i stop these two
            



        }else{}
        
        
        
        

        
        _listOfDialogOptions.Clear();
        _listOfCDButtons.Clear();
        foreach (DialogueOption option in dialogue.DialogueOptions)
        {
            if (!_blockedDialogue.Contains(option.Dialogue))
            {
                GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
                Button button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue, true));
                _listOfDialogOptions.Add(option.Dialogue);
                haveOptionsBool = true;
                if(dialogue.computerDialogBool){
                    buttonObj = Instantiate(_computerDialogButtonPrefab, _computerButtonsPanel);
                    button = buttonObj.GetComponent<Button>();
                    buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                    button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue, true));
                    _listOfCDButtons.Add(buttonObj.GetComponent<ComputerDialogButton>());
                }

                //reputation limiter
                if(option.reputationLimiterBool){
                    bool passBool = false;
                    if(option.mustBeEqualBool)
                        if(option.requiredValue == reputationManager._reputation) passBool = true;

                    if(option.mustBeLessBool)
                        if(reputationManager._reputation < option.requiredValue) passBool = true;

                    if(option.mustBeMoreBool)
                        if(reputationManager._reputation > option.requiredValue) passBool = true;

                    Debug.Log("Reputation Limiter in Action. Actual reputation ("+ reputationManager._reputation 
                        +") Equal ["+ option.mustBeEqualBool +"]"
                        +") More ["+ option.mustBeMoreBool +"]"
                        +") Less ["+ option.mustBeLessBool +"]"
                        + " Target reputation (" + option.requiredValue + ")");
                    if(!passBool) Destroy(buttonObj);
                }
            }
        }
                
        if(_listOfDialogOptions.Count == 0)
        if(dialogue.DialogueWhenNoneAviable.Dialogue != null){
            GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
            Button button = buttonObj.GetComponent<Button>();
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.DialogueWhenNoneAviable.OptionName;
            button.onClick.AddListener(() => OnClickSelectDialogueOption(dialogue.DialogueWhenNoneAviable.Dialogue, true));
            _listOfDialogOptions.Add(dialogue.DialogueWhenNoneAviable.Dialogue);
            haveOptionsBool = true;
            if(dialogue.computerDialogBool){
                buttonObj = Instantiate(_computerDialogButtonPrefab, _computerButtonsPanel);
                button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = dialogue.DialogueWhenNoneAviable.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(dialogue.DialogueWhenNoneAviable.Dialogue, true));
                _listOfCDButtons.Add(buttonObj.GetComponent<ComputerDialogButton>());
            }
        }//else{Debug.Log("There is no safety line[dialog] to put in when there are no options.");}


        //sound and typping
        if(haveOptionsBool){
            //_lastDialogChoice = dialogue;
            if(dialogue.computerDialogBool) ReadTextStartTypping();
            RevealDialougeOptions(dialogue);
            normalDialogBool = false;
        }else{
            if(!dialogue.computerDialogBool)
                normalDialogBool = true;
            ReadTextStartTypping();

            if(soundEfectHoldingTippingBool == false){
                OperateDiodes(1, true, false);
                audioManager.PlayTyppingSound();
            }
        }


        if(_informativeBSCloudImage.gameObject.activeInHierarchy)
            _informativeBSCloudImage.gameObject.SetActive(false);

        if(dialogue.informativeBSBool){
            appearDissapear.OrderForBSPanel(true, false, false);
            if(dialogue.informativeSuportSprite != null){
                _informativeBSImage.sprite = dialogue.informativeSuportSprite;
            }else{
                if(_informativeBSImage.sprite != emptyTransparentSprite)
                    _informativeBSImage.sprite = emptyTransparentSprite;
            }
            normalDialogBool = false;
            if(dialogue.placeMistUnderInformativeBool){
                _informativeBSCloudImage.gameObject.SetActive(true);
            }
        }

        if(dialogue.computerDialogBool){
            menuManager.computerDialogPanelOnBool = true;
            _computerDialogPanel.gameObject.SetActive(true);
            _computerButtonsPanel.gameObject.SetActive(false);
            _computerDialogText.text = "";
            normalDialogBool = false;
        }


        if(dialogue.itemPlaceLeft) movabelItemScript.OrderToMove(true, false, false, 0);
        if(dialogue.itemPlaceRight) movabelItemScript.OrderToMove(false, true, false, 0);
        if(dialogue.itemMoveBool)
            movabelItemScript.OrderToMove(false, false, true, dialogue.itemNewCordinates);
        if(dialogue.ItemNewSprite != null) movableItemImage.sprite = dialogue.ItemNewSprite;


        //step back
        if(stepBackBool){
            Debug.Log("Step back, try to make an alternations on bacground image and music.");
            stepBackBool = false;
            if(dialogue.lastBacgroundSprite != null)
                if(_bgImage.sprite != dialogue.lastBacgroundSprite)
                _bgImage.sprite = dialogue.lastBacgroundSprite;
            if (dialogue.lastMusicClip != null)
                if(audioManager._musicPlayer.clip != dialogue.lastMusicClip)
                audioManager._musicPlayer.clip = dialogue.lastMusicClip;
        }

        if(dialogue.redirectionOnStoryScene != null){
            ChangeStoryScene(dialogue.redirectionOnStoryScene);
        }

        if(dialogue.endingTheStoryBool){
            Debug.Log("ending scene");
            menuManager.creditsManager.endingNumber = _curentActiveDialog.endigNumber;
            ClearTheGame();
            menuManager.creditsManager.OpenStartEndCreditsPanel(false, true);
        }
    }//UpdateDialogueBox











    public void StartTheGame(){
        reputationManager.reputationChangesDuringTheGame = 0;  
        ChangeStoryScene(startingStoryScene);
        clearToTypeBool = true;
    }

    public void ClearTheGame(){
        _blockedDialogue.Clear();
        audioManager._musicPlayer.Stop();
        ResetDialogueOptions();
        _lastActiveDialog = null;
        _lastDialogChoice = null;

        reputationManager._reputation = 0;
        menuManager.gameIsOnBool = false;
        clearToTypeBool = true;
        typingBool = false;
        bodyTextDisplayedBool = false;
        voiceActorReadyBool = false;
        soundEfectHoldingTippingBool = false;        
        _reputationNumberText.text = "";

        movableItemImage.sprite = emptyTransparentSprite;
        _actorLeftSprite.sprite = emptyTransparentSprite;
        _actorRightSprite.sprite = emptyTransparentSprite;
        _actorMiddleSprite.sprite = emptyTransparentSprite;
        _actorTagLeftSprite.sprite = emptyTransparentSprite;
        _actorTagRightSprite.sprite = emptyTransparentSprite;
        _actorTagMiddleSprite.sprite = emptyTransparentSprite;

        appearDissapear.Clear();
        movabelItemScript.OrderToMove(true, false, false, 0);
        OperateDiodes(0, false, true);
        ReputationTransformEnable(false);
    }


    private void ChangeStoryScene(StoryScene storyScene)
    {
        ActiveStoryScene = storyScene;
        ChangeBackgroundImage(storyScene.BackgroundImage);
        if(storyScene.sceneMusicAudioClip != null)
            audioManager.ChangeBacgroundMusic(storyScene.sceneMusicAudioClip);        
        PrepareToUpdateDialogueBox(storyScene.StartingDialogue);
        if (storyScene.startingPointOfStoryBool){
            reputationManager.ReputationBeginStory();
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

        if(optionsAreDisplayedBool){
            Debug.Log("Options are on? Turning off that panel.");
            _dialogueOptionsBox.gameObject.SetActive(false);
            optionsAreDisplayedBool = false;
        }
    }

    public void ChangeNpcImage(bool itsLeftBool, bool itsRightBool, bool itsMiddleBool, CharacterSheet characterSheet, int number)
    {
        Sprite chosenSprite = emptyTransparentSprite,
            chosenTagSprite = emptyTransparentSprite ;
        if(characterSheet != null){

            if(characterSheet.spritesArray.Length > number){
                if(characterSheet.spritesArray[number] != null)
                    chosenSprite = characterSheet.spritesArray[number];
            }

            if(chosenSprite == emptyTransparentSprite){
                if(characterSheet.baseSprite != emptyTransparentSprite)
                    chosenSprite = characterSheet.baseSprite;
            }

            if(characterSheet.nameTagSprite != null)
                chosenTagSprite = characterSheet.nameTagSprite;

            if(itsLeftBool){
                _actorLeftSprite.sprite = chosenSprite;
                _actorTagLeftSprite.sprite = chosenTagSprite;
            }
            if(itsRightBool){
                _actorRightSprite.sprite = chosenSprite; 
                _actorTagRightSprite.sprite = chosenTagSprite;               
            }
            if(itsMiddleBool){
                _actorRightSprite.sprite = chosenSprite;  
                _actorTagMiddleSprite.sprite = chosenTagSprite;              
            }

            if(_curentActiveDialog.overwriteNameText != "")
                if(charactersInGame[_curentActiveDialog.characterInUse] != null)
                if(charactersInGame[_curentActiveDialog.characterInUse] 
                == characterSheet)
                {
                    if(itsLeftBool) _actorTagLeftSprite.sprite = emptyTransparentSprite;
                    if(itsRightBool) _actorTagRightSprite.sprite = emptyTransparentSprite;
                    if(itsMiddleBool) _actorTagMiddleSprite.sprite = emptyTransparentSprite;
                }

        }else{
            if(itsLeftBool){
                _actorLeftSprite.sprite = emptyTransparentSprite;
                _actorTagLeftSprite.sprite = emptyTransparentSprite;
            }
            if(itsRightBool){
                _actorRightSprite.sprite = emptyTransparentSprite;
                _actorTagRightSprite.sprite = emptyTransparentSprite;
            }
            if(itsMiddleBool){
                _actorMiddleSprite.sprite = emptyTransparentSprite;
                _actorTagMiddleSprite.sprite = emptyTransparentSprite;
            }
        }
        
        
    }

    private void WhiteOrGrayNpcImage(bool itsLeftBool, bool itsRightBool, bool itsMiddleBool, bool whiteBool){
        if(itsLeftBool){

            if(whiteBool){
                if(_actorLeftSprite.color != Color.white) {
                    valueW = _actorLeftSprite.color.a;
                    v4Visibility = (Vector4)Color.white;
                    v4Visibility.w = valueW;
                    _actorLeftSprite.color = v4Visibility;
                }
            }else{
                if(_actorLeftSprite.color != Color.grey) {
                        valueW = _actorLeftSprite.color.a;
                        v4Visibility = (Vector4)Color.grey;
                        v4Visibility.w = valueW;
                        _actorLeftSprite.color = v4Visibility;
                }     
            }    
        }
        
        if(itsRightBool){

            if(whiteBool){
                if(_actorRightSprite.color != Color.white) {
                    valueW = _actorRightSprite.color.a;
                    v4Visibility = (Vector4)Color.white;
                    v4Visibility.w = valueW;
                    _actorRightSprite.color = v4Visibility;
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

        if(itsMiddleBool){

            if(whiteBool){
                if(_actorMiddleSprite.color != Color.white) {
                    valueW = _actorMiddleSprite.color.a;
                    v4Visibility = (Vector4)Color.white;
                    v4Visibility.w = valueW;
                    _actorMiddleSprite.color = v4Visibility;
                }
            }else{
                if(_actorMiddleSprite.color != Color.grey) {
                        valueW = _actorMiddleSprite.color.a;
                        v4Visibility = (Vector4)Color.grey;
                        v4Visibility.w = valueW;
                        _actorMiddleSprite.color = v4Visibility;
                }     
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

    




    private void ReadTextStartTypping(){
        stopReadingTextBool = false;
        typingBool = true;        
        waitProtectTippingBool = true;
        textLenght = _curentActiveDialog.BodyText.Length +1;
        textTarget = _curentActiveDialog.BodyText +" ";
        textPostition = 0;
    }


    private void RevealNextButton(){
            nextDialogButtonOnBool = true;
            _nextDialogButtonTransform.gameObject.SetActive(true);
            OperateDiodes(4, true, false);
    }

    private void RevealDialougeOptions(Dialogue dialogue)
    {
            if(!menuManager.computerDialogPanelOnBool)
                _dialogueOptionsBox.gameObject.SetActive(true);
            else
                _computerButtonsPanel.gameObject.SetActive(true);

            OperateDiodes(2, true, false);
            optionsAreDisplayedBool = true;
    }

    public void OrderFromInfoBlackScreen(){
        PrepareToUpdateDialogueBox(_curentActiveDialog.DialogueNext);
    }


    public void OrderFromNextDialogueButton(){
            OperateDiodes(4, false, false);
            _nextDialogButtonTransform.gameObject.SetActive(false);
            nextDialogButtonOnBool = false;
            PrepareToUpdateDialogueBox(_curentActiveDialog.DialogueNext);
    }

    public void OnClickSelectDialogueOption(Dialogue dialogue, bool playSFxBool)
    {
        if(playSFxBool) audioManager.PlayUiAudio(audioManager.selectDialogAC);
        _dialogueOptionsBox.gameObject.SetActive(false);
        if(menuManager.computerDialogPanelOnBool){
            menuManager.computerDialogPanelOnBool = false;
            _computerDialogText.text = "";
            _computerDialogPanel.gameObject.SetActive(false);
        }
        optionsAreDisplayedBool = false;
        clearToTypeBool = false;
        OperateDiodes(2, false, false);
        if(nextDialogButtonOnBool) OperateDiodes(4, false, false);        
        reputationManager.ReputationChange(dialogue.reputation);
        _lastDialogChoice = _curentActiveDialog;
        _lastDialogChoice.lastBacgroundSprite = _bgImage.sprite;
        _lastDialogChoice.lastMusicClip = audioManager._musicPlayer.clip;
        PrepareToUpdateDialogueBox(dialogue);
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
        if(!reputationEnabledBool) ReputationTransformEnable(true);
        _reputationNumberText.text = number.ToString();
        if(addon != "") _reputationNumberText.text += addon; 

        if(number == 0) if(_reputationNumberText.color !=Color.white)
            _reputationNumberText.color =Color.white;
        if(number > 0) if(_reputationNumberText.color !=Color.green)
            _reputationNumberText.color =Color.green;
        if(number < 0) if(_reputationNumberText.color !=Color.magenta)
            _reputationNumberText.color =Color.magenta;
    }


    void ReputationTransformEnable(bool option){
        if(option){
            _reputationTransform.gameObject.SetActive(false);
            reputationEnabledBool = false;
        }else{
            _reputationTransform.gameObject.SetActive(true);
            reputationEnabledBool = true;
        }
    }

    

    

    void OperateDiodes(int diodeNumber, bool option, bool clearAllBool){
        if(clearAllBool){
            _diodTyping.gameObject.SetActive(false); //1
            _diodOptions.gameObject.SetActive(false); //2
            _diodSpaceBar.gameObject.SetActive(false); //3
            _diodNextBtn.gameObject.SetActive(false); //4
            _diodEnter.gameObject.SetActive(false); //5
            _diodMouse.gameObject.SetActive(false); //6
            _diodTextDisplayed.gameObject.SetActive(false); //7
            _diodSFXHold.gameObject.SetActive(false); //8
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

            if(diodeNumber == 4)
            if(!option) _diodNextBtn.gameObject.SetActive(false);
            else _diodNextBtn.gameObject.SetActive(true);

            if(diodeNumber == 5)
            if(!option) _diodEnter.gameObject.SetActive(false);
            else _diodEnter.gameObject.SetActive(true);

            if(diodeNumber == 6)
            if(!option) _diodMouse.gameObject.SetActive(false);
            else _diodMouse.gameObject.SetActive(true);

            if(diodeNumber == 7)
            if(!option) _diodTextDisplayed.gameObject.SetActive(false);
            else _diodTextDisplayed.gameObject.SetActive(true);

            if(diodeNumber == 8)
            if(!option) _diodSFXHold.gameObject.SetActive(false);
            else _diodSFXHold.gameObject.SetActive(true);
        }
    }

    void OperateSpace(){
        spaceIsDownBool = true;
        OperateDiodes(3, true, false);
    }



    



    

}
