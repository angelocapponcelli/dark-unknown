using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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

}