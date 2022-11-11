using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _minDistance;

    private float _distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _distance = Vector2.Distance(transform.position, _target.transform.position);
        Vector2 direction = _target.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (_distance > _minDistance)
        {            
            //transform.position = Vector2.MoveTowards(this.transform.position, _target.transform.position, _speed*Time.deltaTime);
            if (Mathf.Abs(angle) < 90)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else
            {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }            
        } else
        {
            direction = Vector2.Perpendicular(direction);
        }
        transform.Translate(direction * _speed * Time.deltaTime);
    }
}
