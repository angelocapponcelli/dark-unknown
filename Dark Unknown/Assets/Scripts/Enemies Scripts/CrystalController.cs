using System;
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
<<<<<<< HEAD:Dark Unknown/Assets/Scripts/Enemies Scripts/CrystalManager.cs
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;

    //Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        // _animator = GetComponent<Animator>();
        // _particles = GetComponent<ParticleSystem>();
=======
    private bool _isHittable;

    //Start is called before the first frame update
    private void Start()
    {
        DisableVulnerability();
>>>>>>> develop:Dark Unknown/Assets/Scripts/Enemies Scripts/CrystalController.cs
    }

    public override void TakeDamageMelee(float damage)
    {
        if (!_isHittable) return;
        if (_currentHealth > 0)
        {
<<<<<<< HEAD:Dark Unknown/Assets/Scripts/Enemies Scripts/CrystalManager.cs
            Debug.Log("damage");
            _health -= damage;
            StartCoroutine(Flash());
=======
            _currentHealth -= damage;
            StartCoroutine(FlashRed());
>>>>>>> develop:Dark Unknown/Assets/Scripts/Enemies Scripts/CrystalController.cs
        }
        else if (!isDead)
        {
            Destroyed();
        }
    }

    public override IEnumerator RecoverySequence()
    {
        yield break; 
    }
    
    public override void TakeDamageDistance(float damage)
    {
        if (_health > 0)
        {
            Debug.Log("damage");
            _health -= damage;
            StartCoroutine(Flash());
        }
        else if (isDead==false)
        {
            Destroyed();
        }
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
