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

    private void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        MovementDirection = new Vector2(_x, _y).normalized;
        
        PointerPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        GetLeftClickEvent();
    }

    private void GetLeftClickEvent()
    {
        if (Input.GetMouseButtonDown(0))
            LeftClick?.Invoke();
    }

    public bool GetSpaceDown()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
