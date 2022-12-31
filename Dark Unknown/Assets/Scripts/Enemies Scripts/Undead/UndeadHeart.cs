using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UndeadHeart : EnemyController
{
    private Vector2 _targetPosition;
    private Vector2 _initialPosition;
    private Vector2 _vertexPosition;
    private Vector2 _offsetPosition;
    private float _a, _b, _c, _d, _x=0, _angle;
    [SerializeField] private float _velocity = 0.5f;
    [SerializeField] private float health;

    [SerializeField] private float recoveryTime;
    private UndeadController _parent;

    public void Init(UndeadController parent)
    {
        _parent = parent;
    }
    
    private void Start()
    {
        StartCoroutine(Countdown());

        /* Calculate the parameters of the trajectory considering the initial point at the origin (0,0)
         * The calculation of the trajectory is done by calculating the equation of a parabola passing
         * through three points which are: start point (_initialPosition), vertex of the parabola (_vertexPosition),
         * end point where the target is located (_targetPosition) */
        
        float h = 1f; //height of the parabolic trajectory
        _offsetPosition = transform.position; //to translate everything to the origin 
        
        _targetPosition = (Vector2)Player.Instance.transform.position-_offsetPosition;
        _initialPosition = new Vector2(0,0);
        float xVertex =
            ((_initialPosition.y + _targetPosition.y) / 2 + (_initialPosition.x - _targetPosition.x) /
                (_initialPosition.y - _targetPosition.y) * (_initialPosition.x + _targetPosition.x) / 2 - h) *
            ((_initialPosition.x - _targetPosition.x) * (_initialPosition.y - _targetPosition.y)) /
            ((float)Math.Pow((_initialPosition.y - _targetPosition.y), 2) +
             (float)Math.Pow((_initialPosition.x - _targetPosition.x), 2));
        float yVertex;
        if (_initialPosition.x - _targetPosition.x != 0) //SINGULAR CASE when _initialPosition and _targetPosition have the same x coordinate
            yVertex = (_initialPosition.y - _targetPosition.y) / (_initialPosition.x - _targetPosition.x) * xVertex + h;
        else yVertex = _targetPosition.y - _initialPosition.y / 2;
        _vertexPosition = new Vector2(xVertex,yVertex);

        // Rotating axis we have to calculate the new values of target and vertex (the initial point remain the same because it is in the origin)
        Vector2 targetPositionPrime, vertexPositionPrime;
        _angle = (float)Math.Atan2(_targetPosition.y - _initialPosition.y, _targetPosition.x - _initialPosition.x);
        vertexPositionPrime.x = _vertexPosition.x * (float)Math.Cos(-_angle) - _vertexPosition.y * (float)Math.Sin(-_angle);
        vertexPositionPrime.y = _vertexPosition.x * (float)Math.Sin(-_angle) + _vertexPosition.y * (float)Math.Cos(-_angle);
        targetPositionPrime.x = _targetPosition.x * (float)Math.Cos(-_angle) - _targetPosition.y * (float)Math.Sin(-_angle);
        targetPositionPrime.y = _targetPosition.x * (float)Math.Sin(-_angle) + _targetPosition.y * (float)Math.Cos(-_angle);
        
        //calculate parameter of the parabola passing from 3 points -> y = _a*_x^2 + _b*_x + _c
        _d = (_initialPosition.x - vertexPositionPrime.x) * (_initialPosition.x - targetPositionPrime.x) *
            (vertexPositionPrime.x - targetPositionPrime.x);
        if (_d != 0) 
        {
            _a = (targetPositionPrime.x * (vertexPositionPrime.y - _initialPosition.y) +
                  vertexPositionPrime.x * (_initialPosition.y - targetPositionPrime.y) +
                  _initialPosition.x * (targetPositionPrime.y - vertexPositionPrime.y)) / _d;
            _b = (targetPositionPrime.x * targetPositionPrime.x * (_initialPosition.y - vertexPositionPrime.y) +
                  vertexPositionPrime.x * vertexPositionPrime.x * (targetPositionPrime.y - _initialPosition.y) +
                  _initialPosition.x * _initialPosition.x * (vertexPositionPrime.y - targetPositionPrime.y)) / _d;
            _c = (vertexPositionPrime.x * targetPositionPrime.x * (vertexPositionPrime.x - targetPositionPrime.x) *
                  _initialPosition.y +
                  targetPositionPrime.x * _initialPosition.x * (targetPositionPrime.x - _initialPosition.x) *
                  vertexPositionPrime.y +
                  _initialPosition.x * vertexPositionPrime.x * (_initialPosition.x - vertexPositionPrime.x) *
                  targetPositionPrime.y) / _d;
        }
        else //SINGULAR CASE when _initialPosition and _targetPosition have the same x coordinate
        {
            _a = 0;
            _b = _targetPosition.y > _initialPosition.y ? 1 : -1;
            _c = 0;
        }
        transform.position = _initialPosition + _offsetPosition;
    }

    private void Update()
    {
        if (Math.Abs(transform.position.x-_offsetPosition.x) < Math.Abs(_targetPosition.x))
        {
            StartCoroutine(moveProjectile());
        }
        else
        {
            
            if (Math.Abs(transform.position.x - _offsetPosition.x) == Math.Abs(_targetPosition.x) &&
                Math.Abs(transform.position.y - _offsetPosition.y) < Math.Abs(_targetPosition.y))
                //SINGULAR CASE when _initialPosition and _targetPosition have the same x coordinate
            {
                StartCoroutine(moveProjectile());
            }
            else
            {
                //Destroy(gameObject);
            }
        }
    }

    private IEnumerator moveProjectile()
    {
        Vector2 position;
        if (_d != 0)
        {
            float xNorm = _x, yNorm = _a * (float)Math.Pow(_x, 2) + _b * _x + _c;
            position = new Vector2(
                xNorm * (float)Math.Cos(_angle) - yNorm * (float)Math.Sin(_angle),
                xNorm * (float)Math.Sin(_angle) + yNorm * (float)Math.Cos(_angle));
        }
        else //SINGULAR CASE when _initialPosition and _targetPosition have the same x coordinate
        {
            position = new Vector2(0, _x*_b);
        }
        transform.position = position + _offsetPosition;
        _x = _x + _velocity;
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(recoveryTime);
        _parent.Recover();
        Destroy();
    }


    public void Destroy()
    {
        Destroy(gameObject);
    }

    public override void TakeDamage(float damage, bool source)
    {
        health -= damage;
        if (health <= 0)
        {
            _parent.ReduceEnemyCounterPublic();
            Destroy();
        }
    }
    
    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        //TODO
        //rallenta
        yield return new WaitForSeconds(seconds);
        //torna a velocità normale
    }

    public override IEnumerator RecoverySequence()
    {
        throw new NotImplementedException();
    }
}
