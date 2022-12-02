using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalManager : EnemyController
{
    private float _health = 40f;
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particles;

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
        if (_health <= 0)
        {
            Destroyed();
        }
    }

    private void Destroyed()
    {
        GameObject.FindGameObjectWithTag("Boss").GetComponent<SkeletonBossController>().CrystalDestroyed();
        _particles.Stop();
        _animator.SetTrigger("dead");
    }
}
