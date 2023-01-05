using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossController : EnemyController, IEffectable
{
    private Player _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    private float _offset = 0.3f;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 5f;
    
    [SerializeField] private float healingCountdown = 10f;
    private float _healingCounter;
    private int _numOfCrystals;
    private bool _isHealing;
    private bool _crystalDestroyed;
    private bool _allCrystalsDestroyed;
    [SerializeField] private StatusEffectData healingEffect;
    private StatusEffectData _statusEffect;
    private float _currentEffectTime = 0;
    private float _nextTickTime = 0;
    private GameObject _statusEffectParticles;
    // only for testing
    //private GameObject[] _crystals;

    private BossUIController _bossUIController = null;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
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

    // only for testing
    /*private void Awake()
    {
        _crystals = GameObject.FindGameObjectsWithTag("Crystal");
    }*/

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = Player.Instance;
        
        _currentHealth = _maxHealth;
        _canMove = true;
        
        if (GetComponent<BossUIController>() == null) return;
        _bossUIController = GetComponent<BossUIController>();
        _bossUIController.SetName("Spider Broodmother");
        _bossUIController.SetMaxHealth(_maxHealth);
        
        _currentRoom = LevelManager.Instance.GetCurrentRoom();
        _numOfCrystals = _currentRoom.crystals.Count;
        _healingCounter = healingCountdown;
        
        if (_currentRoom.crystals.Count == 0)
        {
            AllCrystalsDestroyed();
        }
        
        // only for testing
        /*_numOfCrystals = _crystals.Length;
        _healingCounter = healingCountdown;
        if (_crystals.Length == 0)
        {
            AllCrystalsDestroyed();
        }*/

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _spiderRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spiderRenderer.material;
        _ai = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    private void Update()
    {
        _timeElapsedFromShot += (Time.deltaTime % 60);
        if (_target == null)
        {
            return;
        }
        
        if (!isDead && !_allCrystalsDestroyed)
        {
            if (_healingCounter > 0)
            {
                _healingCounter -= Time.deltaTime;
                Debug.Log(_healingCounter);
                if (_healingCounter <= 0 && _isHealing == false)
                {
                    Debug.Log("Started healing");
                    Healing();
                }
            }
        }
        
        if(_statusEffect != null) HandleEffect();
        
        if (_isHealing) return;
        
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        
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
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(50);
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
        AudioManager.Instance.PlaySpiderAttackSound();

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
            if (_bossUIController != null) _bossUIController.SetHealth(0);
            Die();
            DisableBoxCollider();
            _bossUIController.DeactivateHealthBar();
        } else
        {
            if (_bossUIController != null)  _bossUIController.SetHealth(_currentHealth);
            StartCoroutine(DamageDistance());
        }
    }

    public override void TakeDamageDistance(float damage)
    {
        if (isDead) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_bossUIController != null) _bossUIController.SetHealth(0);
            Die();
            DisableBoxCollider();
            _bossUIController.DeactivateHealthBar();
        } else
        {
            if (_bossUIController != null)  _bossUIController.SetHealth(_currentHealth);
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
        AudioManager.Instance.PlaySpiderHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlaySpiderHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
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
        AudioManager.Instance.PlaySpiderDieSound();
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

    private void Healing()
    {
        _isHealing = true;
        _statusEffect = healingEffect;
        ApplyEffect(_statusEffect);
        _currentRoom.crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability();
        //_crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability(); // for testing
        //Debug.Log(_crystals[_numOfCrystals-1].name);
    }
    
    public override void CrystalDestroyed()
    {
        if (_currentRoom.crystals.Count <= 0) return;
        //if (_crystals.Length <= 0) return; // for testing
        _numOfCrystals -= 1;
        Debug.Log(_numOfCrystals);
        StartCoroutine(CrystalDestroyedCoroutine());
        if (_numOfCrystals == 0)
        {
            AllCrystalsDestroyed();
        }
    }
    
    private IEnumerator CrystalDestroyedCoroutine()
    {
        _crystalDestroyed = true;
        yield return new WaitForSeconds(0.5f);
        _isHealing = false;
        _healingCounter = healingCountdown;
        _crystalDestroyed = false;
    }

    private void AllCrystalsDestroyed()
    {
        _allCrystalsDestroyed = true;
    }

    private void DeactivateCrystal()
    {
        _currentRoom.crystals[_numOfCrystals-1].GetComponent<CrystalController>().DisableVulnerability();
        //_crystals[_numOfCrystals-1].GetComponent<CrystalController>().DisableVulnerability(); // for testing
        _isHealing = false;
        _healingCounter = healingCountdown;
    }
    
    public void ApplyEffect(StatusEffectData data)
    {
        _statusEffect = data;
        _statusEffectParticles = Instantiate(data.particles, transform);
    }

    public void RemoveEffect()
    {
        Destroy(_statusEffectParticles);
        _statusEffect = null;
        _currentEffectTime = 0;
        _nextTickTime = 0;
    }

    private void HandleEffect()
    {
        _currentEffectTime += Time.deltaTime;

        if(_crystalDestroyed) RemoveEffect();
        if (_statusEffect == null) return;
        if (_currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _statusEffect.tickSpeed;
            if (_currentHealth + _statusEffect.damage >= _maxHealth)
            {
                _currentHealth = _maxHealth;
                RemoveEffect();
                DeactivateCrystal();
            }
            else _currentHealth += _statusEffect.damage;
            UIController.Instance.SetBossHealth(_currentHealth);
        }
    }
}
