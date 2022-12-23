using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthReward : Reward
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                /*character.GetComponentInParent<Player>().RegenerateHealth(Player.Instance.GetMaxHealth());
                AudioManager.Instance.PlayPLayerRewardSound();
                Destroy(gameObject);*/
                character.GetComponentInParent<Player>().PickUpPotion(gameObject);
            }
        }
    }
    
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            Player.Instance.DisableCanGetPotion();
        }
    }
}
