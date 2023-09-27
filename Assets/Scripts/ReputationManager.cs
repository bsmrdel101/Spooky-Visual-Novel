using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationManager : MonoBehaviour
{
    [Header("Actions")]
    public static Action<int> DecreaseReputationAction;
    public static Action<int> IncreaseReputationAction;

    [Header("Reputation System")]
    [SerializeField] public int _reputation = 0;
    private string addonString="";
    
    [Header("Managers")]
    public MenuPanelManager menuManager;


    private void OnEnable()
    {
        DecreaseReputationAction += DecreaseReputation;
        IncreaseReputationAction += IncreaseReputation;
    }

    private void OnDisable()
    {
        DecreaseReputationAction -= DecreaseReputation;
        IncreaseReputationAction -= IncreaseReputation;
    }

    private void DecreaseReputation(int amount)
    {
        if(amount != 0){
            _reputation -= amount;
            addonString = "";
            addonString = "(-"+amount+")";
            menuManager.dialogueManager.SetReputation(_reputation, addonString);
            menuManager.audioManager.PlayReputationSound(false);
        }
    }

    private void IncreaseReputation(int amount)
    {
        if(amount != 0){
            _reputation += amount;
            addonString = "";
            addonString = "(+"+amount+")";
            menuManager.dialogueManager.SetReputation(_reputation, addonString);
            menuManager.audioManager.PlayReputationSound(true);
        }
    }
}
