using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (!character.gameObject.CompareTag("EnemyFeetCollider") &&
                !character.gameObject.CompareTag("PlayerFeetCollider")) continue;
            //Debug.Log("damage taken");
            //character.GetComponentInParent<Player>().TakeDamage(_damage);
            if(character.GetComponentInParent<Player>()!=null)
                character.GetComponentInParent<Player>().TakeDamage(_damage);
            if(character.GetComponentInParent<EnemyController>()!=null)
                character.GetComponentInParent<EnemyController>().TakeDamage(_damage, false);
        }
    }
}

