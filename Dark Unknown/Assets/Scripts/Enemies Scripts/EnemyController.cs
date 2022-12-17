using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D customCursor;

    public abstract void TakeDamageMelee(float damage);
    public abstract void TakeDamageDistance(float damage);
    public bool IsDead()
    {
        return isDead;
    }
    
    private void OnMouseEnter()
    {
        Cursor.SetCursor(customCursor, Vector2.zero, CursorMode.Auto);
    }
    
    private void OnMouseExit()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}
