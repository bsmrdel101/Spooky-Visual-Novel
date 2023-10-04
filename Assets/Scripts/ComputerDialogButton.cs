using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComputerDialogButton : MonoBehaviour
{
    public TMP_Text myText;
    public int myId=0;
    public Dialogue myDialougue;
    public DialogueManager dialogueManager;


    public void ButtonClick(){
        dialogueManager.OnClickSelectDialogueOption(myDialougue, true);
    }

    public void SetThisButon(string newString, Dialogue newDialogue){
        myDialougue = newDialogue;
        myText.text = newString;
    }
}
