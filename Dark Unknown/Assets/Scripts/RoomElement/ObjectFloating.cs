using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFloating : MonoBehaviour
{
    [SerializeField] private float _amplitude = 0.005f;
    [SerializeField] private float _speed = 3f;
    private Vector3 _tempPos = new Vector3();
    private float _tempVal;

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            _tempPos = transform.position;
            _tempPos.y = _tempPos.y + _amplitude * Mathf.Sin(_speed * Time.time);
            transform.position = _tempPos;
        }
    }
}
