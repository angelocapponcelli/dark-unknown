using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedReward : Reward
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                character.GetComponentInParent<Player>().IncreaseSpeed(Random.Range(5,10)/100f);
                AudioManager.Instance.PlayPLayerRewardSound();
                Destroy(gameObject);
            }
        }
    }
}
