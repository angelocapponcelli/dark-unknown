using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectable
{
    public void ApplyEffect(StatusEffectData _data);
    public void RemoveEffect();
}
