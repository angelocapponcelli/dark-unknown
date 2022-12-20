using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponents<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamageMelee(_damage*Player.Instance.GetStrengthMultiplier());
            }
        }
    }
}
