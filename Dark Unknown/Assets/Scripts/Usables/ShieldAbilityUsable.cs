using UnityEngine;

public class ShieldAbilityUsable : IUsable
{
    public ShieldAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/CircleAbility");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.ActivateAbility();
    }
}
