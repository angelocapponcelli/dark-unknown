using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RatController : EnemyController
{
    [SerializeField] private Player _target;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _minDistance = 3f;
    [SerializeField] private float meleeAttackDistance;
    [SerializeField] private Transform _spawnProjectilePoint;
    private float _offset = 0.3f;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Material flashMaterial;
    
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    //private bool _damageCoroutineRunning;
    private float _timeElapsedFromShot;
    private float _shotFrequency = 3;

    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private SpriteRenderer _ratRenderer;
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
        _ratRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _ratRenderer.material;
        _ai = GetComponent<EnemyAI>();

        _timeElapsedFromShot = 0;

    }

    // Update is called once per frame
    private void Update()
    {
        _timeElapsedFromShot += (Time.deltaTime % 60);
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
        if (!isDead && _distance <= _chaseDistance)
        {
            if (_distance <= meleeAttackDistance && !_isAttacking)
            {
                AttackEvent(true);
            }
            else if (_distance > _minDistance + _offset && _canMove)
            {
                if (_timeElapsedFromShot >= _shotFrequency && !_isAttacking)
                {
                    AttackEvent(false);
                }
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
            else if (_distance < _minDistance - _offset && _canMove)
            {
                _movement.MoveEnemy(_ai.GetMovingDirection()*(-1));
                _animator.AnimateEnemy(true, _ai.GetMovingDirection());
            } else if (_distance >= _minDistance - _offset && _distance <= _minDistance + _offset && _canMove)
            {
                _animator.AnimateIdle();
                _movement.StopMovement();
                if (_timeElapsedFromShot >= _shotFrequency && !_isAttacking)
                {
                    AttackEvent(false);
                }
            }
            /*else if (_distance == _minDistance && !_isAttacking && !_damageCoroutineRunning)
            {
                GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = _ai.GetMovingDirection();
                Destroy(projectile, 2.5f);
                // At the minimum distance, it stops moving
                _isAttacking = true;
                _canMove = false;
                _movement.StopMovement();
                StartCoroutine(Attack(_ai.GetMovingDirection()));
            }*/
        }
        else
        {
            _animator.AnimateIdle();
        }

        if (_isAttacking && !isDead) //to flip skeleton in the right direction when is attacking
        {
            _animator.flip(_target.transform.position - transform.position);
        }

        // -- Handle Animations --
        // Hurt
        /*if (Input.GetKeyDown("e"))
            TakeDamage(50,false);*/
        // Death
        /*if (Input.GetKeyUp("z")) {
            if (isDead)
            {
                StartCoroutine(RecoverySequence());
            }            
        }*/
    }

    private void AttackEvent(bool meleeAttack)
    {
        if (_ai.GetMovingDirection() != Vector2.zero && !meleeAttack)
        {
            _isAttacking = true;
            _canMove = false;
            _movement.StopMovement();
            StartCoroutine(Attack(_ai.GetMovingDirection(), meleeAttack));
            _timeElapsedFromShot = 0;   
        } else if (meleeAttack)
        {
            _isAttacking = true;
            _canMove = false;
            _movement.StopMovement();
            StartCoroutine(Attack(_ai.GetMovingDirection(), meleeAttack));
        }
    }

    private IEnumerator Attack(Vector2 direction, bool meleeAttack)
    {
        if(meleeAttack)
            _animator.AnimateSecondAttack(direction);
        else
        {
            _animator.AnimateAttack(direction);
            AudioManager.Instance.PlaySkeletonAttackSound(); 
        }


        yield return new WaitForSeconds(1f);
        //yield return new WaitForSeconds(_animator.GetCurrentState().length+_animator.GetCurrentState().normalizedTime);

        _isAttacking = false;
        _canMove = true;
        _animator.canMove();
    }

    public void ShotProjectile()
    {
        GameObject projectile = Instantiate(_projectile, _spawnProjectilePoint.position, Quaternion.identity);
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
            //_damageCoroutineRunning = true;
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
            //_damageCoroutineRunning = true;
            StartCoroutine(DamageDistance());
        }
    }
    
    private IEnumerator DamageMelee()
    {
        _animator.AnimateTakeDamage(); 
        _canMove = false;
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        //_damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        //_damageCoroutineRunning = false;
    }
    
    private IEnumerator Flash()
    {
        _ratRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _ratRenderer.material = _originalMaterial;
    }
    
    private IEnumerator FlashRed()
    {
        _ratRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _ratRenderer.color = Color.white;
    }

    private void Die()
    {
        isDead = true;
        _canMove = false;
        _movement.StopMovement();
        _animator.AnimateDie();
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlaySkeletonDieSound();
        _deathSoundPlayed = true;
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
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

    private void DisableBoxCollider()
    {
        var spiderColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in spiderColliders)
        {
            collider.gameObject.SetActive(false);//.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
