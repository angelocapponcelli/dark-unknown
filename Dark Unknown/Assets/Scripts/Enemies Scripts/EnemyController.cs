using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;

    public abstract void TakeDamageMelee(float damage);
    public abstract void TakeDamageDistance(float damage);
    public bool IsDead()
    {
        return isDead;
    }
}
