using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DialogueManager : MonoBehaviour
{
    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    [SerializeField] private float _textReadDelay = 0.01f;
    private int _dialogueIndex = 0;
    private Label _dialogueBoxSpeaker;
    private TextElement _dialogueBoxBodyText;
    private Box _dialogueOptionsBox;

    [Header("References")]
    [SerializeField] private UIDocument _dialogueBoxDoc;


    private void Start()
    {
        // Get references to UI elements
        _dialogueBoxBodyText = _dialogueBoxDoc.rootVisualElement.Q<TextElement>("dialogue-body-text");
        _dialogueBoxSpeaker = _dialogueBoxDoc.rootVisualElement.Q<Label>("dialogue-speaker");
        _dialogueOptionsBox = _dialogueBoxDoc.rootVisualElement.Q<Box>("dialogue-options");

        ChangeStoryScene(ActiveStoryScene);
    }

    private void ChangeStoryScene(StoryScene storyScene)
    {
        UpdateDialogueBox(storyScene.DialogueList[_dialogueIndex]);
    }

    private void UpdateDialogueBox(Dialogue dialogue)
    {
        // Set the speaker and body text for the dialogue box
        _dialogueBoxSpeaker.text = dialogue.Speaker;
        _dialogueBoxSpeaker.style.color = dialogue.SpeakerColor;
        StartCoroutine(ReadText(_dialogueBoxBodyText, dialogue.BodyText, dialogue));

        // Renders the dialogue options
        foreach (Dialogue option in dialogue.DialogueOptions)
        {
            Button button = new Button();
            button.text = option.name;
            button.AddToClassList("dialogue-options__choice");
            button.RegisterCallback<ClickEvent>((e) => SelectDialogueOption(option));
            _dialogueOptionsBox.Add(button);
        }
    }

    // Creates the typing effect for text
    private IEnumerator ReadText(TextElement element, string textContent, Dialogue dialogue)
	{
        element.text = "";
		foreach (char c in textContent) 
		{
			element.text += c;
			yield return new WaitForSeconds(_textReadDelay);
		}

        // Makes the dialogue options visible if they exist
        if (dialogue.DialogueOptions.Length > 0)
            _dialogueOptionsBox.RemoveFromClassList("hidden");
	}

    private void SelectDialogueOption(Dialogue dialogue)
    {
        _dialogueOptionsBox.Clear();
        _dialogueOptionsBox.AddToClassList("hidden");
        UpdateDialogueBox(dialogue);
    }
}
