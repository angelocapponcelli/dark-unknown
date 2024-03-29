using System.Collections;
using UnityEngine;

public class BatAbility : Ability
{
    [SerializeField] private GameObject _batPrefab;
    [SerializeField] private GameObject _abilityReward;
    [SerializeField] private float timeAlive = 200f;
    private GameObject _bat;
    private Coroutine _coroutine;
    
    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector2(0,0.5f);
    }

    public override void Activate()
    {
         _bat = Instantiate(_batPrefab, transform.position, Quaternion.identity);
         _coroutine = StartCoroutine(TimeToReturn(timeAlive));
    }

    public void Deactivate()
    {
        if(isActive)
            StopCoroutine(_coroutine);
        isActive = false;
    }
    
    private IEnumerator TimeToReturn(float seconds)
    {
        isActive = true;
        yield return new WaitForSeconds(seconds);
        Destroy(_bat.gameObject);
        isActive = false;
    }

    public override string GetText()
    {
        return "a new ability that spawns a bat that will follow you and hit enemies.";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
