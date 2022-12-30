using UnityEngine;

public class AirAttackAbilityUsable : IUsable
{
    public AirAttackAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/CircleAbility");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.ActivateAbility();
    }
}
