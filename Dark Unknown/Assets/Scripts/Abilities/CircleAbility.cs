using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleAbility : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private int _numberProjectile = 3;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private float _speedRotate = 50f;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
        for (int i = 0; i < _numberProjectile; i++)
        {
            float angle = i * Mathf.PI*2f / _numberProjectile;
            Vector2 newPos = new Vector2(Mathf.Cos(angle)*_radius, Mathf.Sin(angle)*_radius);
            GameObject projectile = Instantiate(_projectile, newPos, Quaternion.identity);
            projectile.gameObject.transform.parent = gameObject.transform;
            projectile.transform.localPosition = newPos;
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.forward * (_speedRotate * Time.deltaTime));
    }
}
