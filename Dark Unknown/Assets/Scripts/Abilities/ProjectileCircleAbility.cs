using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCircleAbility : MonoBehaviour
{
    [SerializeField] private float _damage = 0.5f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamageDistance(_damage);
            }
        }
        if (collision.CompareTag("Trap")) return;
    }
}
