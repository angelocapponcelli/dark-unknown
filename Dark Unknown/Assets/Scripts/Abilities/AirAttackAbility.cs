using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackAbility : Ability
{
    [SerializeField] private GameObject _airAttack;

    public override void Activate()
    {
        GameObject gameObjectCreated = Instantiate(_airAttack, transform.position, Quaternion.identity);
        gameObjectCreated.GetComponent<AirAttack>().SetAirAttackAbility(this);
        isActive = true;
    }
}
