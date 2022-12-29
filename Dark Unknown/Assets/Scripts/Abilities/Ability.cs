using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected bool isActive;

    public void SetIsActive(bool value)
    {
        isActive = value;
    }
    public bool IsActive()
    {
        return isActive;
    }
    public abstract void Activate();
}
