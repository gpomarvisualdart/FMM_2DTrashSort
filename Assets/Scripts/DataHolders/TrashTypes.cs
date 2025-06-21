using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashTypes : MonoBehaviour
{
    public static readonly Dictionary<int, KeyCode> TrashKeys = new Dictionary<int, KeyCode>()
    {
        {0, KeyCode.Alpha1},
        {1, KeyCode.Alpha2},
        {2, KeyCode.Alpha3},
        {3, KeyCode.Alpha8},
        {4, KeyCode.Alpha9},
        {5, KeyCode.Alpha0}
    };
}
