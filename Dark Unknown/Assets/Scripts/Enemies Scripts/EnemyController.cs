using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;
    protected bool _damageFromDistance;
    
    public abstract void TakeDamage(float damage, bool source);

    public bool IsDead()
    {
        return isDead;
    }
}
