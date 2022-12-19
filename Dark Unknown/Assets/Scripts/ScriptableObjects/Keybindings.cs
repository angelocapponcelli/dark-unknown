using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    [System.Serializable]
    public class KeybindingCheck
    {
        public KeybindingActions KeybindingAction;
        public KeyCode KeyCode;
    }

    public KeybindingCheck[] KeybindingChecks;
}
