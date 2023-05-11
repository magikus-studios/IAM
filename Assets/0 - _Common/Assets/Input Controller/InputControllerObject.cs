using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable] public class InputControllerObject
{
    public KeyCode primaryKey;
    public KeyCode secondaryKey;
    public UnityEvent OnKeyDown;
    public UnityEvent OnKeyUp;
    public UnityEvent OnKey;

    public void CheckKey() 
    {
        if (Input.GetKeyDown(primaryKey) || Input.GetKeyDown(secondaryKey)) { OnKeyDown?.Invoke(); } 
        else if (Input.GetKey(primaryKey) || Input.GetKey(secondaryKey)) { OnKey?.Invoke(); } 
        else if (Input.GetKeyUp(primaryKey) || Input.GetKeyUp(secondaryKey)) { OnKeyUp?.Invoke(); } 
    }
}
