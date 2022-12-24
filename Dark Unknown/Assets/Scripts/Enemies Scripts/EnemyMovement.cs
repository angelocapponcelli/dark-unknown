using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;
    private float _originalSpeed;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalSpeed = _speed;
    }

    public void MoveEnemy(Vector2 direction)
    {
        //transform.Translate((_speed * Time.deltaTime) * direction);

        //TODO modify skeleton movement to detect collision
        //_rb.MovePosition(_rb.position + ((_speed * Time.deltaTime) * direction));
        _rb.velocity = direction * _speed;
    }

    public void StopMovement()
    {
        _rb.velocity = Vector2.zero;
    }

    public void setSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void resetSpeed()
    {
        _speed = _originalSpeed;
    }

}
