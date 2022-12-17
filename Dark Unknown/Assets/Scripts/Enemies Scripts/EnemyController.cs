using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;
<<<<<<< HEAD
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D customCursor;
=======
    protected bool _damageFromDistance;
    
    public abstract void TakeDamage(float damage, bool source);
    //public abstract void TakeDamage(float damage);
    
    public abstract IEnumerator RecoverySequence();
>>>>>>> develop

    public abstract void TakeDamageMelee(float damage);
    public abstract void TakeDamageDistance(float damage);
    public bool IsDead()
    {
        return isDead;
    }

    protected static void ReduceEnemyCounter()
    {
        GameManager.NumOfEnemies -= 1;
        UIController.Instance.SetEnemyCounter(GameManager.NumOfEnemies);
    }
    
    protected static void IncrementEnemyCounter()
    {
        GameManager.NumOfEnemies += 1;
        UIController.Instance.SetEnemyCounter(GameManager.NumOfEnemies);
    }

}
