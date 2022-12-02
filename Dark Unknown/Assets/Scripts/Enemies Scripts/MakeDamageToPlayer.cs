using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamageToPlayer : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("PlayerFeet"))
            {
                character.GetComponentInParent<Player>().TakeDamage(_damage);
            }
        }
    }
}
