using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }


    public event Action<bool> ActivateGameOverEvent;
    public void ActivateGameOverScreen(bool isActivate)
    {
        ActivateGameOverEvent?.Invoke(isActivate);
    }


    public event Action<int> ChangeMissedTrashValueEvent;
    public void ChangeMissedTrashValue(int value)
    {
        ChangeMissedTrashValueEvent?.Invoke(value);
    }


    public event Action<int> ChangeCoinsEvent;
    public void ChangeCoins(int value)
    {
        ChangeCoinsEvent?.Invoke(value);
    }
}
