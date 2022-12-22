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
        Debug.Log("Potion used.");
        Player.Instance.RegenerateHealth(Player.Instance.GetMaxHealth()/2);
        //_used = true;
    }
}
