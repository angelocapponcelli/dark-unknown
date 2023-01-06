using System.Collections;
using UnityEngine;

public class DUController : EnemyController
{
    [SerializeField] private GameObject beam;
    private readonly Vector3 _beamPos = new Vector3(0, 0, 0);
    [SerializeField] private GameObject eyeProjectile;
    private float velocityProjectileMax = 5f, velocityProjectileMin = 2f;
    
    private float _currentHealth;
    [SerializeField] private float _maxHealth = 200f;
    
    private BossUIController _bossUIController = null;
    private bool _deathSoundPlayed = false;
    
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;
    [SerializeField] private Material flashMaterial;

    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _maxHealth;
        
        if (GetComponent<BossUIController>() == null) return;
        _bossUIController = GetComponent<BossUIController>();
        _bossUIController.SetName("Dark Unknown");
        _bossUIController.SetMaxHealth(_maxHealth);
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
    }

    // Update is called once per frame
    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Instantiate(beam, transform.position, beam.transform.rotation);
        }
    }*/

    public void BeamAttack()
    {
        Instantiate(beam, _beamPos, beam.transform.rotation);
        _animator.SetInteger("stateNumber", 1);
    }
    
    public void EyeAttack()
    {
        StartCoroutine(generateProjectile());
        //_animator.SetInteger("stateNumber", 2); //se viene implementato il terzo attacco passare allo stato 2 invece che allo 0
        _animator.SetInteger("stateNumber", 0);
    }
    
    public void SpinAttack()
    {
        //Instantiate(beam, transform.position, beam.transform.rotation);
        _animator.SetInteger("stateNumber", 0);
    }

    private IEnumerator generateProjectile()
    {
        for (int j = 0; j < 4; j++)
        {
            float angle = Random.Range(0f, 2f * Mathf.PI);
            for (int i = 0; i < Random.Range(30,35); i++)
            {
                float angleDir, randomQuantity = Random.Range(-Mathf.PI, Mathf.PI);
                angleDir = angle + randomQuantity;
                GameObject projectile = Instantiate(eyeProjectile, transform.position, beam.transform.rotation);
                Vector2 direction = new Vector2 (Mathf.Cos(angleDir), Mathf.Sin(angleDir));
                projectile.GetComponent<Rigidbody2D>().velocity = direction * Random.Range(velocityProjectileMin, velocityProjectileMax);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
        }
        _animator.SetTrigger("nextState");
    }
    
    public override void TakeDamageMelee(float damage)
    {
        TakeDamage(damage);
    }
    
    public override void TakeDamageDistance(float damage)
    {
        TakeDamage(damage);
    }

    private void TakeDamage(float damage)
    {
        if (isDead) return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            if (_bossUIController != null) _bossUIController.SetHealth(0);
            Die();
            DisableBoxCollider();
            _bossUIController.DeactivateHealthBar();
        } else
        {
            if (_bossUIController != null)  _bossUIController.SetHealth(_currentHealth);
            DamageDistance();
        }
    }
    
    private void DamageDistance()
    {
        StartCoroutine(Flash());
        AudioManager.Instance.PlaySkeletonHurtSound();
    }
    
    private IEnumerator Flash()
    {
        _spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _originalMaterial;
    }

    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator RecoverySequence()
    {
        throw new System.NotImplementedException();
    }

    public override void CrystalDestroyed()
    {
        throw new System.NotImplementedException();
    }
    
    private void Die()
    {
        isDead = true;
        _animator.SetTrigger("die");
        if (_deathSoundPlayed) return;
        AudioManager.Instance.PlaySkeletonDieSound();
        _deathSoundPlayed = true;
        
        // TODO il finale
        //ReduceEnemyCounter(LevelManager.Instance.GetCurrentRoom());
        //GameManager.Instance.LoadVictoryScene();
    }
    
    private void DisableBoxCollider()
    {
        var collider = GetComponent<CircleCollider2D>();
        collider.enabled = false;
    }
}
