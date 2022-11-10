using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private Animator _animator;
    private bool _isWalking;
    private float _x, _y;
    private Vector2 _pointerPos;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TakeInput();

    }
    private void FixedUpdate()
    {
        
        _x = Input.GetAxisRaw("Horizontal");
        _y = Input.GetAxisRaw("Vertical");


        Vector2 pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        

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
        _rb.velocity = _direction * speed * Time.deltaTime;
    }

    private void StopMoving()
    {
        _rb.velocity = Vector2.zero;
    }
}

