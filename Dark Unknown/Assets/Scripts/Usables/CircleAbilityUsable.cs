using UnityEngine;

public class CircleAbilityUsable : IUsable
{
    public CircleAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/CircleAbility");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        //Player.Instance.GetCurrentWeapon().Attack();
    }
}
