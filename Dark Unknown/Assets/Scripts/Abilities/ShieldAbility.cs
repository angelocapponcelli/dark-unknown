using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldAbility : Ability
{
    [SerializeField] private float _delay = 2f;
    private SpriteRenderer _spriteRenderer;
    private CircleCollider2D _collider;
    [SerializeField] private GameObject _abilityReward;

    // Start is called before the first frame update
    private void Start()
    {
        transform.localPosition = new Vector2(0,0);
        transform.localScale = new Vector3(2.5f, 2.2f, 1);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        Activation(false);
    }

    public override void Activate()
    {
        if (!_spriteRenderer.enabled)
        {
            Activation(true);
            StartCoroutine(DelayedDeactivation());
        }
    }

    
    private IEnumerator DelayedDeactivation()
    {
        yield return new WaitForSeconds(_delay);
        Activation(false);
    }

    private void Activation(bool value)
    {
        _spriteRenderer.enabled = value;
        _collider.enabled = value;
        isActive = value;
    }
    
    public override string GetText()
    {
        return "a new ability that creates a shield that protects you from enemy attacks.";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
