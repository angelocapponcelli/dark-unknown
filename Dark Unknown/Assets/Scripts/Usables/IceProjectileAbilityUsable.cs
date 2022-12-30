using UnityEngine;

public class IceProjectileAbilityUsable : IUsable
{
    public IceProjectileAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/CircleAbility");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        //Player.Instance.GetCurrentWeapon().Attack();
    }
}
