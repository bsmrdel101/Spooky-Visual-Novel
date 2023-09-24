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
        _reputation -= amount;
        menuManager.audioManager.PlayReputationSound(false);
    }

    private void IncreaseReputation(int amount)
    {
        _reputation += amount;
        menuManager.audioManager.PlayReputationSound(true);
    }
}
