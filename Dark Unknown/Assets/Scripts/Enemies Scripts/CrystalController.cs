using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : EnemyController
{
    private float _currentHealth = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particles;
    private static readonly int Dead = Animator.StringToHash("dead");
    private bool _isHittable;

    //Start is called before the first frame update
    private void Start()
    {
        DisableVulnerability();
    }

    public override void TakeDamage(float damage, bool damageFromArrow)
    {
        if (!_isHittable) return;
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;
            StartCoroutine(FlashRed());
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
    
    private IEnumerator FlashRed()
    {
        SpriteRenderer crystalRenderer = GetComponent<SpriteRenderer>();
        crystalRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        crystalRenderer.color = Color.white;
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
