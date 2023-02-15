using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string name;
    public float damage;
    public float time;
    public float tickSpeed;

    public GameObject particles;
}
