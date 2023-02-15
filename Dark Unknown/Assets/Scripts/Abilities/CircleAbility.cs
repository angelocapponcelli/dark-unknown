using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CircleAbility : Ability
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private int _numberProjectile = 3;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private float _speedRotate = 50f;
    private List<GameObject> _activeProjectile = new List<GameObject>();
    [SerializeField] private GameObject _abilityReward;
    [SerializeField] private float timeAlive = 20f;
    private Coroutine _deactivation;
    
    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
    }

    public override void Activate()
    {
        for (var i = 0; i < _numberProjectile; i++)
        {
            var angle = i * Mathf.PI*2f / _numberProjectile;
            var newPos = new Vector2(Mathf.Cos(angle)*_radius, Mathf.Sin(angle)*_radius);
            var projectile = Instantiate(_projectile, newPos, Quaternion.identity);
            projectile.gameObject.transform.parent = gameObject.transform;
            projectile.transform.localPosition = newPos;
            _activeProjectile.Add(projectile);
        }

        isActive = true;
        _deactivation = StartCoroutine(DelayedDeactivation(timeAlive));
    }

    private void Update()
    {
        if (!isActive) return;
        transform.Rotate(Vector3.forward * (_speedRotate * Time.deltaTime));
        foreach (var projectile in _activeProjectile.ToList())
        {
            if (projectile == null)
                _activeProjectile.Remove(projectile);
        }

        if (_activeProjectile.Count != 0) return;
        isActive = false;
        StopCoroutine(_deactivation);
    }
    
    private IEnumerator DelayedDeactivation(float seconds)
    {
        Debug.Log("Deactivation coroutine started");
        yield return new WaitForSeconds(seconds);
        foreach (var projectile in _activeProjectile.ToList())
        {
            Destroy(projectile);
        }
        Debug.Log("Projectiles deactivated");
        isActive = false;
    }

    public override string GetText()
    {
        return "a new ability that creates a circle of fireballs around you.";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
