using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DialogueManager : MonoBehaviour
{
    [Header("Actions")]
    public static Action<StoryScene> ChangeStorySceneAction;

    [Header("Scene")]
    public StoryScene ActiveStoryScene;
    private int _dialogueIndex = 0;

    [Header("References")]
    [SerializeField] private TMP_Text _speakerText;
    [SerializeField] private TMP_Text _bodyText;


    private void OnEnable()
    {
        ChangeStorySceneAction += ChangeStoryScene;
    }

    private void OnDisable()
    {
        ChangeStorySceneAction -= ChangeStoryScene;
    }

    private void Start()
    {
        ChangeStoryScene(ActiveStoryScene);
    }

    private void ChangeStoryScene(StoryScene storyScene)
    {
        UpdateDialogueBox(storyScene.DialogueList[_dialogueIndex]);
    }

    private void UpdateDialogueBox(Dialogue dialogue)
    {
        _speakerText.text = dialogue.Speaker;
        _bodyText.text = dialogue.BodyText;
    }
}
