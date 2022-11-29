using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PurpleProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;
    private Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        StartCoroutine(DeactivateCollider(0.3f));
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        _animator.SetTrigger("destroy");
        foreach (Collider2D player in hitEnemies)
        {
            if (player.gameObject.CompareTag("Player"))
            {
                Debug.Log("damage");
                player.GetComponentInParent<Player>().TakeDamage(_damage);
            }
        }

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
