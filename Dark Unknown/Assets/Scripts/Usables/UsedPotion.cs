using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedPotion : IUsable
{
    public UsedPotion()
    {
        MyIcon = Resources.Load<Sprite>("ActionBarIcons/UsedPotionSprite");
    }
    
    public Sprite MyIcon { get; private set; }

    public void Use(){}
}
