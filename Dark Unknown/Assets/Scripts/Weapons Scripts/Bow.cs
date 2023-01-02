using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bow : Weapon
{
    //private Animator _weaponAnimator;
    //[SerializeField] private Animator _effectAnimator;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float travellingTime = 1.0f;

    private void Start()
    {
        _delay = 0.8f;
        //_damage = 10f; non � pi� usato in questa classe, vedi WeaponAttack
        _canRotateFreely = true;
        
        //_weaponAnimator = GetComponent<Animator>();

    }


    public override void Attack()
    {
        if (_attackBlocked)
            return;
        GameObject arrow = Instantiate(_arrow, transform.position, Quaternion.Euler(0f,0f,-90f) );
        arrow.GetComponent<Rigidbody2D>().velocity = (direction*10).normalized * 8;
        arrow.transform.Rotate(0.0f,0.0f,rotation);
        Destroy(arrow, travellingTime);

        //_effectAnimator.SetTrigger("Attack");
        _attackBlocked = true;
        AudioManager.Instance.PlayPLayerAttackBowSound();

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
    
    
    

    /*ToDisplaycircleOfAttack
    private void OnDrawGizmosSelected()
    {
        if (_attackPoint == null)
            return;
        Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }*/
}
