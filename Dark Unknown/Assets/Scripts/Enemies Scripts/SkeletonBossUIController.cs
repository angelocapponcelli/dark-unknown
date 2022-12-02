using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossUIController : MonoBehaviour
{
    public void SetMaxHealth(float maxHealth)
    {
        UIController.Instance.setActiveBossHealth(true);
        UIController.Instance.SetMaxBossHealth(maxHealth);
    }
    public void SetHealth(float health)
    {
        UIController.Instance.SetBossHealth(health);
    }
}
