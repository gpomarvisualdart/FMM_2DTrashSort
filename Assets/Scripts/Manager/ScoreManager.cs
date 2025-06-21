using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [SerializeField] int currentCoins;
    [SerializeField] int totalCombos;
    [SerializeField] int maxTotalCombos;


    private void Awake()
    {
        if (instance != null) return;
        instance = this;
    }


    private void OnEnable()
    {
        UIManager.instance.ChangeCoins(currentCoins);
    }


    public event Action<int, int> OnAddedTrashEvent;
    public void AddCoins(int value, int trashTypes)
    {
        if (totalCombos > 0)
        {
            currentCoins += value * totalCombos;
        }
        else
        {
            currentCoins += value;
        }

        OnAddedTrashEvent?.Invoke(value, trashTypes);
        UIManager.instance.ChangeCoins(currentCoins);
    }


    public void AddCombo(int v)
    {
        if (totalCombos >= maxTotalCombos) return;
        totalCombos += v;
        if (totalCombos > maxTotalCombos)
        {
            int offset = totalCombos - maxTotalCombos;
            totalCombos -= offset;
        }
    }


    public event Action ComboBreakEvent;
    public void ComboBreak()
    {
        totalCombos = 0;
        ComboBreakEvent?.Invoke();
    }
}
