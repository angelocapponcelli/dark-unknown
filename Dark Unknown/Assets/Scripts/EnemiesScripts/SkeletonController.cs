using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : EnemyController
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isDead;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _damageCoroutineRunning;

    private SkeletonMovement _movement;
    private SkeletonAnimator _animator;
    private SkeletonAI _ai;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        _currentHealth = _maxHealth;
        _canMove = true;

        _movement = GetComponent<SkeletonMovement>();
        _animator = GetComponent<SkeletonAnimator>();
        _ai = GetComponent<SkeletonAI>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            return;
        }
        
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        /*
        _direction = _target.transform.position - transform.position;
        _direction.Normalize();*/
        
        // If the skeleton is not dead
        if (!_isDead && _distance <= _chaseDistance)
        {
            // It follows the player till it reaches a minimum distance
            if (_distance > _minDistance && _canMove)
            {
                _movement.MoveSkeleton(_ai.GetMovingDirection());
                _animator.AnimateSkeleton(true, _ai.GetMovingDirection());
            }
            else if (!_isAttacking && !_damageCoroutineRunning)
            {
                // At the minimum distance, it stops moving
                _isAttacking = true;
                _canMove = false;
                StartCoroutine(Attack(_ai.GetMovingDirection()));
            }
        }
        else
        {
            _animator.AnimateIdle();
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

    private void AttackEvent()
    {
        if (!_isAttacking && !_damageCoroutineRunning)
        {
            // At the minimum distance, it stops moving
            _isAttacking = true;
            _canMove = false;
            StartCoroutine(Attack(_ai.GetMovingDirection()));
        }
    }

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateAttack(direction);

        do
        {
            yield return null;
        } while (_distance < _minDistance);

        yield return new WaitForSeconds(0.7f);
        //yield return new WaitForSeconds(_animator.GetCurrentState().length+_animator.GetCurrentState().normalizedTime);

        _isAttacking = false;
        _canMove = true;
        _animator.canMove();
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
            _damageCoroutineRunning = true;
            StartCoroutine(Damage());
        }
    }
    
    private IEnumerator Damage()
    {
        _animator.AnimateTakeDamage();
        _canMove = false;
        yield return new WaitForSeconds(_animator.GetCurrentState().length);
        _canMove = true;
        _damageCoroutineRunning = false;
    }

    private void Die()
    {
        _isDead = true;
        _canMove = false;
        _animator.AnimateDie();
    }
}
