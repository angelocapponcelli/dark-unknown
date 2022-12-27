using System;
using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] private GameObject _axeLaunch;
    [SerializeField] private float _angleStep = 30;
    [SerializeField] private int _axesNumber = 1;
    private int _axesReturned = 1;

    private void Start()
    {
        _axesReturned = _axesNumber;
    }

    public override void Attack()
    {
        if (_attackBlocked)
            return;

        for (int i = 0; i < _axesNumber; i++)
        {
            GameObject axeLaunch = Instantiate(_axeLaunch, transform.position, Quaternion.identity);
            axeLaunch.GetComponent<AxeAttack>().setTarget(GetComponentInParent<WeaponParent>().PointerPosition);
            axeLaunch.GetComponent<Rigidbody2D>().velocity = Quaternion.Euler(0f, 0f, -(_angleStep * (_axesNumber-1))/2 + i * _angleStep) *
                                                             (direction * 10).normalized * 8;
            axeLaunch.GetComponent<Rigidbody2D>().angularVelocity = 1000.0f;
            _axesReturned--;
        }
        enableSprite(false);
        
        AudioManager.Instance.PlayPLayerAttackBowSound();
    }

    public void enableSprite(bool enable)
    {
        if(enable) _axesReturned = Math.Min(_axesReturned+1, _axesNumber);
        GetComponent<SpriteRenderer>().enabled = (_axesReturned == _axesNumber);
        _attackBlocked = (_axesReturned != _axesNumber);
    }

    private void SetAxesNumber(int value)
    {
        _axesNumber = value;
        _axesReturned = _axesNumber;
    }
}
