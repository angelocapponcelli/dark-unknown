using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class InputManager : Singleton<InputManager>
{
    [SerializeField] public Keybindings keybindings;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public KeyCode GetKeyForAction(KeybindingActions keybindingAction)
    {
        return (from keybindingCheck in keybindings.KeybindingChecks 
            where keybindingCheck.KeybindingAction == keybindingAction select keybindingCheck.KeyCode).FirstOrDefault();
    }

    public bool SetKeyForAction(KeybindingActions keybindingAction, KeyCode code)
    {
        var tmpIndex = 0;
        for (var i = 0; i < keybindings.KeybindingChecks.Length; i++)
        {
            if (keybindings.KeybindingChecks[i].KeybindingAction == keybindingAction)
            {
                tmpIndex = i;
            }

            if (keybindings.KeybindingChecks[i].KeyCode != code) continue;
            Debug.Log("Key already bound.");
            return false;
        }

        keybindings.KeybindingChecks[tmpIndex].KeyCode = code;
        return true;
    }
    
    // Attempt at a swap keybinding
    /*public string SetKeyForAction(KeybindingActions keybindingAction, KeyCode code)
    {
        var tmpIndex = 0;
        var swapIndex = 0;
        var swapCode = KeyCode.None;
        for (var i = 0; i < keybindings.KeybindingChecks.Length; i++)
        {
            if (keybindings.KeybindingChecks[i].KeybindingAction == keybindingAction)
            {
                tmpIndex = i;
                swapCode = keybindings.KeybindingChecks[i].KeyCode;
            }

            if (keybindings.KeybindingChecks[i].KeyCode == code)
            {
                swapIndex = i;
                
            }
            /*Debug.Log("Key already bound.");
            return false;#1#
        }
        
        keybindings.KeybindingChecks[tmpIndex].KeyCode = code;
        keybindings.KeybindingChecks[swapIndex].KeyCode = swapCode;
        return keybindings.KeybindingChecks[swapIndex].KeybindingAction.ToString();
    }*/

    public bool GetKeyDown(KeybindingActions key)
    {
        return (from keybindingCheck in keybindings.KeybindingChecks
            where keybindingCheck.KeybindingAction == key select Input.GetKeyDown(keybindingCheck.KeyCode)).FirstOrDefault();
    }

    public bool GetKey(KeybindingActions key)
    {
        return (from keybindingCheck in keybindings.KeybindingChecks
            where keybindingCheck.KeybindingAction == key select Input.GetKey(keybindingCheck.KeyCode)).FirstOrDefault();
    }
    public bool GetKeyUp(KeybindingActions key)
    {
        return (from keybindingCheck in keybindings.KeybindingChecks
            where keybindingCheck.KeybindingAction == key select Input.GetKeyUp(keybindingCheck.KeyCode)).FirstOrDefault();
    }

    public int GetAxisRaw(string axis)
    {
        var axisRaw = 0;
        switch (axis)
        {
            case "Horizontal":
                if (GetKey(KeybindingActions.MoveLeft)) axisRaw = -1;
                if (GetKey(KeybindingActions.MoveRight)) axisRaw = +1;
                break;
            case "Vertical":
                if (GetKey(KeybindingActions.MoveUp)) axisRaw = +1;
                if (GetKey(KeybindingActions.MoveDown)) axisRaw = -1;
                break;
        }
        return axisRaw;
    }
}
