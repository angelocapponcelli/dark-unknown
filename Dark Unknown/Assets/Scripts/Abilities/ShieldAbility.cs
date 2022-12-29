using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : Ability
{
    [SerializeField] private float _delay = 2f;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;

        // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector2(0,0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        activation(false);
    }

    public override void Activate()
    {
        if (!_spriteRenderer.enabled)
        {
            activation(true);
            StartCoroutine(delayedDeactivation());
        }
    }

    
    private IEnumerator delayedDeactivation()
    {
        yield return new WaitForSeconds(_delay);
        activation(false);
    }

    private void activation(bool value)
    {
        _spriteRenderer.enabled = value;
        _collider.enabled = value;
        isActive = value;
    }
}
