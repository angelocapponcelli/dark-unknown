using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ProjectileCircleAbility : MonoBehaviour
{
    [SerializeField] private float _damage = 0.5f;
    private Animator _animator;
    private static readonly int _destroyTrigger = Animator.StringToHash("Destroy");

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                enemy.GetComponentInParent<EnemyController>().TakeDamageDistance(_damage);
                _animator.SetTrigger(_destroyTrigger);
            }
        }
        if (collision.CompareTag("Trap")) return;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
