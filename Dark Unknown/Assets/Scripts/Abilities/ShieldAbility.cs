using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldAbility : MonoBehaviour
{
    [SerializeField] private float _delay = 2f;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    private bool _isActive = false;

        // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector2(0,0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        activation(false);
    }

    public void Activate()
    {
        if (!_spriteRenderer.enabled)
        {
            activation(true);
            StartCoroutine(delayedDeactivation());
        }
    }

    public bool isActive()
    {
        return _isActive;
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
        _isActive = value;
    }
}
