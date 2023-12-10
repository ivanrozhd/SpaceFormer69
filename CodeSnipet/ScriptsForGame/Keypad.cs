using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private List<int> code;
    [SerializeField] private List<Key> keys;

    private List<int> _currentInput = new List<int>();
    private int _index = 0;

    public event EventHandler doorOpened;

    public void InputKey(int key_id)
    {
        if (key_id <= -1 || key_id >= keys.Count) return;
        
        _currentInput.Add(key_id);  
        
        if (CheckInput())
        {
            CorrectInput(key_id);
        }
        else
        {
            StartCoroutine(ResetInput());
        }
        
    }

    private bool CheckInput()
    {
        return _currentInput[_index] == code[_index];
    }

    public void CorrectInput(int key_id)
    {
        keys[key_id].RightKey();
        _index++;
        if (_index == code.Count)
            Activate();
    }

    private IEnumerator ResetInput()
    {
        _currentInput.Clear();
        _index = 0;
        
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].WrongKey();
        }

        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < keys.Count; i++)
        {
            keys[i].ResetKey();
        }
    }

    public int FindKey(Key key)
    {
        if (keys.Contains(key))
            return keys.IndexOf(key);
        
        return -1;
    }

    public void Activate()
    {
        OnDoorOpened(EventArgs.Empty);
    }

    public virtual void OnDoorOpened(EventArgs e)
    {
        EventHandler handler = doorOpened;
        handler?.Invoke(this, e);
    }
}
