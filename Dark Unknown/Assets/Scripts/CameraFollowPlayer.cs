using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothing;
    [SerializeField] private Vector3 _offset;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player != null)
        {
            Vector3 newPosition = Vector3.Lerp(transform.position, _player.transform.position + _offset, _smoothing);
            transform.position = newPosition;
        }
    }
}
