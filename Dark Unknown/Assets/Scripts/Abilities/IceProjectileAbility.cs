using System.Collections;
using UnityEngine;

public class IceProjectileAbility : Ability
{
    [SerializeField] private GameObject _iceProjectile;
    [SerializeField] private float _speed = 8f;
    [SerializeField] private float _distance = 0.6f;
    [SerializeField] private GameObject _abilityReward;
    private GameObject _projectile;
    private Coroutine _coroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
    }

    public override void Activate()
    {
        Vector3 direction = (Vector3)Player.Instance.GetComponent<PlayerInput>().PointerPosition - transform.position;
        _projectile = Instantiate(_iceProjectile, transform.position, Quaternion.identity);
        _projectile.GetComponent<IceProjectile>().SetIceProjectileAbility(this);
        _projectile.GetComponent<Rigidbody2D>().velocity = (direction * 10).normalized * _speed;
        
        _coroutine = StartCoroutine(timeToReturn(_distance));
    }

    public void Deactivate()
    {
        if(isActive)
            StopCoroutine(_coroutine);
        isActive = false;
    }
    
    private IEnumerator timeToReturn(float seconds)
    {
        isActive = true;
        yield return new WaitForSeconds(seconds);
        _projectile.GetComponent<IceProjectile>().DestroyProjectileRoutine();
        isActive = false;
    }

    public override string GetText()
    {
        return "new ability that throws an ice ball at enemies";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
