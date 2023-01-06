using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(DeactivateCollider(0.3f));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (var element in hitEnemies)
        {
            if (element.gameObject.CompareTag("Player") || element.gameObject.CompareTag("PlayerFeetCollider"))
            {
                element.GetComponentInParent<Player>().TakeDamage(_damage);
            }
            if (element.gameObject.CompareTag("EnemyCollider") || element.gameObject.CompareTag("EnemyFeetCollider") ||
                element.gameObject.CompareTag("EyeProjectile") || element.gameObject.layer == 2) return;
        }

        if (collision.gameObject.CompareTag("WeaponCollider") || collision.gameObject.CompareTag("Projectile")) return;
        _animator.SetTrigger("destroy");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    
    private IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<CircleCollider2D>().enabled = true;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
