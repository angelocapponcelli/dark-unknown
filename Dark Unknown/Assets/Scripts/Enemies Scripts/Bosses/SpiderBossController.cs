using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossController : EnemyController
{
private Player _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    private float _offset = 0.3f;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 5f;
    [SerializeField] private float vulnerabilityTime = 5f;
    [SerializeField] private ParticleSystem _particleSystem;
    private int _numOfCrystals;
    private bool _isHittable;
    private bool _allCrystalsDestroyed;
    //private float _timeForNextAttack;
    private BossUIController _bossUIController = null;

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
    private SpriteRenderer _spiderRenderer;
    [SerializeField] private Material flashMaterial;
    private Material _originalMaterial;
    private EnemyAI _ai;

    private bool _deathSoundPlayed = false;
    private RoomLogic _currentRoom;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = Player.Instance;
        
        _currentHealth = _maxHealth;
        _canMove = true;
        
        if (GetComponent<BossUIController>() == null) return;
        _bossUIController = GetComponent<BossUIController>();
        _bossUIController.SetMaxHealth(_maxHealth);
        
        _currentRoom = LevelManager.Instance.GetCurrentRoom();
        _numOfCrystals = _currentRoom.crystals.Count;
        //StateGameManager.Crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability();
        
        if (_currentRoom.crystals.Count == 0)
        {
            AllCrystalsDestroyed();
        }

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _spiderRenderer = GetComponent<SpriteRenderer>();
        _ai = GetComponent<EnemyAI>();

        //_timeForNextAttack = 0;
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
            // It follows the player till it reaches a minimum distance
            if (_distance > _minDistance + _offset && _canMove)
            {
                if (_timeElapsedFromShot >= _shotFrequency)
                {
                    AttackEvent();
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
                //AudioManager.Instance.PlaySkeletonWalkSound(); //TODO sistemare il suono dei passi che va in loop
            }
            else if (_distance < _minDistance - _offset && _canMove)
            {
                _movement.MoveEnemy(_ai.GetMovingDirection()*(-1));
                _animator.AnimateEnemy(true, _ai.GetMovingDirection());
            } else if (_distance >= _minDistance - _offset && _distance <= _minDistance + _offset && _canMove)
            {
                _animator.AnimateIdle();
                _movement.StopMovement();
                if (_timeElapsedFromShot >= _shotFrequency)
                {
                    AttackEvent();
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

    private void AttackEvent()
    {
        if (_ai.GetMovingDirection() != Vector2.zero)
        {
            GameObject projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
            projectile.GetComponent<Rigidbody2D>().velocity = _ai.GetMovingDirection()*_projectileSpeed;
            Destroy(projectile, 2.5f);
            // At the minimum distance, it stops moving
            _isAttacking = true;
            _canMove = false;
            _movement.StopMovement();
            StartCoroutine(Attack(_ai.GetMovingDirection()));
            _timeElapsedFromShot = 0;   
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
        if (!_isHittable) return;
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
        if (!_isHittable) return;
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
        _animator.Freeze(slowdownFactor);
        _movement.DecreaseSpeed(slowdownFactor);
        _spiderRenderer.color = Color.cyan;
        yield return new WaitForSeconds(seconds);
        _animator.StopFreeze(slowdownFactor);
        _movement.IncreaseSpeed(slowdownFactor);
        _spiderRenderer.color = Color.white;
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
        _spiderRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _spiderRenderer.material = _originalMaterial;
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
        var skeletonColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in skeletonColliders)
        {
            collider.gameObject.SetActive(false);//.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    
    public override void CrystalDestroyed()
    {
        if (_currentRoom.crystals.Count <= 0) return;
        _numOfCrystals -= 1;
        /*for (var i=0; i<StateGameManager.Crystals.Count; i++)
        {
            var crystal = StateGameManager.Crystals[i];
            if (crystal.IsDead()) StateGameManager.Crystals.Remove(crystal);
        }*/
        Debug.Log(_numOfCrystals);
        if (_numOfCrystals == 0)
        {
            AllCrystalsDestroyed();
            return;
        }
        StartCoroutine(CrystalDestroyedCoroutine());
    }

    /*private IEnumerator CrystalDestroyedCoroutine()
    {
        _isHittable = true;
        _particleSystem.Stop();
        foreach (var crystal in StateGameManager.Crystals)
        {
            crystal.GetComponent<CrystalController>().DisableVulnerability();
        }
        yield return new WaitForSeconds(5f);
        if (_allCrystalsDestroyed) yield break;
        foreach (var crystal in StateGameManager.Crystals)
        {
            crystal.GetComponent<CrystalController>().EnableVulnerability();
        }
        _isHittable = false;
        _particleSystem.Play();
    }*/
    
    private IEnumerator CrystalDestroyedCoroutine()
    {
        _isHittable = true;
        _particleSystem.Stop();
        yield return new WaitForSeconds(vulnerabilityTime);
        if (_allCrystalsDestroyed || isDead) yield break;
        _currentRoom.crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability();
        _isHittable = false;
        _particleSystem.Play();
    }

    private void AllCrystalsDestroyed()
    {
        Debug.Log("all crystals destroyed");
        _particleSystem.Stop();
        _isHittable = true;
        _allCrystalsDestroyed = true;
    }
}
