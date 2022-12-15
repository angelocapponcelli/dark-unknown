using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RatProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;
    [SerializeField] private GameObject poison;
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
            //if (element.gameObject.CompareTag("Player") || element.gameObject.CompareTag("PlayerFeetCollider"))
            if (element.gameObject.transform.CompareTag("Player"))
            {
                Instantiate(poison,transform.position, Quaternion.Euler(0f,0f,0));
            }
        }
        /*if (collision.CompareTag("Trap") || 
            collision.CompareTag("EnemyFeetCollider") ||
            collision.CompareTag("Enemy")) return;*/
        if (collision.CompareTag("Trap") ||
            collision.CompareTag("Projectile") ||
            collision.gameObject.transform.parent.CompareTag("Enemy")) return;
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
