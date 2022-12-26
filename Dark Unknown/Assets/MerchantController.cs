using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private bool _isTriggered;
    [SerializeField] private GameObject shopUI;

    private void Update()
    {
        if (_isTriggered && InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
        {
            shopUI.SetActive(true);
        }
    }


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _isTriggered = true;
            //set room colliders to false
            //useful mainly because we can't delete the serialized room from the GameManager
            //TODO: could do a room just for it with a special class, ...
            Player.Instance.ShowPlayerUI(true, "Press " + 
                                               InputManager.Instance.GetKeyForAction(KeybindingActions.Interact) + 
                                               " to interact");
        }
    }
    public void OnTriggerExit2D (Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _isTriggered = false;
            Player.Instance.ShowPlayerUI(false, "");
        }
    }

    public void DeactivateUI()
    {
        shopUI.SetActive(false);
    }
    
}
