using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Worm2Controller : EnemyController
{
    [SerializeField] private Player _target;
    [SerializeField] private float _triggerDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 2.5f;
    [SerializeField] private Transform _spawnProjectilePoint;
    [SerializeField] private float radius;
    
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canAttack;
    private float _timeElapsedFromShot;
    private float _timeElapsedFromHide;
    private float _shotFrequency = 5;
    private float _shotsNumber;
    private bool _isHidden;

    private EnemyMovement _movement;
    private WormAnimator _animator;
    private SpriteRenderer _wormRenderer;

    private bool _deathSoundPlayed = false;
    


    private void Start()
    {
        isDead = false;
        _target = Player.Instance;
        _isHidden = true;
        _canAttack = false;

        _currentHealth = _maxHealth;
        _shotsNumber = 0;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<WormAnimator>();
        _wormRenderer = GetComponent<SpriteRenderer>();

        _timeElapsedFromShot = 0;
        _timeElapsedFromHide = 0;


    }

    private void Update()
    {
        _timeElapsedFromShot += (Time.deltaTime % 60);
        _timeElapsedFromHide += (Time.deltaTime % 60);
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

            _animator.flip(_direction);
            if (_isHidden && _timeElapsedFromHide >= 5)
            {
                Show();
            }

            if (_timeElapsedFromShot >= _shotFrequency && _shotsNumber <= 3 && !_isHidden && _canAttack)
            {
                Attack(6);
                _shotsNumber += 1;
                _timeElapsedFromShot = 0;
            } else if (_shotsNumber > 3)
            {
                _timeElapsedFromHide = 0;
                _shotsNumber = 0;
                Hide();
            }
            
            
        } else if (!isDead && _distance > _triggerDistance)
        {
            if (!_isHidden)
            {
                _shotsNumber = 0;
                Hide();   
            }
        }
        
        
    }
    
    public override void TakeDamage(float damage, bool damageFromArrow)
    {
        if (isDead) return;
        _movement.StopMovement();
        _currentHealth -= damage;
        _damageFromDistance = damageFromArrow;
        if (_currentHealth <= 0)
        {
            Die();
            DisableBoxCollider();
        } else
        {
            StartCoroutine(Damage());
        }
    }

    public override IEnumerator RecoverySequence()
    {
        _currentHealth = _maxHealth;
        _animator.AnimateRecover();
        yield return new WaitForSeconds(1f);
        _animator.AnimateIdle();
        yield return new WaitForSeconds(1.5f);
        IncrementEnemyCounter();
        isDead = false;
        _deathSoundPlayed = false;
    }

    private IEnumerator Damage()
    {
        if (!_damageFromDistance)
        {
            _animator.AnimateTakeDamage();
        } else StartCoroutine(FlashRed());
        AudioManager.Instance.PlaySkeletonHurtSound();
        yield return new WaitForSeconds(_animator.GetCurrentState().length + 0.3f); //added 0.3f offset to make animation more realistic
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
    }


    private void Attack(float numberOfProjectiles)
    {
        _animator.Attack();

        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;
        
        for(int i = 0; i < numberOfProjectiles; i++)
        {

            float projectileDirXPosition = _spawnProjectilePoint.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
            float projectileDirYPosition = _spawnProjectilePoint.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

            Vector2 projectileVector = new Vector2(projectileDirXPosition, projectileDirYPosition);
            Vector2 projectileMoveDirection = (projectileVector - (Vector2)_spawnProjectilePoint.position).normalized * _projectileSpeed;
            
            GameObject projectile = Instantiate(_projectile, _spawnProjectilePoint.position, Quaternion.identity);
            projectile.GetComponent<SpriteRenderer>().color = Color.green;
            projectile.GetComponent<Rigidbody2D>().velocity =
                new Vector2(projectileMoveDirection.x, projectileMoveDirection.y);
            Destroy(projectile, 5f);

            angle += angleStep;

        }
        

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
        isDead = true;
        _movement.StopMovement();
        _animator.AnimateDie();
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlaySkeletonDieSound();
        _deathSoundPlayed = true;
        ReduceEnemyCounter();
    }
    
    private IEnumerator FlashRed()
    {
        _wormRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _wormRenderer.color = Color.white;
    }
    
    private void DisableBoxCollider()
    {
        var wormColliders = gameObject.GetComponentsInChildren<BoxCollider2D>();
        foreach (var collider in wormColliders)
        {
            collider.gameObject.SetActive(false);//.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

}
