using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private Animator _animator;
    private bool _isWalking;
    private float _x, _y;
    private Vector2 _pointerPos;
    private WeaponParent _weaponParent;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _weaponParent = GetComponentInChildren<WeaponParent>();
    }

    // Update handles the animation changes based on the mouse pointer 
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");
        Vector2 pointerPos = GetPointerInput();
        _weaponParent.PointerPosition = pointerPos;

        if (_x != 0 || _y != 0)
        {
            if (!_isWalking)
            {
                _isWalking = true;
                _animator.SetBool("isMoving", _isWalking);
            }
        }else if(_isWalking)
        {
            _isWalking = false;
            _animator.SetBool("isMoving", _isWalking);
            StopMoving();
        }
        _animator.SetFloat("x", (pointerPos - _rb.position).normalized.x);

        _direction = new Vector2(_x, _y).normalized;

    }
    
    // FixedUpdate handles the movement 
    private void FixedUpdate()
    {
        _rb.velocity = (speed * Time.deltaTime) * _direction ;  // order of operations (float * float * vector) for the efficiency
    }
    
    // Stops the movement when WASD are not pressed
    private void StopMoving()
    {
        _rb.velocity = Vector2.zero;
    }

    private Vector2 GetPointerInput()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}

