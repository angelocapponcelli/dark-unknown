using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : EnemyController
{
    private float _health = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private Material flashMaterial;
    private static readonly int Dead = Animator.StringToHash("dead");
    private SpriteRenderer _spriteRenderer;
    private Material _originalMaterial;

    //Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _originalMaterial = _spriteRenderer.material;
        // _animator = GetComponent<Animator>();
        // _particles = GetComponent<ParticleSystem>();
    }

    public override void TakeDamageMelee(float damage)
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
}
