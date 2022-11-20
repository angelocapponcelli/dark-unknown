using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothing = 1.0f;
    [SerializeField] private Vector3 _offset;
    private Vector3 _playerPos;

    [SerializeField] private CompositeCollider2D _mapBounds;
    [SerializeField] private float xZoomOffsetCam = 5;
    [SerializeField] private float yZoomOffsetCam = 2.5f;
    private float _xMin, _xMax, _yMin, _yMax;

    private void Start()
    {
        _xMin = _mapBounds.bounds.min.x + xZoomOffsetCam;
        _xMax = _mapBounds.bounds.max.x - xZoomOffsetCam;
        _yMin = _mapBounds.bounds.min.y + yZoomOffsetCam;
        _yMax = _mapBounds.bounds.max.y - yZoomOffsetCam;
    }

    private void FixedUpdate()
    {
        if (_player.IsUnityNull()) return;

        _playerPos = _player.transform.position + _offset;
        _playerPos.x = Mathf.Clamp(_playerPos.x, _xMin, _xMax);
        _playerPos.y = Mathf.Clamp(_playerPos.y, _yMin, _yMax);
        transform.position = Vector3.Lerp(transform.position, _playerPos, _smoothing * Time.deltaTime);
    }
    
}
