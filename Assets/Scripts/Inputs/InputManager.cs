using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public event EventHandler<KeyDownEventArgs> KeyDownEvent;
    public class KeyDownEventArgs : EventArgs { public KeyCode pressedKey; }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in TrashCanKeys.TrashCanFixedKeys)
            {
                if (Input.GetKeyDown(key))
                {
                    KeyDownEvent?.Invoke(this, new KeyDownEventArgs { pressedKey = key });
                    break;
                }
            }
        }
    }
}
