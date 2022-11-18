using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : EnemyController
{
    [SerializeField] private GameObject target;
    [SerializeField] private float _speed;
    [SerializeField] private float minDistance;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    private float _distance;
    private bool _isDead;
    private bool _isRecovering;
    private float _currentSpeed;

    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private static readonly int Attack = Animator.StringToHash("Attack");

    private static readonly int Hurt = Animator.StringToHash("Hurt");

    private static readonly int Death = Animator.StringToHash("Death");

    private static readonly int Recover = Animator.StringToHash("Recover");

    private IEnumerator _recoverySequence;
    //private Vector2 _direction;

    [SerializeField] private float _maxHealth;
    private float _currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        _recoverySequence = RecoverySequence();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _currentHealth = _maxHealth;
        _currentSpeed = _speed;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculates distance and direction of movement
        var targetPosition = target.transform.position;
        var ownPosition = transform.position;
        _distance = Vector2.Distance(ownPosition, targetPosition);
        _direction = targetPosition - ownPosition;
        _direction.Normalize();
        

        // If the skeleton is dead or recovering, it stands still 
        if (_isDead || _isRecovering) 
        {
            _currentSpeed = 0;
            _animator.SetBool(IsMoving, false);
        } 
        // Otherwise it follows the player till it reaches a minimum distance
        else if (_distance > minDistance)
        {
            _currentSpeed = _speed;
            _animator.SetBool(IsMoving, true);
            flip(_direction);     
        } 
        else
        {
            // At the minimum distance, it stops moving
            _currentSpeed = 0;
            _animator.SetBool(IsMoving, false);
            attack();

            //direction = Vector2.Perpendicular(direction);
        }
        transform.Translate(_direction * (_currentSpeed * Time.deltaTime));

        // -- Handle Animations --
        // Attack
        if(Input.GetKeyDown("q")) 
        {
            _animator.SetTrigger(Attack);
            // ExecuteAttack();
        }
        // Hurt
        if (Input.GetKeyDown("e"))
            _animator.SetTrigger(Hurt);
        // Death
        if (Input.GetKeyDown("z")) {
            if(!_isDead)
                _animator.SetTrigger(Death);
            else
            {
                _animator.SetTrigger(Recover);
                _isRecovering = true;
                StartCoroutine(_recoverySequence);
            }
            _isDead = !_isDead;
        }
    }

    private void attack()
    {
        _animator.SetTrigger(Attack);
    }

    IEnumerator RecoverySequence()
    { 
        Debug.Log("Recovering");
        yield return new WaitForSeconds(3);
        Debug.Log("Recovered");
        _isRecovering = false;
    }

    private void flip(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (Mathf.Abs(angle) < 90)
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x)*-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }
    }
 
    public override void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        } else
        {
            _animator.SetTrigger(Hurt);
        }
    }

    private void Die()
    {
        _isDead = true;
        _animator.SetTrigger(Death);
    }
    /* void FixedUpdate()
{
   _rb.velocity = (_speed * Time.deltaTime) * _direction;
} */
}
