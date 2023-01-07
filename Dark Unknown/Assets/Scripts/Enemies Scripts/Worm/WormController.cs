using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class WormController : EnemyController
{
    [SerializeField] private GameObject _killedReward;
    [SerializeField] private int _rewardAmount = 3;
    [SerializeField] private Player _target;
    [SerializeField] private float _triggerDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 2f;
    [SerializeField] private Transform _spawnProjectilePoint;
    [SerializeField] private Material flashMaterial;
    
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _canAttack;
    //private bool _damageCoroutineRunning;
    private float _timeElapsedFromShot;
    private float _shotFrequency = 3;
    private float _shotsNumber;
    private bool _isHidden;

    private EnemyMovement _movement;
    private WormAnimator _animator;
    private SpriteRenderer _wormRenderer;
    private Material _originalMaterial;

    private bool _deathSoundPlayed = false;

    private Transform _nextPosition;
    private bool _coroutineRunning;

    private Vector2 lastVelocity;
    private bool isMoving;
    private bool velCollider = false;
    
    private void Start()
    {
        isDead = false;
        _target = Player.Instance;
        _isHidden = true;
        _canAttack = false;

        _currentHealth = _maxHealth;
        _canMove = true;
        _shotsNumber = 0;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<WormAnimator>();
        _wormRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _wormRenderer.material;

        _timeElapsedFromShot = 0;
        _coroutineRunning = false;

        Vector2 vect = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10f;
        lastVelocity = vect;
    }

    private void Update()
    {

        _timeElapsedFromShot += (Time.deltaTime % 60);
        if (_target == null)
        {
            return;
        }

        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        _direction = _target.transform.position - transform.position;
        _direction.Normalize();

        if (!isDead && _distance <= _triggerDistance)
        {
            if (!_coroutineRunning)
            {
                _coroutineRunning = true;
                StartCoroutine(FindNewPosition());
            }
            _animator.flip(_direction);
            if (_isHidden && !isMoving)
            {
                Show();
            }

            if (_timeElapsedFromShot >= _shotFrequency && _shotsNumber <= 3 && _canAttack && !isMoving)
            {
                Attack();
                _shotsNumber += 1;
                _timeElapsedFromShot = 0;
            } else if (_shotsNumber > 3 && !isMoving)
            {
                _shotsNumber = 0;
                StartCoroutine(Move());
            }
            
            
        } else if (!isDead && _distance > _triggerDistance)
        {
            if (!_isHidden)
            {
                _shotsNumber = 0;
                Hide();   
            }
        }
        // -- Handle Animations --
        // Hurt
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(50);
        
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
    
    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        //TODO
        //rallenta
        yield return new WaitForSeconds(seconds);
        //torna a velocità normale
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
        throw new System.NotImplementedException();
    }

    private IEnumerator DamageMelee()
    {
        _animator.AnimateTakeDamage(); 
        _canMove = false;
        AudioManager.Instance.PlayWormHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        //_damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlayWormHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        //_damageCoroutineRunning = false;
    }
    
    private IEnumerator Flash()
    {
        _wormRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _wormRenderer.material = _originalMaterial;
    }

    private void Show()
    {
        _animator.Show();
        _isHidden = false;
    }
    private void Hide()
    {
        _animator.Hide();
        _isHidden = true;
        _coroutineRunning = false;
    }

    private IEnumerator Move()
    {
        Hide();
        isMoving = true;
        yield return new WaitForSeconds(1.9f);
        if (velCollider)
            lastVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 10f;
        GetComponent<Rigidbody2D>().velocity = lastVelocity;
        yield return new WaitForSeconds(0.3f);
        velCollider = false;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        isMoving = false;
        //transform.position = _nextPosition.position*0.8f;
    }

    private void Attack()
    {
        _animator.Attack();
        GameObject projectile = Instantiate(_projectile, _spawnProjectilePoint.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().color = Color.green;
        projectile.GetComponent<Rigidbody2D>().velocity = _direction*_projectileSpeed;
        AudioManager.Instance.PlayWormAttackSound();
        Destroy(projectile, 5f);
    }

    public void CanAttack()
    {
        _canAttack = true;
    }

    public void CantAttack()
    {
        _canAttack = false;
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
        AudioManager.Instance.PlayWormDieSound();
        _deathSoundPlayed = true;
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
    }
    
    private void DisableBoxCollider()
    {
        var wormColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var c in wormColliders)
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
    

    private IEnumerator FindNewPosition()
    {
        yield return new WaitForSeconds(2f);
        _nextPosition = _target.transform;
    }

    public void SetVelocity(Vector2 value)
    {
        lastVelocity = value;
        velCollider = true;
    }
    
    public Vector2 GetVelocity()
    {
        return lastVelocity;
    }
}
