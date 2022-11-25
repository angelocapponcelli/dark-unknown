using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D player in hitEnemies)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                Debug.Log("damage");
                player.GetComponentInParent<Player>().TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
