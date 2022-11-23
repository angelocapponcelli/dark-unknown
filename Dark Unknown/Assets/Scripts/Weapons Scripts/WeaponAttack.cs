using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private float _damage = 10f;
    [SerializeField] Sword _sword;

    private void Start()
    {
        if (_sword != null)
        {
            _damage = _sword.getDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponents<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamage(_damage);
            }
        }
    }
}
