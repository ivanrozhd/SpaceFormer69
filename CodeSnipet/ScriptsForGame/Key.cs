using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractable
{
    private Color _originalColor;
    private Keypad _keypad;
    private bool _set = false;
    private void Awake()
    {
        _originalColor = GetComponentInChildren<MeshRenderer>().material.color;
        _keypad = GetComponentInParent<Keypad>();
    }

    public void Interact()
    {
        if (!_set)
        {
            int id = _keypad.FindKey(this);
            _keypad.InputKey(id);
        }
    }

    public void WrongKey()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        SetSet(true);
    }
    
    public void RightKey()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        SetSet(true);
    }

    public void ResetKey()
    {
        GetComponentInChildren<MeshRenderer>().material.color = _originalColor;
        SetSet(false);
    }

    public void SetSet(bool value)
    {
        _set = value;
    }
}
