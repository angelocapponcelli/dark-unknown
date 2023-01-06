using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    [SerializeField] private Pointer pointer;
    private Vector3 _targetPosition = new Vector3(0,0,0);
    [SerializeField] private float hideDistance;

    void Start()
    {
        pointer = GetComponentInChildren<Pointer>();
    }
    void Update()
    {
        var dir = _targetPosition - transform.position;

        if (dir.magnitude < hideDistance)
        {
            SetPointerActive(false);
        }
        else
        {
            SetPointerActive(true);
        
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void SetPointerActive(bool value)
    {
        pointer.gameObject.SetActive(value);
    }

    public void SetTargetPosition(Vector3 position)
    {
        _targetPosition = position;
    }
}
