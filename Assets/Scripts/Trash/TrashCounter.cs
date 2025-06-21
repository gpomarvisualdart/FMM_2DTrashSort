using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : MonoBehaviour
{
    [SerializeField] int totalTrash = 0;
    public int GetTotalTrashCount() => totalTrash;
    [SerializeField] int currentTrashRow = 0;
    public int GetCurrentTrashRow() => currentTrashRow;
    [SerializeField] int currentComboMilestone = 0;
    public float GetCurrentComboMilestone() => currentComboMilestone;
    [SerializeField] int trashType;
    public int GetTrashType() => trashType;
    int nextMilestone;


    private void OnEnable()
    {
        nextMilestone = 3 * (1 << currentComboMilestone);
        ScoreManager.instance.OnAddedTrashEvent += OnAddedTrashEventReceiver;
        ScoreManager.instance.ComboBreakEvent += ComboBreakEventReceiver;
    }


    private void OnAddedTrashEventReceiver(int arg1, int arg2)
    {
        if (arg2 != trashType) return;
        //Debug.Log($"{this.name} has received trash!");
        ChangeTrashCount(arg1);
    }


    public void ChangeTrashCount(int i)
    {
        totalTrash += i;
        currentTrashRow += i;

        if (currentTrashRow >= nextMilestone)
        {
            currentComboMilestone++;
            nextMilestone = 3 * (1 << currentComboMilestone);
            currentTrashRow = 0;
            int comboMultiplied = currentComboMilestone + 1;
            Debug.Log($"{this.name} has reached {currentComboMilestone} combo milestone!!");
            ScoreManager.instance.AddCombo(comboMultiplied);
        }
    }


    private void ComboBreakEventReceiver()
    {
        ComboBreak();
    }


    public void ComboBreak()
    {
        currentComboMilestone = 0;
        nextMilestone = 3 * (1 << currentComboMilestone);
        currentTrashRow = 0;
    }
}
