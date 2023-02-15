using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowUsable : IUsable
{
    public BowUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Weapons/BowAttackIcon");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.GetCurrentWeapon().Attack();
    }
}
