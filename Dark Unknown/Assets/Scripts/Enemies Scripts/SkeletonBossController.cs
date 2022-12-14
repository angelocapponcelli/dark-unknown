using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkeletonBossController : EnemyController
{
    private Player _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float vulnerabilityTime = 5f;
    [SerializeField] private float reanimationCountDown = 20f;
    private float _timeForNextAttack;
    private float _timeForNextReanimation;
    private bool _reanimationStarted = false;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _damageCoroutineRunning;
    private bool _isHittable;
    private int _numOfCrystals;
    //private GameObject[] _crystals;
    private bool _allCrystalsDestroyed;
    [SerializeField] private ParticleSystem _particleSystem;

    private EnemyMovement _movement;
    private EnemyAnimator _animator;
    private SpriteRenderer _skeletonRenderer;
    private EnemyAI _ai;
    private SkeletonBossUIController _bossUIController = null;
    
    private bool _deathSoundPlayed = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _allCrystalsDestroyed = false;
        _target = Player.Instance;
        //StateGameManager.Crystals = GameObject.FindGameObjectsWithTag("Crystal");

        _currentHealth = _maxHealth;
        _canMove = true;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<EnemyAnimator>();
        _skeletonRenderer = GetComponent<SpriteRenderer>();
        _ai = GetComponent<EnemyAI>();

        _timeForNextAttack = 0;
        _timeForNextReanimation = reanimationCountDown;

        if (GetComponent<SkeletonBossUIController>() == null) return;
        _bossUIController = GetComponent<SkeletonBossUIController>();
        _bossUIController.SetMaxHealth(_maxHealth);
        
        // Testing
        /*foreach (var crystal in GameObject.FindGameObjectsWithTag("Crystal"))
        {
            StateGameManager.Crystals.Add(crystal.GetComponent<EnemyController>());
        }*/
        _numOfCrystals = StateGameManager.Crystals.Count;
        //StateGameManager.Crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability();
        
        if (StateGameManager.Crystals.Count == 0)
        {
            AllCrystalsDestroyed();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null || isDead)
        {
            return;
        }

        /*if (StateGameManager.Crystals.Length == 0 && _allCrystalsDestroyed == false)
        {
            AllCrystalsDestroyed();
        }*/
        
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);

        if (_timeForNextAttack > 0) _timeForNextAttack -= Time.deltaTime;
        if (_timeForNextReanimation > 0)
        {
            _timeForNextReanimation -= Time.deltaTime;
            if (_timeForNextReanimation is < 2f and > 0 && _reanimationStarted==false)
            {
                _reanimationStarted = true;
                StartCoroutine(ReanimationTelegraph());
            }
        }
        else
        {
            Debug.Log("Reanimating");
            ReanimateMobs();
            _timeForNextReanimation = reanimationCountDown;
            _reanimationStarted = false;
        }
        
        // If the skeleton is not dead
        if (!isDead && _distance <= _chaseDistance)
        {
            // It follows the player till it reaches a minimum distance
            if (_distance > _minDistance && _canMove)
            {
                _movement.MoveEnemy(_ai.GetMovingDirection());
                _animator.AnimateEnemy(true, _ai.GetMovingDirection());
            }
            else if (!_damageCoroutineRunning && _timeForNextAttack <= 0)
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

        // -- Cheats --
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

    private IEnumerator Attack(Vector2 direction)
    {
        _animator.AnimateAttack(direction);
        AudioManager.Instance.PlaySkeletonAttackSound();

        yield return new WaitForSeconds(0.7f);

        _isAttacking = false;
        _canMove = true;
        _animator.canMove();
    }

    private void ReanimateMobs()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.GetComponent<EnemyController>().IsDead())
            {
                StartCoroutine(enemy.GetComponent<EnemyController>().RecoverySequence());
            }
        }
    }
    
    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(2);
        isDead = false; 
        _canMove = true;
    }
 
    public override void TakeDamage(float damage, bool damageFromArrow)
    {
        if (!_isHittable) return;
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
            _damageCoroutineRunning = true;
            StartCoroutine(Damage());
        }
    }
    
    private IEnumerator Damage()
    {
        if (!_damageFromDistance)
        {
            _animator.AnimateTakeDamage();
            _canMove = false;
        }
        else
        {
            StartCoroutine(FlashRed());
            _canMove = true;
        }
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
        _canMove = true;
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator FlashRed()
    {
        _skeletonRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _skeletonRenderer.color = Color.white;
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
        //ReduceEnemyCounter();
    }

    private IEnumerator ReanimationTelegraph()
    {
        for (float i = 0; i < 2f; i += 0.1f)
        {
            _skeletonRenderer.color = Color.cyan;
            yield return new WaitForSeconds(0.1f);
            _skeletonRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void DisableBoxCollider()
    {
        var spiderColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in spiderColliders)
        {
            collider.gameObject.SetActive(false);
        }
    }

    public void CrystalDestroyed()
    {
        if (StateGameManager.Crystals.Count <= 0) return;
        var crystal = StateGameManager.Crystals[_numOfCrystals-1];
        if (crystal.IsDead()) StateGameManager.Crystals.Remove(crystal);
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
        StateGameManager.Crystals[_numOfCrystals-1].GetComponent<CrystalController>().EnableVulnerability();
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
