using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantController : MonoBehaviour
{
    private bool _isTriggered;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private Button xButton;

    private void Start()
    {
        xButton.onClick.AddListener(OnExitClick);
    }

    private void Update()
    {
        if (_isTriggered && InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
        {
            shopUI.SetActive(true);
            Time.timeScale = 0f;
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

    private void OnExitClick()
    {
        Time.timeScale = 1f;
        DeactivateUI();
    }

    public void DeactivateUI()
    {
        shopUI.SetActive(false);
    }
    
}
