using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : EnemyController
{
    private float _health = 40f;
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem particles;
    private static readonly int Dead = Animator.StringToHash("dead");
    private bool _isHittable = true;
    private ParticleSystem _particleSystem;

    //Start is called before the first frame update
    private void Start()
    {
        _isHittable = true;
        _particleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public override void TakeDamage(float damage)
    {
        if (!_isHittable) return;
        if (_health > 0)
        {
            Debug.Log("damage");
            _health -= damage;
            StartCoroutine(FlashRed());
        }
        else if (isDead==false)
        {
            Destroyed();
        }
    }
    
    /*private IEnumerator CrystalDestroyedCoroutine()
    {
        _isHittable = true;
        _particleSystem.Stop();
        yield return new WaitForSeconds(5f);
        _isHittable = false;
        _particleSystem.Play();
    }*/
    
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
    
    /*public IEnumerator CrystalDestroyedCoroutine()
    {
        Debug.Log("coroutine started");
        yield return new WaitForSeconds(5f);
        _isHittable = true;
        _particleSystem.Play();
    }*/

    public void DisableVulnerability()
    {
        Debug.Log("start");
        _isHittable = false;
        _particleSystem.Stop();
    }
    
    public void EnableVulnerability()
    {
        if (isDead) return;
        Debug.Log("stop");
        _isHittable = true;
        _particleSystem.Play();
    }
}
