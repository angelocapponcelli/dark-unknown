using UnityEngine;
using UnityEngine.UI;

public class AirAttackAbility : Ability
{
    [SerializeField] private GameObject _airAttack;
    [SerializeField] private GameObject _abilityReward;

    private void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
    }
    public override void Activate()
    {
        GameObject gameObjectCreated = Instantiate(_airAttack, transform.position, Quaternion.identity);
        gameObjectCreated.GetComponent<AirAttack>().SetAirAttackAbility(this);
        isActive = true;
    }
    
    public override string GetText()
    {
        return "new ability that create a toxic cloud that damages enemies";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
