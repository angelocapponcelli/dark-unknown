using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BatController : EnemyController
{
    private Player _player;
    [SerializeField] private float _targetDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float attackDelay = 3f;
    private float _timeForNextAttack;
    [SerializeField] private GameObject[] _enemies;
    private GameObject _target;
    private float _timeElapsedFromShot;
    private float _shotFrequency = 3;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 5f;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private Vector2 _targetDirection;
    private float _distance;
    private bool _isAttacking;


    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private SpriteRenderer _skeletonRenderer;
    private EnemyAI _ai;

    private bool _deathSoundPlayed = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = Player.Instance;
        _target = null;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _skeletonRenderer = GetComponent<SpriteRenderer>();
        _ai = GetComponent<EnemyAI>();

        _timeElapsedFromShot = 0;
        _timeForNextAttack = 0;
        FindEnemies();
    }

    // Update is called once per frame
    private void Update()
    {

        if (_target == null)
        {
            FindTarget();
        }
        else if (_target.GetComponent<EnemyController>().IsDead() || Vector2.Distance(_target.transform.position, transform.position) >= _targetDistance)
        {
            FindTarget();
        }
        
        _timeElapsedFromShot += (Time.deltaTime % 60);
        
        // Calculates distance and direction of movement
        _direction = (_player.transform.position - transform.position).normalized;
        _targetDirection = (_target.transform.position - transform.position).normalized;
        _distance = Vector2.Distance(transform.position, _player.transform.position);

        if (_timeForNextAttack > 0) _timeForNextAttack -= Time.deltaTime;

        _animator.AnimateEnemy(true, _targetDirection);
        // If the skeleton is not dead
        if (!_isAttacking && _distance >= _chaseDistance)
        {
            _movement.MoveEnemy(_direction);
        }

        if (_timeElapsedFromShot >= _shotFrequency && !_target.GetComponent<EnemyController>().IsDead())
        {
            AttackEvent();
        }
        
        
    }

    private void FindEnemies()
    {
        _enemies = GameObject.FindGameObjectsWithTag("Enemy");
    }

    private void FindTarget()
    {
        foreach (var enemy in _enemies)
        {
            if (Vector2.Distance(enemy.transform.position, transform.position) <= _targetDistance && !enemy.GetComponent<EnemyController>().IsDead())
            {
                _target = enemy;
                return;
            }
        }
    }
    
    private void AttackEvent()
    {
        if (_targetDirection != Vector2.zero)
        {
            GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = _targetDirection*_projectileSpeed;
            Destroy(projectile, 2.5f);
            // At the minimum distance, it stops moving
            _isAttacking = true;
            _movement.StopMovement();
            StartCoroutine(Attack(_targetDirection));
            _timeElapsedFromShot = 0;   
        }
    }

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateAttack(direction);
        AudioManager.Instance.PlaySkeletonAttackSound();

        yield return new WaitForSeconds(0.7f);

        _isAttacking = false;
        _animator.CanMove();
    }


    public override void TakeDamageMelee(float damage)
    {
        throw new NotImplementedException();
    }

    public override void TakeDamageDistance(float damage)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator RecoverySequence()
    {
        throw new NotImplementedException();
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
            c.gameObject.SetActive(false);
        }
    }
    
    private void EnableBoxCollider()
    {
        // GetComponentsInChildren only returns components of active children
        // Use the parameter includeInactive: true to search through inactive children too
        var allChildren = gameObject.GetComponentsInChildren<BoxCollider2D>(includeInactive: true);
        foreach (var c in allChildren)
        {
            c.gameObject.SetActive(true);
        }
    }
}
