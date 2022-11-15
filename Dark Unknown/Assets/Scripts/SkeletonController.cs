using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _minDistance;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    private float _distance;
    private bool _isDead = false;
    private bool _isRecovering = false;
    //private Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calculates distance and direction of movement
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        _direction = _target.transform.position - transform.position;
        _direction.Normalize();
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        // If the skeleton is dead or recovering, it stands still 
        if (_isDead || _isRecovering) 
        {
            _speed = 0;
            _animator.SetBool("isMoving", false);
        } 
        else if (_distance > _minDistance)
        {            
            //transform.position = Vector2.MoveTowards(this.transform.position, _target.transform.position, _speed*Time.deltaTime);
            _speed = 2;
            _animator.SetBool("isMoving", true);
            if (Mathf.Abs(angle) < 90)
            {
                gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            } 
            else
            {
                gameObject.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }            
        } 
        else
        {
            _speed = 0;
            _animator.SetBool("isMoving", false);
            //direction = Vector2.Perpendicular(direction);
        }
        transform.Translate(_direction * _speed * Time.deltaTime);

        // -- Handle Animations
        // Attack
        if(Input.GetKeyDown("q")) 
        {
            _animator.SetTrigger("Attack");
            // ExecuteAttack();
        }
        // Hurt
        if (Input.GetKeyDown("e"))
            _animator.SetTrigger("Hurt");
        // Death
        if (Input.GetKeyDown("z")) {
            if(!_isDead)
                _animator.SetTrigger("Death");
            else
            {
                _animator.SetTrigger("Recover");
                _isRecovering = true;
                StartCoroutine(RecoverySequence());
            }
            _isDead = !_isDead;
        }
    }

    IEnumerator RecoverySequence()
    { 
        Debug.Log("Recovering");
        yield return new WaitForSeconds(3);
        Debug.Log("Recovered");
        _isRecovering = false;
    }

    /* void FixedUpdate()
    {
        _rb.velocity = (_speed * Time.deltaTime) * _direction;
    } */
}
