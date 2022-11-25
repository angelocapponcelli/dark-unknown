using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;
    
    public abstract void TakeDamage(float damage);

    public bool IsDead()
    {
        return isDead;
    }
}
