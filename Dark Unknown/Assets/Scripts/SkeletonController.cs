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
    private bool _isWalking;
    private float _x, _y;
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
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        _direction = _target.transform.position - transform.position;
        _direction.Normalize();
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

        if (_distance > _minDistance)
        {            
            //transform.position = Vector2.MoveTowards(this.transform.position, _target.transform.position, _speed*Time.deltaTime);
            _speed = 2;
            _isWalking = true;
            _animator.SetBool("isMoving", _isWalking);
            if (Mathf.Abs(angle) < 90)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }            
        } else
        {
            _speed = 0;
            _isWalking = false;
            _animator.SetBool("isMoving", _isWalking);
            //direction = Vector2.Perpendicular(direction);
        }
        transform.Translate(_direction * _speed * Time.deltaTime);
    }

    /* void FixedUpdate()
    {
        _rb.velocity = (_speed * Time.deltaTime) * _direction;
    } */
}
