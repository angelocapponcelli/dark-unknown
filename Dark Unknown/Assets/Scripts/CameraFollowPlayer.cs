using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothing = 1.0f;
    [SerializeField] private Vector3 _offset;
    private Vector3 _playerPos;

    private void Start()
    {
        if (_player == null) return;

        _offset = transform.position - _player.position;
    }

    private void FixedUpdate()
    {
        if (_player == null) return;

        _playerPos = _player.position + _offset;
        transform.position = Vector3.Lerp(transform.position, _playerPos, _smoothing * Time.deltaTime);
    }
}
