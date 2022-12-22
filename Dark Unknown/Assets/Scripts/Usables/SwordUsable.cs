using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordUsable : IUsable
{
    public SwordUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Weapons/SwordAttackIcon");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.GetCurrentWeapon().Attack();
        //throw new System.NotImplementedException();
    }
}
