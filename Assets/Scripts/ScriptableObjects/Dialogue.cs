using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    public bool actorOnLeftBool= true, actorOnRightBool=false;
    public string speakerName;
    public Sprite speakerOneNewImage, speakerTwoNewImage;
    public Color speakerColor = new Color(147, 61, 95, 255);
    [TextArea(10, 100)] public string BodyText;
    public int reputationIncrease = 0;
    public int reputationDecrease = 0;

    public List<DialogueOption> DialogueOptions;

    [Tooltip("Blocks the dialogue options below from being chosen in the future")]
    public List<Dialogue> DialogueBlockers;

    public AudioClip voiceActorAudioClip;
    public AudioClip soundEffectAudioClip;
    public AudioClip musicAudioClip;

    public StoryScene redirectionOnStoryScene;
    public bool endingTheStoryBool; public int endigNumber=0;
}

[System.Serializable]
public class DialogueOption
{
    [SerializeField]
    public string OptionName;

    [SerializeField]
    public Dialogue Dialogue;
}
