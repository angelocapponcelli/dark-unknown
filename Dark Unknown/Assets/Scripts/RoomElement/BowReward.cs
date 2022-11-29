using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowReward : Reward
{
    [SerializeField] private WeaponParent _bowPrefab;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                //TODO
                character.GetComponentInParent<Player>().ChangeWeapon(_bowPrefab);
                Destroy(gameObject);
            }
        }
    }
}
