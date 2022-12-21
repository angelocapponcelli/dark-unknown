using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class AxeAttack : MonoBehaviour
{
    //ver1: axe launch based on the pointer / ver2: axe launch based on the distance
    [SerializeField] private float _damage = 10f;
    private bool _returnToPlayer = false;
    private Vector2 _target;
    private Rigidbody2D _rigidbody2D;
    private int _countTriggerStay = 0;
    [SerializeField] private float _distance=0.45f; //ver2

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(timeToReturn(_distance)); //ver2
    }

    private void Update()
    {
        if (!_returnToPlayer)
        {
           /* ver1
            Vector2 newPosition = Vector2.MoveTowards(transform.position, _target, Time.deltaTime * 8);
           if (transform.position == (Vector3) _target)
           {
               _returnToPlayer = true;
           }
           else
           {
               _rigidbody2D.MovePosition(newPosition);
           }*/
        }
        else
        {
            _rigidbody2D.velocity = (transform.position - Player.Instance.transform.position).normalized * -8;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitObjects = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D objectHit in hitObjects)
        {
            if (objectHit.gameObject.CompareTag("EnemyCollider"))
            {
                objectHit.GetComponentInParent<EnemyController>()
                    .TakeDamageDistance(_damage * Player.Instance.GetStrengthMultiplier());
            }
            else if (objectHit.gameObject.CompareTag("Player") && _returnToPlayer)
            {
                if (Player.Instance.GetComponentInChildren<WeaponParent>().GetComponentInChildren<Axe>())
                {
                    Player.Instance.GetComponentInChildren<WeaponParent>().GetComponentInChildren<Axe>().enableSprite(true);
                }
                Destroy(gameObject);
            } else if (objectHit.gameObject.layer == 8) //layer 8: Obstacle
            {
                _returnToPlayer = true;
            }
        }

        if (collision.CompareTag("Trap")) return;
    }
    
    public void setTarget(Vector2 target)
    {
        _target = target;
    }

    private void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "PlayerFeetCollider")
        {
            _countTriggerStay++;
            if (_countTriggerStay > 20)
            {
                if (Player.Instance.GetComponentInChildren<WeaponParent>().GetComponentInChildren<Axe>())
                {
                    Player.Instance.GetComponentInChildren<WeaponParent>().GetComponentInChildren<Axe>().enableSprite(true);
                }
                Destroy(gameObject);
            }
        }
    }
    
    //ver2
    private IEnumerator timeToReturn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        _returnToPlayer = true;
    }
}
