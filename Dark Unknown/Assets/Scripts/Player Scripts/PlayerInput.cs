using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _x, _y;
    public Vector2 MovementDirection { get; private set; }
    public Vector2 PointerPosition { get; private set; }
    public event Action LeftClick;
    
    // first gamepad implementation - always active, key binds immutable
    private UnityEngine.InputSystem.PlayerInput _controls; 

    private void Awake()
    {
        _controls = InputManager.Instance.playerInput; // Gameplay
        _controls.actions["Move"].performed += ctx => MovementDirection = ctx.ReadValue<Vector2>();
        _controls.actions["Move"].canceled += ctx => MovementDirection = Vector2.zero;

        _controls.actions["AttackDirection"].performed += ctx =>
        {
            if (Camera.main != null)
                PointerPosition = Camera.main.ScreenToWorldPoint(
                    ctx.ReadValue<Vector2>());
        };
        //_controls.Gameplay.AttackDirection.canceled += ctx => MovementDirection = Vector2.zero;
    }

    private void Update()
    {
        GetLeftClickEvent();
    }

    private void GetLeftClickEvent()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick?.Invoke();
    }
    
    /*private void OnEnable()
    {
        if (_controls.currentActionMap.ToString() == "Gameplay") _controls.currentActionMap.Enable();
    }*/

    /*private void OnDisable()
    {
        if (_controls.currentActionMap.ToString() == "Gameplay") _controls.currentActionMap.Disable();
    }*/
}
