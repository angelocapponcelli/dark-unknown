using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : EnemyController
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxHealth;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isDead;    
    private float _currentHealth;
    private bool _canMove;

    private SkeletonMovement _movement;
    private SkeletonAnimator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        _canMove = true;

        _movement = GetComponent<SkeletonMovement>();
        _animator = GetComponent<SkeletonAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        _direction = _target.transform.position - transform.position;
        _direction.Normalize();

        // If the skeleton is not dead
        if (!_isDead)
        {
            // It follows the player till it reaches a minimum distance
            if (_distance > _minDistance && _canMove)
            {
                _movement.MoveSkeleton(_direction);
                _animator.AnimateSkeleton(true, _direction);
            }
            else
            {
                // At the minimum distance, it stops moving
                _canMove = false;
                StartCoroutine(Attack(_direction));
            }
        }

        // -- Handle Animations --
        // Hurt
        if (Input.GetKeyDown("e"))
            TakeDamage(10);
        // Death
        if (Input.GetKeyUp("z")) {
            if (_isDead)
            {
                StartCoroutine(RecoverySequence());
            }            
        }
    }

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateAttack(direction);
        yield return new WaitForSeconds(2);
        _canMove = true;
    }

    private IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(2);
        _isDead = false;
        _canMove = true;
    }    
 
    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            Die();
        } else
        {
            _animator.AnimateTakeDamage();            
        }
    }

    private void Die()
    {
        _isDead = true;
        _canMove = false;
        _animator.AnimateDie();
    }
}
