using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAttack : MonoBehaviour
{
    [SerializeField] private float _damage = 30f;

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
        //layer.9 == "Arrow" Layer is where circleAbility projectiles are
        if (collision.CompareTag("Trap") || collision.CompareTag("Gem") || collision.gameObject.layer == 9 || collision.CompareTag("HiddenWormCollider")) return;
        Destroy(gameObject);
    }
    
    private IEnumerator DeactivateCollider(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        GetComponentInChildren<BoxCollider2D>().enabled = true;
    }
}
