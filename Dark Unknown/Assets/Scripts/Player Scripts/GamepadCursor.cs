using System;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;

public class GamepadCursor : MonoBehaviour
{
    private UnityEngine.InputSystem.PlayerInput _playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private float deadZoneSize = 0.2f;

    private bool _previousMouseState;
    private Mouse _virtualMouse;
    private Mouse _currentMouse;
    //private Vector3 _screenCenter;

    private string _previousControlScheme = "";
    private const string GamepadScheme = "Gamepad";
    private const string MouseScheme = "Keyboard&Mouse";
    
    private void OnEnable()
    {
        _playerInput = InputManager.Instance.playerInput;
        _playerInput.camera = Camera.main;
        _playerInput.uiInputModule = FindObjectOfType<InputSystemUIInputModule>();
        
        _currentMouse = Mouse.current;
        
        //_screenCenter = new Vector3(Screen.width * .5f, Screen.height * .5f + 70f, 0f);
        
        if (_virtualMouse == null)
        {
            _virtualMouse = (Mouse) InputSystem.AddDevice("VirtualMouse");
        }
        else if (!_virtualMouse.added) InputSystem.AddDevice(_virtualMouse);

        InputUser.PerformPairingWithDevice(_virtualMouse, _playerInput.user);

        if (cursorTransform != null)
        {
            var position = cursorTransform.anchoredPosition;
            InputState.Change(_virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        _playerInput.onControlsChanged += OnControlsChanged;
    }

    private void OnDisable()
    {
        if (_virtualMouse != null && _virtualMouse.added) InputSystem.RemoveDevice(_virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        _playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void UpdateMotion()
    {
        if (_virtualMouse == null || Gamepad.current == null) return;
        if (PauseMenu.GameIsPaused) return;
        //var playerPosition = transform.position;
        var playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        var cursorOrigin = new Vector3(playerPosition.x, playerPosition.y + 0.4f, 0f);

        var deltaValue = Gamepad.current.rightStick.ReadValue(); // (x,y)
        if (deltaValue.x >= -deadZoneSize && deltaValue.x <= deadZoneSize && 
            deltaValue.y >= -deadZoneSize && deltaValue.y <= deadZoneSize) return;

        InputState.Change(_virtualMouse.position, Vector2.zero);

        // the cursor origin follows the position of the player
        if (Camera.main == null) return;
        var newPosition = (Vector2)Camera.main.WorldToScreenPoint(cursorOrigin) + 
                          Vector2.ClampMagnitude(deltaValue * 200f, 200f);
        
        InputState.Change(_virtualMouse.position, newPosition);

        var aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if (_previousMouseState != aButtonIsPressed)
        {
            _virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
            InputState.Change(_virtualMouse, mouseState);
            _previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);
    }

    // Synchronizes the position of the image with the actual position of the cursor
    private void AnchorCursor(Vector2 position)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _playerInput.camera, out var anchoredPosition);
        cursorTransform.anchoredPosition = anchoredPosition;
    }

    // Seamlessly transitions from virtual mouse to physical mouse and vice-versa
    private void OnControlsChanged(UnityEngine.InputSystem.PlayerInput input)
    {
        if (PauseMenu.GameIsPaused) return;
        switch (_playerInput.currentControlScheme)
        {
            case MouseScheme when _previousControlScheme != MouseScheme:
                cursorTransform.gameObject.SetActive(false);
                Cursor.visible = true;
                // move physical mouse to virtual mouse position
                _currentMouse.WarpCursorPosition(_virtualMouse.position.ReadValue());
                // reset virtual mouse position to left corner
                InputState.Change(_virtualMouse.position, Vector2.zero);
                AnchorCursor(Vector2.zero);
                _previousControlScheme = MouseScheme;
                break;
            case GamepadScheme when _previousControlScheme != GamepadScheme:
            {
                cursorTransform.gameObject.SetActive(true);
                var mousePosition = _currentMouse.position.ReadValue();
                // move virtual mouse to physical mouse position
                InputState.Change(_virtualMouse.position, mousePosition);
                AnchorCursor(mousePosition);
                // reset physical mouse position to left corner
                _currentMouse.WarpCursorPosition(Vector2.zero);
                Cursor.visible = false;
                _previousControlScheme = GamepadScheme;
                break;
            }
        }
    }
}
