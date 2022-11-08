using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    private Vector2 _direction;
    private Animator _animator;
    private enum Facing { UP, DOWN, LEFT, RIGHT };
    private Facing _facingDirection = Facing.DOWN;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TakeInput();
        
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(_direction * _speed * Time.deltaTime);

        if (_direction != Vector2.zero)
        {
            SetAnimatorMovement(_direction);
        }
        else
        {
           // _animator.SetLayerWeight(1, 0);
        }
    }

    private void TakeInput()
    {
        _direction = Vector2.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector2.up;
            _facingDirection = Facing.UP;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector2.left;
            _facingDirection = Facing.LEFT;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector2.down;
            _facingDirection = Facing.DOWN;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector2.right;
            _facingDirection = Facing.RIGHT;
        }
        _direction = _direction.normalized;
    }

    private void SetAnimatorMovement(Vector2 direction)
    {
        //_animator.SetLayerWeight(1, 1);
        _animator.SetFloat("xDirection", direction.x);
        _animator.SetFloat("yDirection", direction.y);
    }
}
