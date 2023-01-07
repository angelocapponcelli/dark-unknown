using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UndeadController : EnemyController
{
    private Player _target;
    [SerializeField] public GameObject _killedReward;
    [SerializeField] public int _rewardAmount = 3;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private UndeadHeart heart;
    private UndeadHeart _spawnedHeart;
    [SerializeField] private Transform _spawnProjectilePoint;
    [SerializeField] public GameObject shadow;
    private float _timeForNextAttack;
    private bool _isPartialDead = false;

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
        if (_canMove && (!isDead && !_isPartialDead) && _distance <= _chaseDistance)
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
        // -- Handle Animations --
        // Hurt
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(100);
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
        AudioManager.Instance.PlayUndeadAttackSound();

        yield return new WaitForSeconds(0.7f);

        _isAttacking = false;
        _canMove = true;
        _animator.CanMove();
    }

    public override void TakeDamageMelee(float damage)
    {
        if (isDead || _isPartialDead) return;
        _movement.StopMovement();
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

    public override void TakeDamageDistance(float damage)
    {
        if (isDead || _isPartialDead) return;
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
        AudioManager.Instance.PlayUndeadHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlayUndeadHurtSound();
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
        shadow.gameObject.SetActive(false);
        _isPartialDead = true;
        _canMove = false;
        _movement.StopMovement();
        _animator.AnimateDie();
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlayUndeadDieSound();
        _deathSoundPlayed = true;
        _spawnedHeart = Instantiate(heart, _spawnProjectilePoint.position + Vector3.zero, Quaternion.identity);
        _spawnedHeart.GetComponent<UndeadHeart>().Init(this);
    }

    public void Recover()
    {
        EnableBoxCollider();

        StartCoroutine(RecoverySequence());
    }
    
    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(1f);
        _animator.AnimateIdle();
        yield return new WaitForSeconds(1.5f);
        isDead = false;
        _isPartialDead = false;
        _canMove = true;
        _deathSoundPlayed = false;
        shadow.gameObject.SetActive(true);
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
        var undeadColliders = gameObject.GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
        foreach (var c in undeadColliders)
        {
            c.enabled = true;
        }
    }

    public void ReduceEnemyCounterPublic()
    {
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
    }

    public void SetIsDead(bool dead)
    {
        isDead = dead;
    }

    public bool GetIsDead()
    {
        return isDead;
    }
}
