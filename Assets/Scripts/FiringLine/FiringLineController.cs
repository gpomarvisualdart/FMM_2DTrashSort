using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiringLineController : MonoBehaviour
{
    InputManager inpManager;
    Vector2 overlapSize = new Vector2(10.6f, 0.2f);
    LayerMask trashLayer;


    private void OnEnable()
    {
        inpManager = FindAnyObjectByType<InputManager>();
        if (inpManager != null)
        {
            inpManager.KeyDownEvent += KeyDownEventReceiver;
        }
        trashLayer = LayerMask.GetMask("Trashes");
    }


    private void KeyDownEventReceiver(object sender, InputManager.KeyDownEventArgs e)
    {
        CheckTrashInLine(e.pressedKey);
    }
    

    private void CheckTrashInLine(KeyCode pressedKey)
    {
        Collider2D[] colliders = new Collider2D[8];
        int overlaps = Physics2D.OverlapBoxNonAlloc(transform.position, overlapSize, transform.eulerAngles.z, colliders, trashLayer);
        Debug.Log(overlaps);

        if (overlaps < 1) return;
        TrashController fallBackFalse = null;
        foreach(Collider2D col in colliders)
        {
            if (col == null) continue;
            TrashController tC = col.transform.TryGetComponent(out TrashController t) ? t : null;
            if (tC == null) continue;
            if (tC.GetCurrentKeyCode() == pressedKey) { tC.ActivateSort(true); return; }
            else { fallBackFalse = tC; }
        }

        if (fallBackFalse == null) return;
        fallBackFalse.ActivateSort(false);
        fallBackFalse = null;
    }
}
