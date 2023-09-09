using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueManager : MonoBehaviour
{
    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    private Label _dialogueBoxSpeaker;
    private TextElement _dialogueBoxBodyText;
    private Box _dialogueOptionsBox;
    private Image _npcSprite;

    [Header("Dialogue")]
    [SerializeField] private float _textReadDelay = 0.01f;
    private bool stopReadingText = false;

    [Header("References")]
    [SerializeField] private UIDocument _dialogueBoxDoc;


    private void Start()
    {
        // Get references to UI elements
        _dialogueBoxBodyText = _dialogueBoxDoc.rootVisualElement.Q<TextElement>("dialogue-body-text");
        _dialogueBoxSpeaker = _dialogueBoxDoc.rootVisualElement.Q<Label>("dialogue-speaker");
        _dialogueOptionsBox = _dialogueBoxDoc.rootVisualElement.Q<Box>("dialogue-options");
        _npcSprite = _dialogueBoxDoc.rootVisualElement.Q<Image>("npc-sprite");

        ChangeStoryScene(ActiveStoryScene);
    }

    private void Update()
    {
        // Stop text from reading if player presses spacebar
        if (Input.GetKeyDown(KeyCode.Space)) stopReadingText = true;
    }

    private void ChangeStoryScene(StoryScene storyScene)
    {
        ChangeBackgroundImage(storyScene.BackgroundImage);
        UpdateDialogueBox(storyScene.StartingDialogue);
    }

    private void UpdateDialogueBox(Dialogue dialogue)
    {
        // Set the speaker and text body values for the dialogue box
        ChangeNpcImage(dialogue.SpeakerImage);
        _dialogueBoxSpeaker.text = dialogue.Speaker;
        _dialogueBoxSpeaker.style.color = dialogue.SpeakerColor;
        StartCoroutine(ReadText(_dialogueBoxBodyText, dialogue.BodyText, dialogue));

        // Renders the dialogue options
        foreach (DialogueOption option in dialogue.DialogueOptions)
        {
            Button button = new Button();
            button.text = option.OptionName;
            button.AddToClassList("dialogue-options__choice");
            button.RegisterCallback<ClickEvent>((e) => SelectDialogueOption(option.Dialogue));
            _dialogueOptionsBox.Add(button);
        }
    }

    private void ChangeNpcImage(Sprite image)
    {
        _npcSprite.style.backgroundImage = new StyleBackground(image);
    }

    private void ChangeBackgroundImage(Sprite bgImage)
    {
        _dialogueBoxDoc.rootVisualElement.style.backgroundImage = new StyleBackground(bgImage);
    }

    // Creates the typing effect for text
    private IEnumerator ReadText(TextElement element, string textContent, Dialogue dialogue)
	{
        stopReadingText = false;
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

        // Makes the dialogue options visible if they exist
        RevealDialougeOptions(dialogue);
	}

    private void RevealDialougeOptions(Dialogue dialogue)
    {
        if (dialogue.DialogueOptions.Count > 0)
            _dialogueOptionsBox.RemoveFromClassList("hidden");
    }

    private void SelectDialogueOption(Dialogue dialogue)
    {
        _dialogueOptionsBox.Clear();
        _dialogueOptionsBox.AddToClassList("hidden");
        UpdateDialogueBox(dialogue);
    }
}
