using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class ShieldAbilityUsable : IUsable
{
    public ShieldAbilityUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Spells/ShieldAbility");
    }

    public Sprite MyIcon { get; }
    
    public void Use()
    {
        Player.Instance.ActivateAbility();
    }
}
