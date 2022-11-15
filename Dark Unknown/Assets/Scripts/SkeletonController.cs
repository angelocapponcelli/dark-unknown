using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float minDistance;

    private Rigidbody2D _rb;
    private Animator _animator;
    private Vector2 _direction;
    private float _distance;
    private bool _isDead;
    private bool _isRecovering;

    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private static readonly int Attack = Animator.StringToHash("Attack");

    private static readonly int Hurt = Animator.StringToHash("Hurt");

    private static readonly int Death = Animator.StringToHash("Death");

    private static readonly int Recover = Animator.StringToHash("Recover");

    private IEnumerator _recoverySequence;
    //private Vector2 _direction;

    // Start is called before the first frame update
    void Start()
    {
        _recoverySequence = RecoverySequence();
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
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
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        // If the skeleton is dead or recovering, it stands still 
        if (_isDead || _isRecovering) 
        {
            speed = 0;
            _animator.SetBool(IsMoving, false);
        } 
        // Otherwise it follows the player till it reaches a minimum distance
        else if (_distance > minDistance)
        {            
            //transform.position = Vector2.MoveTowards(this.transform.position, _target.transform.position, _speed*Time.deltaTime);
            speed = 2;
            _animator.SetBool(IsMoving, true);
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
            // At the minimum distance, it stops moving
            speed = 0;
            _animator.SetBool(IsMoving, false);
            //direction = Vector2.Perpendicular(direction);
        }
        transform.Translate(_direction * (speed * Time.deltaTime));

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
