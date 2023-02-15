using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeUsable : IUsable
{
    public AxeUsable()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/Weapons/AxeThrowIcon");
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.GetCurrentWeapon().Attack();
    }
}
