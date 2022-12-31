using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private float speed = 100f;

    private Vector2 _direction;
    private float _travelledDistance;
    private float _initialPosition;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _initialPosition = -1;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _direction = new Vector2(1.0f, 0);
        var position = transform.position;
        _travelledDistance = position.x;
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (_rb.position.x > distance)
        {
            _direction = new Vector2(-1.0f, 0);
        }
        else if (_rb.position.x < _initialPosition)
        {
            _direction = new Vector2(1.0f, 0);
        }
        _rb.velocity = speed * _direction;
        Debug.Log(speed);
        Debug.Log(_rb.velocity);
    }
}
