using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 150f;

    private Rigidbody2D _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void MovePlayer(Vector2 direction)
    {
        _rb.velocity = (speed * Time.deltaTime) * direction ;  // order of operations (float * float * vector) for the efficiency
    }
    
    public void StopMoving()
    {
        _rb.velocity = Vector2.zero;
    }

    public Vector2 GetRBPos()
    {
        return _rb.position;
    }
}
