using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinUIController : MonoBehaviour
{
    [SerializeField] GameObject overallUI;
    [SerializeField] TextMeshProUGUI txt;
    private void OnEnable()
    {
        UIManager.instance.ChangeCoinsEvent += ChangeCoinsEventReceiver;       
    }


    private void ChangeCoinsEventReceiver(int obj)
    {
        txt.text = obj.ToString();
    }
}
