using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;

    public abstract void TakeDamageMelee(float damage);
    public abstract void TakeDamageDistance(float damage);
    
    public abstract IEnumerator RecoverySequence();

    public bool IsDead()
    {
        return isDead;
    }

    protected static void ReduceEnemyCounter()
    {
        GameManager.NumOfEnemies -= 1;
        UIController.Instance.SetEnemyCounter(GameManager.NumOfEnemies);
        Player.Instance.IncreaseMana(1);
    }
    
    protected static void IncrementEnemyCounter()
    {
        GameManager.NumOfEnemies += 1;
        UIController.Instance.SetEnemyCounter(GameManager.NumOfEnemies);
    }

}
