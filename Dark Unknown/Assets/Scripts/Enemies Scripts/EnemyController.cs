using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    protected bool isDead;

    public abstract void TakeDamageMelee(float damage);
    public abstract void TakeDamageDistance(float damage);
    
    public abstract IEnumerator Freeze(float seconds, float slowdownFactor);
    
    public abstract IEnumerator RecoverySequence();

    public bool IsDead()
    {
        return isDead;
    }

    protected static void ReduceEnemyCounter(RoomLogic currentRoom)
    {
        currentRoom.ModifyNumOfEnemies(-1);
        UIController.Instance.SetEnemyCounter(currentRoom.GetNumOfEnemies());
        Player.Instance.IncreaseMana(1);
    }
    
    protected static void IncrementEnemyCounter(RoomLogic currentRoom)
    {
        currentRoom.ModifyNumOfEnemies(1);
        UIController.Instance.SetEnemyCounter(currentRoom.GetNumOfEnemies());
    }

}
