using UnityEngine;

public class IceProjectileAbilityUsable : IUsable
{
    public IceProjectileAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/IceProjectileAbility");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.ActivateAbility();
    }
}
