using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeController : MonoBehaviour
{
    [SerializeField] private float _damage = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                character.GetComponentInParent<Player>().TakeDamage(_damage);
            }
        }
    }

    /*private void OnColliderEnter(Collision2D collision)
    {
        Collider2D[] hitCharacters = collision.collider.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                character.GetComponent<Player>().TakeDamage(_damage);
            }
        }
    }*/
}
