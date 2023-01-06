using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ViperController : EnemyController
{
    private Player _target;
    [SerializeField] private GameObject _killedReward;
    [SerializeField] private int _rewardAmount = 3;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float dashFrequency;
    [SerializeField] private float dashSpeed;
    private float _timeForNextAttack;
    private float _timeElapsedFromDash;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _damageCoroutineRunning;
    private bool _isDashing;
    private bool _collision;
    private Transform _dashTarget;
    private Vector2 _dashDir;

    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private SpriteRenderer _viperRenderer;
    [SerializeField] private Material flashMaterial;
    private Material _originalMaterial;
    private EnemyAI _ai;

    private bool _deathSoundPlayed = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = Player.Instance;
        
        _currentHealth = _maxHealth;
        _canMove = true;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _viperRenderer = GetComponent<SpriteRenderer>();
        _ai = GetComponent<EnemyAI>();

        _timeForNextAttack = 0;
        _timeElapsedFromDash = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null)
        {
            return;
        }
        
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);

        if (_timeForNextAttack > 0) _timeForNextAttack -= Time.deltaTime;
        _timeElapsedFromDash += Time.deltaTime;

        // If the skeleton is not dead
        if (!isDead && _distance <= _chaseDistance)
        {
            if (_timeElapsedFromDash >= dashFrequency || _isDashing)
            {
                if (!_isDashing)
                {
                    _isAttacking = true;
                    _canMove = false;
                    _movement.StopMovement();
                    StartCoroutine(DashAttack());
                }
                else
                {
                    _movement.MoveEnemy(_dashDir);
                }

                return;
            }
            // It follows the player till it reaches a minimum distance
            if (_distance > _minDistance && _canMove)
            {
                if (!_ai.GetMovingDirection().Equals(Vector2.zero))
                {
                    _movement.MoveEnemy(_ai.GetMovingDirection());
                    _animator.AnimateEnemy(true, _ai.GetMovingDirection());    
                }
                else
                {
                    _animator.AnimateIdle();
                }
            }
            else if (!_damageCoroutineRunning && _timeForNextAttack <= 0) //&& !_isAttacking)
            {
                // At the minimum distance, it stops moving
                _isAttacking = true;
                _canMove = false;
                _movement.StopMovement();
                _timeForNextAttack = attackDelay;
                StartCoroutine(Attack(_ai.GetMovingDirection()));
            }
            else
            {
                _animator.AnimateIdle();
            }
        }
        else
        {
            _animator.AnimateIdle();
        }

        if (_isAttacking) //to flip skeleton in the right direction when is attacking
        {
            _animator.flip(_target.transform.position - transform.position);
        }
        
        // -- Handle Animations --
        // Hurt
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(50);

        // -- Handle Animations --
        // Hurt
        /*if (Input.GetKeyDown("e"))
            TakeDamage(50,false);*/
        // Enable while debugging to reanimate enemies
        /*if (Input.GetKeyUp("z")) {
            if (isDead)
            {
                StartCoroutine(RecoverySequence());
            }            
        }*/
    }

    private void AttackEvent()
    {
        if (!_isAttacking && !_damageCoroutineRunning)
        {
            // At the minimum distance, it stops moving
            _isAttacking = true;
            _canMove = false;
            _movement.StopMovement();
            
            StartCoroutine(Attack(_ai.GetMovingDirection()));
        }
    }

    private IEnumerator DashAttack()
    {
        _dashTarget = _target.transform;
        _movement.setSpeed(dashSpeed);
        _dashDir = (_dashTarget.position - transform.position).normalized;
        
        _animator.AnimateAttack(_dashDir);

        yield return new WaitForSeconds(0.8f);
        _isDashing = true;
        yield return new WaitForSeconds(0.5f);
        _movement.resetSpeed();
        yield return new WaitForSeconds(0.2f);

        _timeElapsedFromDash = 0;
        _isAttacking = false;
        _canMove = true;
        _animator.CanMove();
        _isDashing = false;

    }

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateSecondAttack(direction);
        AudioManager.Instance.PlayViperAttackSound();

        yield return new WaitForSeconds(0.4f);

        _isAttacking = false;
        _canMove = true;
        _animator.CanMove();
    }
    
    public override void TakeDamageMelee(float damage)
    {
        if (isDead) return;
        _movement.StopMovement();
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DisableBoxCollider();
            Die();
        } else
        {
            _damageCoroutineRunning = true;
            StartCoroutine(DamageMelee());
        }
    }

    public override void TakeDamageDistance(float damage)
    {
        if (isDead) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            DisableBoxCollider();
            Die();
        } else
        {
            _damageCoroutineRunning = true;
            StartCoroutine(DamageDistance());
        }
    }
    
    private IEnumerator DamageMelee()
    {
        _animator.AnimateTakeDamage(); 
        _canMove = false;
        AudioManager.Instance.PlayViperHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlayViperHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }

    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        _animator.Freeze(slowdownFactor);
        _movement.DecreaseSpeed(slowdownFactor);
        _viperRenderer.color = Color.cyan;
        yield return new WaitForSeconds(seconds);
        _animator.StopFreeze(slowdownFactor);
        _movement.IncreaseSpeed(slowdownFactor);
        _viperRenderer.color = Color.white;
    }
    
    private IEnumerator Flash()
    {
        _viperRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _viperRenderer.material = _originalMaterial;
    }

    private void Die()
    {
        //instantiate the rewards
        for (int i = 0; i < _rewardAmount; i++)
        {
            LevelManager.Instance.killedRewards.Add(Instantiate(_killedReward,transform.position,Quaternion.identity));
        }
        
        isDead = true;
        _canMove = false;
        _movement.StopMovement();
        _animator.AnimateDie();
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlayViperDieSound();
        _deathSoundPlayed = true;
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
    }
    
    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(1f);
        _animator.AnimateIdle();
        EnableBoxCollider();
        yield return new WaitForSeconds(1.5f);
        IncrementEnemyCounter(LevelManager.Instance.GetCurrentRoom());
        isDead = false; 
        _canMove = true;
        _deathSoundPlayed = false;
    }

    public override void CrystalDestroyed()
    {
        throw new NotImplementedException();
    }

    private void DisableBoxCollider()
    {
        var skeletonColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var c in skeletonColliders)
        {
            c.enabled = false;
        }
    }
    
    private void EnableBoxCollider()
    {
        // GetComponentsInChildren only returns components of active children
        // Use the parameter includeInactive: true to search through inactive children too
        var allChildren = gameObject.GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
        foreach (var c in allChildren)
        {
            c.enabled = true;
        }
    }
}
