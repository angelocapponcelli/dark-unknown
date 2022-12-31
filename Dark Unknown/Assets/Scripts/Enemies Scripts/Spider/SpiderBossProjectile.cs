using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SpiderBossProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;
    [SerializeField] private GameObject spider;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(DeactivateCollider(0.6f));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (var element in hitEnemies)
        {
            //if (element.gameObject.CompareTag("Player") || element.gameObject.CompareTag("PlayerFeetCollider"))
            if (element.gameObject.transform.CompareTag("Player"))
            {
                element.GetComponentInParent<Player>().TakeDamage(_damage);
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
        if (StateGameManager.NumOfEnemies < 4)
        {
            Instantiate(spider, transform.position, Quaternion.identity);
            StateGameManager.NumOfEnemies += 1;
            UIController.Instance.SetEnemyCounter(StateGameManager.NumOfEnemies);
        }

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
