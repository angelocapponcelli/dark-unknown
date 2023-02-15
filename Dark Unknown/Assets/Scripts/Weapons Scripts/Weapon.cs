using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Vector2 direction { get; set; }
    public float rotation { get; set; }
    protected float _damage;
    protected float _delay;
    protected bool _attackBlocked;
    protected float[] _rotationBounds;
    protected bool _canRotateFreely;
    
    public abstract void Attack();

    public bool CanRotateFreely()
    {
        return _canRotateFreely;
    }
    
    protected IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(_delay);
        _attackBlocked = false;
    }
}
