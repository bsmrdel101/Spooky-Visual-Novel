using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnicalStepsManager : MonoBehaviour
{

    [Header("Managers")]
    public DialogueManager dialogueManager;

    [Header("References")]
    public Transform technicalStepsPanel;
    public Transform previusDialogueButton, backToLastChoiceButton;
    public Transform previusDialoguePanel, replayDialoguePanel, backToLastChoicePanel;


    public void ButtonTechnicalStepsOnLeft(){
        dialogueManager.technicalStepsPanelOpenBool = true;
        technicalStepsPanel.gameObject.SetActive(true);
        if(dialogueManager._lastActiveDialog != null){
            previusDialogueButton.gameObject.SetActive(true);
        }else{
            previusDialogueButton.gameObject.SetActive(false);
        }
        if(dialogueManager._lastDialogChoice != null){
            backToLastChoiceButton.gameObject.SetActive(true);
        }else{
            backToLastChoiceButton.gameObject.SetActive(false);
        }
        dialogueManager.audioManager.PlayMenuButtonClick(true);
    }

    public void CloseTechnicalStepsOnLeft(){
        dialogueManager.technicalStepsPanelOpenBool = false;
        technicalStepsPanel.gameObject.SetActive(false);
        dialogueManager.audioManager.PlayMenuButtonClick(true);
    }

    public void ButtonPreviusDialogueMain(){
        //previusDialoguePanel.gameObject.SetActive(true);
        dialogueManager.audioManager.PlayMenuButtonClick(true);
        ButtonPreviousDialogueConfirm();
    }

    public void ButtonPreviousDialogueConfirm(){
        Debug.Log("ButtonPreviousDialogueConfirm");
        if(dialogueManager._curentActiveDialog.reputation != 0)
            dialogueManager.reputationManager._reputation -= dialogueManager._curentActiveDialog.reputation;
        
        if(dialogueManager._lastActiveDialog.reputation != 0)
            dialogueManager.reputationManager._reputation -= dialogueManager._lastActiveDialog.reputation;
        
        dialogueManager.PrepareToUpdateDialogueBox(dialogueManager._lastActiveDialog);
        //ClosePreviousDialogue();
        CloseTechnicalStepsOnLeft();
    }

    public void ClosePreviousDialogue(){
        previusDialoguePanel.gameObject.SetActive(false);
        dialogueManager.audioManager.PlayMenuButtonClick(false);
    }

    public void ButtonReplayDialogueMain(){
        //replayDialoguePanel.gameObject.SetActive(true);
        dialogueManager.audioManager.PlayMenuButtonClick(true);
        ButtonReplayDialogueConfirm();
    }

    public void ButtonReplayDialogueConfirm(){
        Debug.Log("ButtonReplayDialogueConfirm");
        if(dialogueManager._curentActiveDialog.reputation != 0){
            dialogueManager.reputationManager._reputation -= dialogueManager._curentActiveDialog.reputation;
        }
        dialogueManager.PrepareToUpdateDialogueBox(dialogueManager._curentActiveDialog);
        //CloseReplayDialogue();
        CloseTechnicalStepsOnLeft();
    }

    public void CloseReplayDialogue(){
        replayDialoguePanel.gameObject.SetActive(false);
        dialogueManager.audioManager.PlayMenuButtonClick(false);
    }

    public void ButtonBackToLastChoiseMain(){
        //backToLastChoicePanel.gameObject.SetActive(true);
        dialogueManager.audioManager.PlayMenuButtonClick(true);
        ButtonBackToLastChoiseConfirm();
    }

    public void ButtonBackToLastChoiseConfirm(){
        Debug.Log("ButtonBackToLastChoiseConfirm");
        dialogueManager.stepBackBool = true;
        dialogueManager.PrepareToUpdateDialogueBox(dialogueManager._lastDialogChoice);
        //CloseBackToLastChoise();
        CloseTechnicalStepsOnLeft();
    }

    public void CloseBackToLastChoise(){
        backToLastChoicePanel.gameObject.SetActive(false);
        dialogueManager.audioManager.PlayMenuButtonClick(false);
    }
}
