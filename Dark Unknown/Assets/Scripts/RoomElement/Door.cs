using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

//create a new type: SymbolType
[System.Serializable]
public class SymbolType
{
    //define all of the values for the class
    [SerializeField] public RoomLogic.Type type;
    [SerializeField] public Sprite sprite;

    //define a constructor for the class
    public SymbolType(RoomLogic.Type newTypeitem, Sprite newSpriteItem)
    {
        type = newTypeitem;
        sprite = newSpriteItem;
    }
}

public class Door : MonoBehaviour
{
    [Range(1, 3)]
    public int myIndex;
    private SpriteRenderer _mySpriteRender;

    [Header ("Symbols")]
    [SerializeField] private SpriteRenderer _symbolAboveDoor;    

    private SymbolType _actualDoorSymbol;
    private Animator _animator;
    private BoxCollider2D _myBoxCollider;
    
    private static readonly int Opening = Animator.StringToHash("Opening");

    private bool _canOpen = false;

    private UnityEngine.InputSystem.PlayerInput _playerControls;

    //TODO Change start in awake
    private void Start()
    {
        _myBoxCollider = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();
        _myBoxCollider.enabled = false;
        _playerControls = InputManager.Instance.playerInput;
    }

    private void Update()
    {
        if (_canOpen && _playerControls.actions["Interact"].WasPressedThisFrame())
        {
            AudioManager.Instance.PlayEnterDoorSound();
            _myBoxCollider.enabled = false;
            LevelManager.Instance.SetNewRoom(myIndex, _actualDoorSymbol.type);
            Player.Instance.SetTargetIndicatorActive(false);
        }
    }

    public void SetSymbol(SymbolType symbolType)
    {
        _actualDoorSymbol = symbolType;
        _symbolAboveDoor.sprite = _actualDoorSymbol.sprite;
    }
    
    public void Open()
    {
        //_isClose = false;
        _animator.SetTrigger(Opening);
        //_mySpriteRender.sprite = _openedDoorSprite;
        _myBoxCollider.enabled = true;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        var text = "";
        if (InputManager.Instance.playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            text = InputManager.Instance.playerInput.actions["Interact"].bindings[0].ToDisplayString();
        }
        else if (InputManager.Instance.playerInput.currentControlScheme == "Gamepad")
        {
            text = "A";
        }
        if (col.CompareTag("Player"))
        {
            //set room colliders to false
            //useful mainly because we can't delete the serialized room from the GameManager
            //TODO: could do a room just for it with a special class, ...
            Player.Instance.ShowPlayerUI(true, "Press " + text + " to enter the door");
            _canOpen = true;
        }
    }
    public void OnTriggerExit2D (Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            _canOpen = false;
        }
    }
}
