using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 2f;

    private void Start()
    {
        StartCoroutine(DeactivateCollider(0.1f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitEnemies = collision.GetComponentsInChildren<Collider2D>();
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("EnemyCollider"))
            {
                //Debug.Log("damage");
                enemy.GetComponentInParent<EnemyController>().TakeDamageDistance(_damage*Player.Instance.GetStrengthMultiplier());
            }
        }
        if (collision.CompareTag("Trap") || collision.gameObject.layer == 9) return; //layer.9 == "Arrow" Layer where circleAbility projectiles are
        Destroy(gameObject);
    }
    
    private IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<BoxCollider2D>().enabled = true;
    }
}
