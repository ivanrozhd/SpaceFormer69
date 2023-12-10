using System;
using UnityEngine;

public class KeypadGate: Gate
{
    [SerializeField] private Keypad keypad;

    private void OnEnable()
    {
        keypad.doorOpened += Open;
    }

    private void OnDisable()
    {
        keypad.doorOpened -= Open;
    }
}