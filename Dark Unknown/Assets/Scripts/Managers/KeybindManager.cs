using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class KeybindManager : Singleton<KeybindManager>
{
    private InputManager _inputManager;
    [SerializeField] private Keybindings savedKeybindings;

    public Text up, down, left, right, dash, potion, spell1, spell2;
    private string _bindName;
    private GameObject _currentKey;
    private GameObject _swappedKey;
    private GameObject[] _keybindingButtons;
    private readonly Color32 _normal = new Color32(12, 37, 63, 255);
    private readonly Color32 _selected = new Color32(12, 18, 32, 255);

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _inputManager = InputManager.Instance;

        _inputManager.SetKeyForAction(KeybindingActions.MoveUp, (KeyCode) System.Enum.Parse(typeof(KeyCode), 
            PlayerPrefs.GetString("MoveUp", "W")));
        _inputManager.SetKeyForAction(KeybindingActions.MoveDown, (KeyCode) System.Enum.Parse(typeof(KeyCode), 
            PlayerPrefs.GetString("MoveDown", "S")));
        _inputManager.SetKeyForAction(KeybindingActions.MoveLeft, (KeyCode) System.Enum.Parse(typeof(KeyCode), 
            PlayerPrefs.GetString("MoveLeft", "A")));
        _inputManager.SetKeyForAction(KeybindingActions.MoveRight, (KeyCode) System.Enum.Parse(typeof(KeyCode), 
            PlayerPrefs.GetString("MoveRight", "D")));
        _inputManager.SetKeyForAction(KeybindingActions.Dash, (KeyCode) System.Enum.Parse(typeof(KeyCode), 
            PlayerPrefs.GetString("Dash", "LeftShift")));
        
        UpdateKeyText();
        SaveKeybindingsArray();
    }

    private void UpdateKeyText()
    {
        up.text = _inputManager.GetKeyForAction(KeybindingActions.MoveUp).ToString();
        down.text = _inputManager.GetKeyForAction(KeybindingActions.MoveDown).ToString();
        left.text = _inputManager.GetKeyForAction(KeybindingActions.MoveLeft).ToString();
        right.text = _inputManager.GetKeyForAction(KeybindingActions.MoveRight).ToString();
        dash.text = _inputManager.GetKeyForAction(KeybindingActions.Dash).ToString();
        //TODO:Add remaining cases
    }
    
    public void ResetDefault()
    {
        _inputManager.SetKeyForAction(KeybindingActions.MoveUp, KeyCode.W);
        _inputManager.SetKeyForAction(KeybindingActions.MoveDown, KeyCode.S);
        _inputManager.SetKeyForAction(KeybindingActions.MoveLeft, KeyCode.A);
        _inputManager.SetKeyForAction(KeybindingActions.MoveRight, KeyCode.D);
        _inputManager.SetKeyForAction(KeybindingActions.Dash, KeyCode.LeftShift);
        
        UpdateKeyText();
    }

    private static KeybindingActions ToKeybindingActions(string keyName)
    {
        var keybindingAction = KeybindingActions.MoveUp;
        switch (keyName)
        {
            case "MoveUp":
                break;
            case "MoveDown":
                keybindingAction = KeybindingActions.MoveDown;
                break;
            case "MoveLeft":
                keybindingAction = KeybindingActions.MoveLeft;
                break;
            case "MoveRight":
                keybindingAction = KeybindingActions.MoveRight;
                break;
            case "Dash":
                keybindingAction = KeybindingActions.Dash;
                break;
            //TODO:add remaining cases
        }

        return keybindingAction;
    }
    
    private void OnGUI()
    {
        if (_currentKey == null) return;
        var e = Event.current;
        if (!e.isKey) return;
        var tmpAction = ToKeybindingActions(_currentKey.name);
        /* TODO: swap of keybindings
         _keybindingButtons = GameObject.FindGameObjectsWithTag("KeyBind");
        var swapCode = _currentKey.GetComponentInChildren<Text>().text;
        var swapAction = _inputManager.SetKeyForAction(tmpAction, e.keyCode);
        foreach (var keybinding in _keybindingButtons)
        {
            if (keybinding.name != swapAction) continue;
            _swappedKey = keybinding;
            break;
        }
        _currentKey.GetComponentInChildren<Text>().text = e.keyCode.ToString();
        _swappedKey.GetComponentInChildren<Text>().text = swapCode;*/
        if(_inputManager.SetKeyForAction(tmpAction, e.keyCode))
        {
            _currentKey.GetComponentInChildren<Text>().text = e.keyCode.ToString();
        }
        _currentKey.GetComponent<Image>().color = _normal;
        _currentKey = null;
        MenuManager.IsChangingKey = false;
    }

    public void ChangeKey(GameObject clicked)
    {
        if (_currentKey != null)
        {
            _currentKey.GetComponent<Image>().color = _normal;
            MenuManager.IsChangingKey = false;
        }
        _currentKey = clicked;
        MenuManager.IsChangingKey = true;
        _currentKey.GetComponent<Image>().color = _selected;
        //_currentKey.GetComponentInChildren<Text>().text = "";
    }

    public void SaveKeys()
    {
        foreach (var key in _inputManager.keybindings.KeybindingChecks)
        {
            PlayerPrefs.SetString(key.KeybindingAction.ToString(), key.KeyCode.ToString());
        }
        
        PlayerPrefs.Save();
        SaveKeybindingsArray();
    }

    private void SaveKeybindingsArray()
    {
        for (var i=0; i< _inputManager.keybindings.KeybindingChecks.Length; i++)
        {
            //savedKeybindings.KeybindingChecks[i].KeybindingAction =
                //_inputManager.keybindings.KeybindingChecks[i].KeybindingAction;
            savedKeybindings.KeybindingChecks[i].KeyCode =
                _inputManager.keybindings.KeybindingChecks[i].KeyCode;
        }
    }

    public void ResetIfNotSaved()
    {
        for (var i = 0; i < savedKeybindings.KeybindingChecks.Length; i++)
        {
            _inputManager.keybindings.KeybindingChecks[i].KeyCode = savedKeybindings.KeybindingChecks[i].KeyCode;
        }
        UpdateKeyText();
    }
}
