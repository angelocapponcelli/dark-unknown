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
    void Start()
    {
        transform.localPosition = new Vector2(0,0);
        transform.localScale = new Vector3(2.5f, 2.2f, 1);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider = GetComponent<CircleCollider2D>();
        activation(false);
    }

    public override void Activate()
    {
        if (!_spriteRenderer.enabled)
        {
            activation(true);
            StartCoroutine(delayedDeactivation());
        }
    }

    
    private IEnumerator delayedDeactivation()
    {
        yield return new WaitForSeconds(_delay);
        activation(false);
    }

    private void activation(bool value)
    {
        _spriteRenderer.enabled = value;
        _collider.enabled = value;
        isActive = value;
    }
    
    public override string GetText()
    {
        return "new ability that create a shield that protect you from enemies";
    }
    
    public override GameObject GetAbilityReward()
    {
        return _abilityReward;
    }
}
