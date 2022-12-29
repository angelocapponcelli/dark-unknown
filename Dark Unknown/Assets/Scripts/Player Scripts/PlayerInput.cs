using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float _x, _y;
    public Vector2 MovementDirection { get; private set; }
    public Vector2 PointerPosition { get; private set; }
    public event Action LeftClick;
    public event Action RightClick;

    private void Update()
    {
        _x = InputManager.Instance.GetAxisRaw("Horizontal");
        _y = InputManager.Instance.GetAxisRaw("Vertical");
        MovementDirection = new Vector2(_x, _y).normalized;
        
        PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        GetLeftClickEvent();
        GetRightClickEvent();
    }

    private void GetLeftClickEvent()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick?.Invoke();
    }
    
    private void GetRightClickEvent()
    {
        if (Input.GetMouseButtonDown(1))
            RightClick?.Invoke();
    }

    public static bool GetDashKeyDown()
    {
        return InputManager.Instance.GetKeyDown(KeybindingActions.Dash);
    }
}
