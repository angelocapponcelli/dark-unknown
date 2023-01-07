using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 300f;
    private bool _isDashing, _canDash = true;
    private Vector2 _dashDirection;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 0.10f;
    [SerializeField] private ParticleSystem dust;
    private Rigidbody2D _rb;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 direction)
    {
        // order of operations (float * float * vector) for the efficiency
        if (_isDashing) return;
        _rb.velocity = (speed * Time.deltaTime) * direction;
    }

    public void Dash(Vector2 direction)
    {
        if (_canDash)
        {
            StartCoroutine(DashCoroutine(direction));
        }
    }
    
    private IEnumerator DashCoroutine(Vector2 direction)
    {
        _canDash = false;
        _isDashing = true;
        _dashDirection = direction;
        _rb.velocity = dashingPower * _dashDirection;
        dust.Play();
        AudioManager.Instance.PlayPLayerDashSound();
        yield return new WaitForSeconds(dashingTime);
        _isDashing = false;
        dust.Stop();
        UIController.Instance.DashCoolDown();
        yield return new WaitForSeconds(dashingCooldown);
        _canDash = true;
    }
    
    public Vector2 GetRbPos()
    {
        return _rb.position;
    }

    public void IncreaseSpeed(float multiplier)
    {
        speed *= multiplier;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public void SetSpeed(float resetSpeed)
    {
        speed = resetSpeed;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }
}
