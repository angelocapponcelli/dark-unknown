using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 5f;
    private AirAttackAbility _airAttackAbility;

    public void SetAirAttackAbility(AirAttackAbility generator)
    {
        _airAttackAbility = generator;
    }

    public void Deactivate()
    {
        if (_airAttackAbility)
            _airAttackAbility.SetIsActive(false);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponents<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamageMelee(_damage);
            }
        }
    }
}
