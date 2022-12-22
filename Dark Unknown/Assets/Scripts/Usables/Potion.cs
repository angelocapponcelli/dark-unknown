using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : IUsable
{
    public Potion()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/PotionSprite");;
    }
    
    public Sprite MyIcon { get; private set; }
    //private bool _used = false;

    public void Use()
    {
        //if (_used) return;
        if (!Player.Instance.HasPotion() ||
            !(Math.Abs(Player.Instance.GetCurrentHealth() - Player.Instance.GetMaxHealth()) > 0)) return;
        Player.Instance.RegenerateHealth(Player.Instance.GetMaxHealth()/2);
        Player.Instance.SetHasPotion(false);
        UIController.Instance.SetUsable(UIController.Instance.actionButtons[2], new UsedPotion());

        //_used = true;
    }
}
