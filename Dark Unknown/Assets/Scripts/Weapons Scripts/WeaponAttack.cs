using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    private float _damage = 20f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitEnemies)
        {
            if (character.gameObject.CompareTag("Enemy"))
            {
                character.GetComponent<EnemyController>().TakeDamage(_damage);
            }
        }
    }
}
