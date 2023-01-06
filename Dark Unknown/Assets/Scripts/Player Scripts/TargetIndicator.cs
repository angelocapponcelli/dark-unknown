using System.Collections;
using System.Collections.Generic;
using Player_Scripts;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    private Pointer _pointer;
    [SerializeField] public Transform target;
    [SerializeField] public float hideDistance;

    void Start()
    {
        _pointer = GetComponentInChildren<Pointer>();
    }
    void Update()
    {
        var dir = target.position - transform.position;

        if (dir.magnitude < hideDistance)
        {
            SetChildrenActive(false);
        }
        else
        {
            SetChildrenActive(true);
        
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetChildrenActive(bool value)
    {
        _pointer.gameObject.SetActive(value);
    }
}
