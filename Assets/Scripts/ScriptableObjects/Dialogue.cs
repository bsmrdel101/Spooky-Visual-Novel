using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 2)]
public class Dialogue : ScriptableObject
{
    [Tooltip("Whitch one will be White and get new image, or become gray.")]
    public bool actorOnLeftBool= true, actorOnRightBool=false;
    public string speakerName;
    [Tooltip("Every time actor is true, the actor Image will be replaced whit replaced by new image.")]
    public Sprite speakerOneNewImage, speakerTwoNewImage;
    public Color speakerColor = new Color(147, 61, 95, 255);
    [TextArea(10, 100)] public string BodyText;
    public int reputationIncrease = 0;
    public int reputationDecrease = 0;
    

    public List<DialogueOption> DialogueOptions;

    [Tooltip("Blocks the dialogue options below from being chosen in the future")]
    public List<Dialogue> DialogueBlockers;
    public DialogueOption DialogueWhenNoneAviable;

    [Tooltip("If set it will be played.")]
    public AudioClip voiceActorAudioClip;
    [Tooltip("If set it will dellay other things.")]
    public AudioClip soundEffectAudioClip;
    public AudioClip musicAudioClip;

    [Tooltip("Put a information on a black screen.")]
    public bool informativeBSBol;
    public Dialogue informativeLeadsToDialogue; 
    public Sprite informativeSuportSprite;
    

    [Tooltip("Redirectino put on new dialog step.")]
    public StoryScene redirectionOnStoryScene;
    [Tooltip("Put ending from Credits on new dialog step")]
    public bool endingTheStoryBool; public int endigNumber=0;

    public Sprite ItemNewSprite;
    [Tooltip("Movable item on screen. Apply if any is true")]
    public bool itemPlaceLeft, itemPlaceRight, itemMoveBool;
    [Tooltip("-1300(left) and 1300(right) shall be safe outside of the screen. -300 and 300 shall be a good bets to move the items.")]
    public int itemNewCordinates; 

    [Tooltip("Use the Computer screen dialog. No Actors!")]
    public bool computerDialogBool;


    [Tooltip("Dont use it! It have an issues.")]
    public bool actorOnLeftAppearBool, actorOnLeftDisapearBool, 
        actorOnRightAppearBool, actorOnRightDisapearBool;


}

[System.Serializable]
public class DialogueOption
{
    [SerializeField]
    public string OptionName;

    [SerializeField]
    public Dialogue Dialogue;
}
