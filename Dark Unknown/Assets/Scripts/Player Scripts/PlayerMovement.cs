using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 150f;
    
    private float _activeSpeed;
    [SerializeField] private float dashSpeed;
    private float _dashDuration = .15f, _dashCooldown = 1f;
    private bool _isDashing, _canDash = true;
    private Vector2 _dashDirection;
    [SerializeField]private ParticleSystem _dust;

    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _activeSpeed = speed;
    }

    public void MovePlayer(Vector2 direction, bool shiftDown)
    {
        

        if (shiftDown && _canDash)
        {
            _isDashing = true;
            _canDash = false;
            StartCoroutine(StopDashing());
            _dashDirection = direction;
            CreateDust();
            AudioManager.Instance.PlayPLayerDashSound();
        }

        if (_isDashing)
        {
            _activeSpeed = dashSpeed;
            _rb.velocity = (_activeSpeed * Time.deltaTime) * _dashDirection ; // order of operations (float * float * vector) for the efficiency
            return;
        }
        else
        {
            _activeSpeed = speed;
            //AudioManager.Instance.PlayPLayerWalkSound(); //TODO sistemare il suono dei passi che va in loop 
        }
        
        _rb.velocity = (_activeSpeed * Time.deltaTime) * direction ;  // order of operations (float * float * vector) for the efficiency

    }


    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(_dashDuration);
        _isDashing = false;
        yield return new WaitForSeconds(_dashCooldown);
        _canDash = true;


    }

    public Vector2 GetRBPos()
    {
        return _rb.position;
    }

    private void CreateDust()
    {
        _dust.Play();
    }

    public void IncreaseSpeed(float multiplier)
    {
        speed = speed * multiplier;
    }
}
