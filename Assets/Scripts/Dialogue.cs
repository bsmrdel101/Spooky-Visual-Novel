using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif


//[CreateAssetMenu(fileName = "Dialogue", menuName = "Assets/Dialogue", order = 3)]
public class Dialogue : MonoBehaviour
{
    [Header("Characters")]
    [Tooltip("Means whos character data will alter how dialog window look.")]
    public int characterInUse=0;
    
    public CharacterSheet leftCharacterSheet;
    public bool characterOnLeftWhiteBool = true;
    public int  characterOnLeftSpriteNumber = 0;
    public bool actorOnLeftAppearBool, actorOnLeftDisapearBool, actorOnLeftAlpha0Bool;
    public bool editLeftActorOnEndBool;
    
    
    public CharacterSheet rightCharacterSheet;
    public bool characterOnRightWhiteBool=false;
    public int  characterOnRightSpriteNumber = 0;
    public bool actorOnRightAppearBool, actorOnRightDisapearBool, actorOnRightAlpha0Bool;
    public bool editRightActorOnEndBool;


    public CharacterSheet middleCharacterSheet;
    public bool characterOnMiddleWhiteBool=false;
    public int  characterOnMiddleSpriteNumber = 0;
    public bool actorOnMiddleAppearBool, actorOnMiddleDisapearBool, actorOnMiddleAlpha0Bool;
    public bool editMiddleActorOnEndBool;

    
    [Header("Scene")]
    [TextArea(10, 100)] public string BodyText;
    public string overwriteNameText="";

    //public int reputationIncrease = 0;
    //public int reputationDecrease = 0;
    [SerializeField] public int reputation;

    public Dialogue DialogueNext;

    [Tooltip("Redirectino put on new dialog step. For now you must create an empty redirection.")]
    public StoryScene redirectionOnStoryScene;

    [Tooltip("If Dialougue Next empty. Display Options")]
    public List<DialogueOption> DialogueOptions;
    [Tooltip("Blocks the dialogue options below from being chosen in the future")]
    public List<Dialogue> DialogueBlockers;
    public DialogueOption DialogueWhenNoneAviable;

    [Tooltip("Put a information on a black screen.")]
    public bool informativeBSBool, placeMistUnderInformativeBool;
    public Sprite informativeSuportSprite;
    [Tooltip("Use the Computer screen dialog. No Actors!")]
    public bool computerDialogBool;
   



    [Header("Audio")]
    [Tooltip("If set it will be played.")]
    public AudioClip voiceActorAudioClip;
    [Tooltip("If Sound Effect set it will dellay other things.")]
    public AudioClip soundEffectAudioClip;
    public AudioClip soundEffectAtEndClip;

    
    [Header("Scene Operators")]
    public Sprite newSpriteForScene=null;
    public AudioClip musicAudioClip;
    [Tooltip("Put ending from Credits on new dialog step")]
    public bool endingTheStoryBool; public int endigNumber=0;

    [Header("Item")]
    public Sprite ItemNewSprite;
    [Tooltip("Movable item on screen. Apply if any is true")]
    public bool itemPlaceLeft, itemPlaceRight, itemMoveBool;
    [Tooltip("-1300(left) and 1300(right) shall be safe outside of the screen. -300 and 300 shall be a good bets to move the items.")]
    public int itemNewCordinates; 


    [Header("Theory about step back")]
    public Sprite lastBacgroundSprite;
    public AudioClip lastMusicClip;


    



}





[System.Serializable]
public class DialogueOption
{
    [SerializeField]     public string OptionName;
    [SerializeField]     public Dialogue Dialogue;
    [SerializeField]    public bool reputationLimiterBool;
    [SerializeField]    public int requiredValue;
    [SerializeField]    public bool mustBeMoreBool;
    [SerializeField]    public bool mustBeEqualBool;
    [SerializeField]    public bool mustBeLessBool;
}
