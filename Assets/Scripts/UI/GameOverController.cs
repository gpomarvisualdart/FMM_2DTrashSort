using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    [SerializeField] GameObject overallUI;

    private void OnEnable()
    {
        UIManager.instance.ActivateGameOverEvent += ActivateGameOverEventReceiver;
    }


    private void ActivateGameOverEventReceiver(bool obj)
    {
        overallUI.gameObject.SetActive(obj);
    }
}
