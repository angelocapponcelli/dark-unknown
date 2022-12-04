using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossUIController : MonoBehaviour
{
    public void SetMaxHealth(float maxHealth)
    {
        UIController.Instance.SetActiveBossHealth(true);
        UIController.Instance.SetMaxBossHealth(maxHealth);
    }
    public void SetHealth(float health)
    {
        UIController.Instance.SetBossHealth(health);
    }
    
    public void DeactivateHealthBar()
    {
        UIController.Instance.SetActiveBossHealth(false);
    }
}
