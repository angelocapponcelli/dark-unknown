using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour, IUsable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite MyIcon { get; }
    public void Use()
    {
        Player.Instance.RegenerateHealth(Player.Instance.GetMaxHealth()/2);
    }
}
