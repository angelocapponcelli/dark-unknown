using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDamageToPlayer : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        if (hitCharacters[0].gameObject.CompareTag("Player") || hitCharacters[0].gameObject.CompareTag("PlayerFeet"))
        {
            hitCharacters[0].GetComponentInParent<Player>().TakeDamage(_damage);
        }
    }
}
