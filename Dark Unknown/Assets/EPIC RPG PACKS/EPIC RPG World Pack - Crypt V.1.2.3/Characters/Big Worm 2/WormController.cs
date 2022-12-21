using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{
    [SerializeField] private Player _target;
    [SerializeField] private float _triggerDistance;
    [SerializeField] private float _maxHealth;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private float _projectileSpeed = 2f;
    [SerializeField] private Transform _spawnProjectilePoint;

    private bool _isDead;
    private Vector2 _direction;
    private float _distance;
    private bool _isAttacking;
    private float _currentHealth;
    private bool _canMove;
    private bool _canAttack;
    private bool _damageCoroutineRunning;
    private float _timeElapsedFromShot;
    private float _shotFrequency = 3;
    private float _shotsNumber;
    private bool _isHidden;

    private EnemyMovement _movement;
    private WormAnimator _animator;
    private SpriteRenderer _spiderRenderer;

    private bool _deathSoundPlayed = false;
    
    
    private void Start()
    {
        _isDead = false;
        _target = Player.Instance;
        _isHidden = true;
        _canAttack = false;

        _currentHealth = _maxHealth;
        _canMove = true;
        _shotsNumber = 0;

        _movement = GetComponent<EnemyMovement>();
        _animator = GetComponent<WormAnimator>();
        _spiderRenderer = GetComponent<SpriteRenderer>();

        _timeElapsedFromShot = 0;

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

        if (!_isDead && _distance <= _triggerDistance)
        {
            _animator.flip(_direction);
            if (_isHidden)
            {
                Show();
            }

            if (_timeElapsedFromShot >= _shotFrequency && _shotsNumber <= 3 && _canAttack)
            {
                Attack();
                _shotsNumber += 1;
                _timeElapsedFromShot = 0;
            } else if (_shotsNumber > 3)
            {
                _shotsNumber = 0;
                Hide();
            }
            
            
        } else if (!_isDead && _distance > _triggerDistance)
        {
            print("Hide bitch");
            if (!_isHidden)
            {
                Hide();   
            }
        }
        
        
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

    private void Attack()
    {
        _animator.Attack();
        GameObject projectile = Instantiate(_projectile, _spawnProjectilePoint.position, Quaternion.identity);
        projectile.GetComponent<SpriteRenderer>().color = Color.green;
        projectile.GetComponent<Rigidbody2D>().velocity = _direction*_projectileSpeed;
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

}
