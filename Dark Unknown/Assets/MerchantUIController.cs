using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantUIController : MonoBehaviour
{
    [SerializeField] private Text weaponName;
    [SerializeField] private Text weaponDescription;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponName.text = Player.Instance.GetWeaponName();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
