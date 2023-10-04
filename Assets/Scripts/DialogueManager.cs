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
    public bool enterIsDownBool, mouseIsDownBool;
    
    public bool soundEfectHoldingTippingBool, voiceActorReadyBool=true, typingBool;
    public bool haveOptionsBool, optionsAreDisplayedBool, clearToTypeBool;
    public bool nextDialogButtonOnBool; 
    public bool reputationEnabledBool;  
    public Dialogue _curentActiveDialog;
    [SerializeField] private Sprite emptyTransparentSprite;
    public bool preparedForTheNextDialogueBool;
    private Dialogue preparedForTheNextDialogueValue=null;
    
    
    
    [Header("Characters")]
    public CharacterSheet[] charactersInGame;
    


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
    float timerSpace=0, timerEnter=0, timerMouse=0;
    public TMP_FontAsset standartSpeakingFont;

    


    [Header("References")]
    [SerializeField] public Image _actorLeftSprite, _actorRightSprite;
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
    public MoveToPosition movabelItemScript;
    public Image movableItemImage;
    public Transform _computerDialogPanel, _computerButtonsPanel;
    public TMP_Text _computerDialogText;
    public List<ComputerDialogButton> _listOfCDButtons;
    [SerializeField] private GameObject _computerDialogButtonPrefab;

    public Transform _diodTyping, _diodOptions, _diodSpaceBar, _diodNextBtn;
    

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
                //OperateDiodes(3, false, false);
            }
        }

        if(mouseIsDownBool){
            timerMouse += Time.deltaTime;
            if(timerMouse >= 0.50f){
                timerMouse = 0;
                enterIsDownBool = false;
                //OperateDiodes(3, false, false);
            }
        }
        
        // Stop text from reading if player presses spacebar
        if (Input.GetKeyDown(KeyCode.Space)) 
        if (!spaceIsDownBool)
        {
            stopReadingText = true; 
            OperateSpace();
            
        }        
        if(!mouseIsDownBool)
        if (Input.GetKeyDown(KeyCode.Mouse0)){
            if(typingBool == true){
                stopReadingText = true;
                OperateSpace();
            }

        }


        if (Input.GetKeyDown(KeyCode.Return)){
            if (!enterIsDownBool){
                OperateSpace();

                if(nextDialogButtonOnBool){
                    nextDialogButtonOnBool = false;
                    OrderFromNextDialogueButton();
                    //OnClickSelectDialogueOption(_curentActiveDialog.DialogueNext, false);
                }
                /*
                if(optionsAreDisplayedBool){
                    if(_listOfDialogOptions.Count == 1){
                        Debug.Log("Enter triger Next on dialog.");
                       OnClickSelectDialogueOption(_listOfDialogOptions[0]);
                    }
                }
                */

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

                if(voiceActorReadyBool) {
                    audioManager.PlayVoiceActorAudio(null, false, false);
                    voiceActorReadyBool = false;
                }
            }            
        }


        if(typingBool){
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
                            //RevealDialougeOptions(_curentActiveDialog);
                            RevealNextButton();
                            OperateDiodes(1, false, false);
                        }
                    }
                }
        }


        if(preparedForTheNextDialogueBool){
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




    public void PrepareToUpdateDialogueBox(Dialogue dialogue){
        preparedForTheNextDialogueValue = dialogue;
        preparedForTheNextDialogueBool = true;
    }



    private void UpdateDialogueBox(Dialogue dialogue)
    {
        ResetDialogueOptions();
        HandleDialogueBlocking(dialogue.DialogueBlockers);
        

        _curentActiveDialog = dialogue;
        audioManager.StopActorsAndSfxAudio();

        
        WhiteOrGrayNpcImage(true, dialogue.characterOnLeftWhiteBool);
        ChangeNpcImage(true, dialogue.leftCharacterSheet, dialogue.characterOnLeftSpriteNumber);            
        if(dialogue.actorOnLeftAppearBool) appearDissapear.OrderToAppearDisaper(true, true);
        if(dialogue.actorOnLeftDisapearBool) appearDissapear.OrderToAppearDisaper(true, false);
        if(dialogue.actorOnLeftAlpha0Bool) appearDissapear.OrderAlpha0(true);


        WhiteOrGrayNpcImage(false, dialogue.characterOnRightWhiteBool);
        ChangeNpcImage(false, dialogue.rightCharacterSheet, dialogue.characterOnRightSpriteNumber);
        if(dialogue.actorOnRightAppearBool) appearDissapear.OrderToAppearDisaper(false, true);
        if(dialogue.actorOnRightDisapearBool) appearDissapear.OrderToAppearDisaper(false, false);
        if(dialogue.actorOnRightAlpha0Bool) appearDissapear.OrderAlpha0(false);

       
        _speakerName.text = charactersInGame[dialogue.characterInUse].characterName;
        _speakerName.color = charactersInGame[dialogue.characterInUse].nameColor;
        _dialogueBodyText.text = "";
        if(charactersInGame[dialogue.characterInUse].speakingFont == null)
            _dialogueBodyText.font = standartSpeakingFont;
        else
            _dialogueBodyText.font = charactersInGame[dialogue.characterInUse].speakingFont;
        _reputationNumberText.text = reputationManager._reputation.ToString();



        if(dialogue.soundEffectAudioClip != null)
        {            
            soundEfectHoldingTippingBool = true;
            audioManager.PlaySfxAudio(dialogue.soundEffectAudioClip);
        }

        if(dialogue.voiceActorAudioClip != null){
            voiceActorReadyBool = true;
            if(dialogue.characterOnLeftWhiteBool)
                audioManager.InsertVoiceAudioClip(dialogue.voiceActorAudioClip, true);
            if(dialogue.characterOnRightWhiteBool)
                audioManager.InsertVoiceAudioClip(dialogue.voiceActorAudioClip, false);
        }

        if(dialogue.musicAudioClip != null){
            audioManager.ChangeBacgroundMusic(dialogue.musicAudioClip);
        }

        if(soundEfectHoldingTippingBool == false){
            
            if(voiceActorReadyBool) {
                audioManager.PlayVoiceActorAudio(null, false, false);
                voiceActorReadyBool = false;
            }
            OperateDiodes(1, true, false);
            audioManager.PlayTyppingSound();
        }else{}
        
        ReadTextVariantTwo();
        
        

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


        if(dialogue.informativeBSBool){
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
        ReputationTransformEnable(false);
    }


    private void ChangeStoryScene(StoryScene storyScene)
    {
        ActiveStoryScene = storyScene;
        ChangeBackgroundImage(storyScene.BackgroundImage);
        if(storyScene.sceneMusicAudioClip != null)
            audioManager.ChangeBacgroundMusic(storyScene.sceneMusicAudioClip);
        if(storyScene.enableReputationBool)
            ReputationTransformEnable(true);
        PrepareToUpdateDialogueBox(storyScene.StartingDialogue);
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

    public void ChangeNpcImage(bool itsLeftBool, CharacterSheet characterSheet, int number)
    {
        Sprite chosenSprite = emptyTransparentSprite;
        if(characterSheet != null){

            if(characterSheet.spritesArray.Length > number){
                if(characterSheet.spritesArray[number] != null)
                    chosenSprite = characterSheet.spritesArray[number];
            }

            if(chosenSprite == emptyTransparentSprite){
                if(characterSheet.baseSprite != emptyTransparentSprite)
                    chosenSprite = characterSheet.baseSprite;
            }

            if(itsLeftBool){
                _actorLeftSprite.sprite = chosenSprite;
                /*
                if(image != null){
                    _actorLeftSprite.sprite = image;
                    
                }
                else
                _actorLeftSprite.sprite = emptyTransparentSprite; 
                */ 
            }else{
                _actorRightSprite.sprite = chosenSprite;
                /*
                if(image != null){
                    _actorRightSprite.sprite = image; 
                    
                }            
                else
                _actorRightSprite.sprite = emptyTransparentSprite;
                */
            }  

        }else{
            if(itsLeftBool)
                _actorLeftSprite.sprite = emptyTransparentSprite; 
            else
                _actorRightSprite.sprite = emptyTransparentSprite;
            //Debug.Log("Warning character Sheet empty on ChangeNpcImage!");
        }
        
        
    }

    private void WhiteOrGrayNpcImage(bool itsLeftBool, bool whiteBool){
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

        }else{

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
    }

   

    private void ChangeBackgroundImage(Sprite bgImage)
    {
        if(bgImage != null)
        _bgImage.sprite = bgImage;
        else
        _bgImage.sprite = emptyTransparentSprite;
    }

    




    private void ReadTextVariantTwo(){
        stopReadingText = false;
        typingBool = true;        
        textLenght = _curentActiveDialog.BodyText.Length +1;
        textTarget = _curentActiveDialog.BodyText +" ";
        textPostition = 0;
    }


    private void RevealNextButton(){
        if(     menuManager.creditPanelOnBool 
            ||  menuManager.informativeBSPanelBool 
            ||  menuManager.computerDialogPanelOnBool
            ){
            RevealDialougeOptions(_curentActiveDialog);
        }else{ 
            nextDialogButtonOnBool = true;
            _nextDialogButtonTransform.gameObject.SetActive(true);
            OperateDiodes(4, true, false);
            }

    }

    private void RevealDialougeOptions(Dialogue dialogue)
    {
        OperateDiodes(1, false, false);

        if (dialogue.DialogueOptions.Count > 0){
            if (_dialogueOptionsBox.gameObject.activeInHierarchy == false)
                _dialogueOptionsBox.gameObject.SetActive(true);

            _computerButtonsPanel.gameObject.SetActive(true);
            OperateDiodes(2, true, false);
            optionsAreDisplayedBool = true;
        
        }else{
            Debug.Log("Need to be alter!");
            /*
            if(!menuManager.creditPanelOnBool && !menuManager.informativeBSPanelBool)
            if(_curentActiveDialog.DialogueNext != null){
                nextDialogButtonOnBool = true;
                _nextDialogButtonTransform.gameObject.SetActive(true);
                OperateDiodes(4, true, false);
            }
            */
        }

        
    }
    public void OrderFromInfoBlackScreen(){
        PrepareToUpdateDialogueBox(_curentActiveDialog.DialogueNext);
    }


    public void OrderFromNextDialogueButton(){
        if (_curentActiveDialog.DialogueOptions.Count > 0){
            RevealDialougeOptions(_curentActiveDialog);
        }else{
            OperateDiodes(4, false, false);
            _nextDialogButtonTransform.gameObject.SetActive(false);
            nextDialogButtonOnBool = false;
            PrepareToUpdateDialogueBox(_curentActiveDialog.DialogueNext);
        }
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
        ReputationManager.IncreaseReputationAction(dialogue.reputationIncrease);
        ReputationManager.DecreaseReputationAction(dialogue.reputationDecrease);        
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
        _reputationNumberText.text = number.ToString();
        if(addon != "") _reputationNumberText.text += addon; 

        if(number == 0) if(_reputationNumberText.color !=Color.white)
            _reputationNumberText.color =Color.white;
        if(number > 0) if(_reputationNumberText.color !=Color.green)
            _reputationNumberText.color =Color.green;
        if(number < 0) if(_reputationNumberText.color !=Color.magenta)
            _reputationNumberText.color =Color.magenta;
    }


    

    

    void OperateDiodes(int diodeNumber, bool option, bool clearAllBool){
        if(clearAllBool){
            _diodTyping.gameObject.SetActive(false);
            _diodOptions.gameObject.SetActive(false);
            _diodSpaceBar.gameObject.SetActive(false);
            _diodNextBtn.gameObject.SetActive(false);
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
        }
    }

    void OperateSpace(){
        spaceIsDownBool = true;
        OperateDiodes(3, true, false);
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

}
