using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PurpleProjectile : MonoBehaviour
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
        foreach (var player in hitEnemies)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                player.GetComponentInParent<Player>().TakeDamage(_damage);
            }
            if (player.gameObject.CompareTag("Enemy")) return;
        }
        _animator.SetTrigger("destroy");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    
    private IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
