using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Reward : MonoBehaviour
{
    // Parents class of all reward that spwan in the room
    protected abstract void OnTriggerEnter2D(Collider2D collision);
}
