using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    [SerializeField] private float _damage = 0.5f;
    private Animator _animator;
    private static readonly int _destroyTrigger = Animator.StringToHash("Destroy");
    private Rigidbody2D _rigidbody2D;
    private IceProjectileAbility _iceProjectileAbility;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool enemyHit = false;
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamageDistance(_damage); //Change to ice effect TODO
                enemyHit = true;
            }
        }
        if (collision.GetComponent<Reward>() != null || enemyHit || collision.CompareTag("Trap") ||
            collision.gameObject.layer == 11) return; //layer 11 == "Player"
        DestroyProjectileRoutine();
        _iceProjectileAbility.Deactivate();
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void DestroyProjectileRoutine()
    {
        _animator.SetTrigger(_destroyTrigger);
        _rigidbody2D.velocity = Vector2.zero;
    }
    
    public void SetIceProjectileAbility(IceProjectileAbility generator)
    {
        _iceProjectileAbility = generator;
    }
}
