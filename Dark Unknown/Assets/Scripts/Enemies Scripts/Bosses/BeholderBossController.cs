using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BeholderBossController : EnemyController
{
    private Player _target;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private Material flashMaterial;
    private Material _originalMaterial;

    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _damageCoroutineRunning;
    private bool _isHittable;
    private SpriteRenderer _spriteRenderer;
    

    private SpriteRenderer _skeletonRenderer;
    private EnemyAI _ai;
    private BossUIController _bossUIController = null;
    
    private bool _deathSoundPlayed = false;
    private RoomLogic _currentRoom;

    // Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _target = Player.Instance;
        _originalMaterial = _spriteRenderer.material;

        _currentHealth = _maxHealth;
        
        _skeletonRenderer = GetComponent<SpriteRenderer>();
        _ai = GetComponent<EnemyAI>();

        if (GetComponent<BossUIController>() == null) return;
        _bossUIController = GetComponent<BossUIController>();
        _bossUIController.SetName("Skeleton Overlord");
        _bossUIController.SetMaxHealth(_maxHealth);

        _currentRoom = LevelManager.Instance.GetCurrentRoom();
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_target == null || isDead)
        {
            return;
        }
        
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);

   
        


        // -- Cheats --
        // Hurt
        if (Input.GetKeyDown(KeyCode.Alpha0))
            TakeDamageMelee(50);
        // Enable while debugging to reanimate enemies
        /*if (Input.GetKeyUp("z")) {
            if (isDead)
            {
                StartCoroutine(RecoverySequence());
            }            
        }*/
    }

    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        yield return new WaitForSeconds(0f);
    }

    public override void CrystalDestroyed()
    {
        throw new NotImplementedException();
    }

    public override void TakeDamageMelee(float damage)
    {
        if (isDead) return;
        if (!_isHittable) return;
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
            StartCoroutine(DamageDistance());
        }
    }
    
    public override void TakeDamageDistance(float damage)
    {
        if (isDead) return;
        if (!_isHittable) return;
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
            StartCoroutine(DamageDistance());
        }
    }
    
    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        throw new NotImplementedException();
    }
    
    private IEnumerator DamageMelee()
    {
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds( 0.8f); //added 0.3f offset to make animation more realistic
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(0.8f); //added 0.3f offset to make animation more realistic
        _damageCoroutineRunning = false;
    }
    
    private IEnumerator Flash()
    {
        _spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _originalMaterial;
    }

    private void Die()
    {
        isDead = true;
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlaySkeletonDieSound();
        _deathSoundPlayed = true;
        ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
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
}
