using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    private bool _isWalking;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimatePlayer(float _x, float _y, Vector2 pointerPos, Vector2 _rbPos)
    {
        if (_x != 0 || _y != 0)
        {
            if (!_isWalking)
            {
                _isWalking = true;
                _animator.SetBool("isMoving", _isWalking);
            }
        }else if(_isWalking)
        {
            _isWalking = false;
            _animator.SetBool("isMoving", _isWalking);
            //StopMoving();
        }
        _animator.SetFloat("x", (pointerPos - _rbPos).normalized.x);
    }
}
