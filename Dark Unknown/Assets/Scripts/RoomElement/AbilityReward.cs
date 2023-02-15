using Unity.Collections;
using UnityEngine;

public class AbilityReward : Reward
{
    [SerializeField] private Ability _ability;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                IUsable usable;
                if (_ability.GetComponent<CircleAbility>()) usable = new CircleAbilityUsable();
                else if (_ability.GetComponent<AirAttackAbility>()) usable = new AirAttackAbilityUsable();
                else if (_ability.GetComponent<IceProjectileAbility>()) usable = new IceProjectileAbilityUsable();
                else if (_ability.GetComponent<BatAbility>()) usable = new BatAbilityUsable();
                else usable = new ShieldAbilityUsable();
                character.GetComponentInParent<Player>().PickUpAbility(_ability, gameObject, usable);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            Player.Instance.DisableCanGetAbility();
        }
    }
}
