using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UndeadController : EnemyController
{
    private Player _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private GameObject heart;
    [SerializeField] private Transform _spawnProjectilePoint;
    private float _timeForNextAttack;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _damageCoroutineRunning;

    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private SpriteRenderer _undeadRenderer;
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
        _undeadRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _undeadRenderer.material;
        _ai = GetComponent<EnemyAI>();

        _timeForNextAttack = 0;
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

        // If the skeleton is not dead
        if (!isDead && _distance <= _chaseDistance)
        {
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
        /*if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(100);*/
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

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateAttack(direction);
        AudioManager.Instance.PlaySkeletonAttackSound();

        yield return new WaitForSeconds(0.7f);

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

    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        _animator.Freeze(slowdownFactor);
        _movement.DecreaseSpeed(slowdownFactor);
        _undeadRenderer.color = Color.cyan;
        yield return new WaitForSeconds(seconds);
        _animator.StopFreeze(slowdownFactor);
        _movement.IncreaseSpeed(slowdownFactor);
        _undeadRenderer.color = Color.white;
    }
    
    private IEnumerator DamageMelee()
    {
        _animator.AnimateTakeDamage(); 
        _canMove = false;
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator Flash()
    {
        _undeadRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _undeadRenderer.material = _originalMaterial;
    }

    private void Die()
    {
        heart = Instantiate(heart, _spawnProjectilePoint.position, Quaternion.identity);
        heart.GetComponent<UndeadHeart>().Init(this);
        isDead = true;
        _canMove = false;
        _movement.StopMovement();
        _animator.AnimateDie();
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlaySkeletonDieSound();
        _deathSoundPlayed = true;
        //ReduceEnemyCounter();
    }

    public void Recover()
    {
        var skeletonColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in skeletonColliders)
        {
            collider.gameObject.SetActive(true);//.GetComponent<BoxCollider2D>().enabled = false;
        }

        StartCoroutine(RecoverySequence());
    }
    
    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(1f);
        _animator.AnimateIdle();
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
        foreach (var collider in skeletonColliders)
        {
            collider.gameObject.SetActive(false);//.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void ReduceEnemyCounterPublic()
    {
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
    }
}