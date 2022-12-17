using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    private Vector2 _cursorHotspot;
    
    public void setPlayCursor()
    {
        _cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(cursor, _cursorHotspot, CursorMode.Auto);
    }
    
    public void setMenuCursor()
    {
        _cursorHotspot = new Vector2(cursor.width / 2, cursor.height / 2);
        Cursor.SetCursor(null, _cursorHotspot, CursorMode.Auto);
    }
}
