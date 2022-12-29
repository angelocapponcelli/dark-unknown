using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleAbility : Ability
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private int _numberProjectile = 3;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private float _speedRotate = 50f;
    private List<GameObject> _activeProjectile = new List<GameObject>();
    
    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
    }

    public override void Activate()
    {
        for (int i = 0; i < _numberProjectile; i++)
        {
            float angle = i * Mathf.PI*2f / _numberProjectile;
            Vector2 newPos = new Vector2(Mathf.Cos(angle)*_radius, Mathf.Sin(angle)*_radius);
            GameObject projectile = Instantiate(_projectile, newPos, Quaternion.identity);
            projectile.gameObject.transform.parent = gameObject.transform;
            projectile.transform.localPosition = newPos;
            _activeProjectile.Add(projectile);
        }

        isActive = true;
    }

    private void Update()
    {
        if (isActive)
        {
            transform.Rotate(Vector3.forward * (_speedRotate * Time.deltaTime));
            foreach (GameObject projectile in _activeProjectile.ToList())
            {
                if (projectile == null)
                    _activeProjectile.Remove(projectile);
            }

            if (_activeProjectile.Count == 0)
                isActive = false;
        }
    }
}
