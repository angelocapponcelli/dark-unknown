using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private StatusEffectData statusEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("PlayerFeetCollider"))
            {
                print("Effect time: " + statusEffect.time);
                character.GetComponentInParent<Player>().ApplyEffect(statusEffect);
                character.GetComponentInParent<Player>().TakeDamage(statusEffect.damage);
                Player.Instance.RemoveEffect();
            }
        }
    }
}
