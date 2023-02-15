using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Sword : Weapon
{
    private Animator _weaponAnimator;
    [SerializeField] private Animator _effectAnimator;

    private void Start()
    {
        _delay = 0.6f;
        _canRotateFreely = false;
        
        _weaponAnimator = GetComponent<Animator>();

    }


    public override void Attack()
    {
        if (_attackBlocked)
            return;
        
        _weaponAnimator.SetTrigger("Attack");
        _effectAnimator.SetTrigger("Attack");
        _attackBlocked = true;
        AudioManager.Instance.PlayPLayerAttackSwordSound();

        /*Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Enemy"))
            {
                //TODO bisogna generalizzare non solo per skeletonController ma per tutti i nemici
                enemy.GetComponent<EnemyController>().TakeDamage(Random.Range(Mathf.Max(2, _damage-5), Mathf.Max(5, _damage + 5))); 
            }
        }*/

        StartCoroutine(DelayAttack());
    }
}
