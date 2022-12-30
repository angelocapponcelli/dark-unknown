using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIController : MonoBehaviour
{
    public void SetMaxHealth(float maxHealth)
    {
        UIController.Instance.SetActiveBossHealth();
        UIController.Instance.SetMaxBossHealth(maxHealth);
    }
    public void SetHealth(float health)
    {
        UIController.Instance.SetBossHealth(health);
    }
    
    public void DeactivateHealthBar()
    {
        UIController.Instance.SetInactiveBossHealth();
    }
}
