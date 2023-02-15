using System.Linq;
using Menu;
using UnityEngine;
using UnityEngine.InputSystem;

// Dynamic, persistent with no duplicates key binding manager
public class InputManager : Singleton<InputManager>
{
    //[SerializeField] public Keybindings keybindings;
    [SerializeField] private InputActionAsset inputActions;
    [HideInInspector] public UnityEngine.InputSystem.PlayerInput playerInput;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        
        playerInput = GetComponent<UnityEngine.InputSystem.PlayerInput>();
    }
    
    public void ResetDefault()
    {
        foreach (var map in inputActions.actionMaps)
        {
            map.RemoveAllBindingOverrides();           
        }
        
        PlayerPrefs.DeleteKey("rebinds");
    }
    
    public void SaveUserRebinds(UnityEngine.InputSystem.PlayerInput player)
    {
        var rebinds = player.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", rebinds);
    }
    
    public void ResetIfNotSaved(UnityEngine.InputSystem.PlayerInput player)
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        player.actions.LoadBindingOverridesFromJson(rebinds);
        
        MenuManager.Instance.UpdateText(player);
    }
}
