using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("damage");
                enemy.GetComponentInParent<EnemyController>().TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
