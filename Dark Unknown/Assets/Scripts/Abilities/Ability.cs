using UnityEngine.UI;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    protected bool isActive;
    [SerializeField] private float cost;

    public void SetIsActive(bool value)
    {
        isActive = value;
    }
    public bool IsActive()
    {
        return isActive;
    }

    public float GetCost()
    {
        return cost;
    }
    public abstract void Activate();
    public abstract string GetText();
    public abstract GameObject GetAbilityReward();
}
