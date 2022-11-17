using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    protected float _damage;
    protected float _delay;
    protected bool _attackBlocked;
    protected float[] _rotationBounds;
    
    public abstract void Attack();
    
    protected IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(_delay);
        _attackBlocked = false;
    }
}
