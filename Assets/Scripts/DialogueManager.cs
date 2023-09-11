using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    [Header("Scene")]
    public StoryScene ActiveStoryScene;

    [Header("Dialogue")]
    [SerializeField] private float _textReadDelay = 0.01f;
    private bool stopReadingText = false;
    [SerializeField] private List<Dialogue> _blockedDialogue = new List<Dialogue>();

    [Header("References")]
    [SerializeField] private Image _speakerSprite;
    [SerializeField] private Image _bgImage;
    [SerializeField] private TextMeshProUGUI _speakerName;
    [SerializeField] private TextMeshProUGUI _dialogueBodyText;
    [SerializeField] private Transform _dialogueOptionsBox;
    [SerializeField] private GameObject _buttonPrefab;


    private void Start()
    {
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
        ResetDialogueOptions();
        HandleDialogueBlocking(dialogue.DialogueBlockers);

        // Set the speaker and text body values for the dialogue box
        ChangeNpcImage(dialogue.SpeakerImage);
        _speakerName.text = dialogue.Speaker;
        _speakerName.color = dialogue.SpeakerColor;
        StartCoroutine(ReadText(_dialogueBodyText, dialogue.BodyText, dialogue));

        // Renders the dialogue options
        foreach (DialogueOption option in dialogue.DialogueOptions)
        {
            if (!_blockedDialogue.Contains(option.Dialogue))
            {
                GameObject buttonObj = Instantiate(_buttonPrefab, _dialogueOptionsBox);
                Button button = buttonObj.GetComponent<Button>();
                buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = option.OptionName;
                button.onClick.AddListener(() => OnClickSelectDialogueOption(option.Dialogue));
            }
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
        _speakerSprite.sprite = image;
    }

    private void ChangeBackgroundImage(Sprite bgImage)
    {
        _bgImage.sprite = bgImage;
    }

    // Creates the typing effect for text
    private IEnumerator ReadText(TextMeshProUGUI element, string textContent, Dialogue dialogue)
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
            _dialogueOptionsBox.gameObject.SetActive(true);
    }

    private void OnClickSelectDialogueOption(Dialogue dialogue)
    {
        _dialogueOptionsBox.gameObject.SetActive(false);
        UpdateDialogueBox(dialogue);
    }

    private void HandleDialogueBlocking(List<Dialogue> dialogueList)
    {
        foreach (Dialogue dialogue in dialogueList)
        {
            _blockedDialogue.Add(dialogue);
        }
    }
}
