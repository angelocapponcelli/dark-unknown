using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] private GameObject _axeLaunch;
    
    public override void Attack()
    {
        if (_attackBlocked)
            return;
        GameObject axeLaunch = Instantiate(_axeLaunch, transform.position, Quaternion.Euler(0f,0f,-90f) );
        axeLaunch.GetComponent<AxeAttack>().setTarget(GetComponentInParent<WeaponParent>().PointerPosition);
        axeLaunch.GetComponent<Rigidbody2D>().velocity = (direction*10).normalized * 8;
        axeLaunch.GetComponent<Rigidbody2D>().angularVelocity = 1000.0f;
        enableSprite(false);
        //Destroy(arrow, travellingTime);

        //_effectAnimator.SetTrigger("Attack");
        //_attackBlocked = true;
        AudioManager.Instance.PlayPLayerAttackBowSound();

        //StartCoroutine(DelayAttack());
    }

    public void enableSprite(bool enable)
    {
        GetComponent<SpriteRenderer>().enabled = enable;
        _attackBlocked = !enable;
    }
}
