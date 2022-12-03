using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : EnemyController
{
    private float _health = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particles;
    private static readonly int Dead = Animator.StringToHash("dead");

    /* Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _particles = GetComponent<ParticleSystem>();
    }*/

    public override void TakeDamage(float damage)
    {
        Debug.Log("damage");
        _health -= damage;
        StartCoroutine(FlashRed());
        if (_health <= 0)
        {
            Destroyed();
        }
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
}
