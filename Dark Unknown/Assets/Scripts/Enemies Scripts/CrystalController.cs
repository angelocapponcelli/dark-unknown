using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : EnemyController
{
    private float _currentHealth = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Material flashMaterial;
    private static readonly int Dead = Animator.StringToHash("dead");
    private bool _isHittable;
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;

    //Start is called before the first frame update
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        DisableVulnerability();
    }

    public override void TakeDamageMelee(float damage)
    {
        if (!_isHittable) return;
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;
            StartCoroutine(Flash());
        }
        else if (!isDead)
        {
            Destroyed();
        }
    }
    
    public override void TakeDamageDistance(float damage)
    {
        if (!_isHittable) return;
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;
            StartCoroutine(Flash());
        }
        else if (!isDead)
        {
            Destroyed();
        }
    }
    
    public override IEnumerator Freeze(float seconds, float slowdownFactor)
    {
        yield break;
    }

    public override IEnumerator RecoverySequence()
    {
        yield break; 
    }
    
    private IEnumerator Flash()
    {
        _spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.material = _originalMaterial;
    }

    private void Destroyed()
    {
        isDead = true;
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SkeletonBossController>().CrystalDestroyed();
        particles.Stop();
        animator.SetTrigger(Dead);
    }

    public void DisableVulnerability()
    {
        _isHittable = false;
        particles.Stop();
    }
    
    public void EnableVulnerability()
    {
        if (isDead) return;
        Debug.Log("stop");
        _isHittable = true;
        particles.Play();
    }
}
