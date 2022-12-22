using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsable
{
    Sprite MyIcon
    {
        get;
    }

    void Use();
}
