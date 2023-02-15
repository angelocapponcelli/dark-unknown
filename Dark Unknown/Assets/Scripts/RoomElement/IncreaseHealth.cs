using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreaseHealth : MonoBehaviour
{
    [SerializeField] private int rewardCost = 200;
    [SerializeField] private float healthIncrease = 50f;
    private int _playerRewards = 0;
    private bool _canBuy;
    private Collider2D _collider2D;
    [SerializeField] private Text costText;
    
    private UnityEngine.InputSystem.PlayerInput _playerControls;

    // Start is called before the first frame update
    private void Start()
    {
        _canBuy = false;
        _collider2D = GetComponent<Collider2D>();
        costText.text = "x" + rewardCost;
        _playerControls = InputManager.Instance.playerInput;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_canBuy && _playerControls.actions["Interact"].WasPressedThisFrame())
        {
           _collider2D.enabled = false;
            Player.Instance.IncreaseHealth(healthIncrease);
            Player.Instance.ModifyKilledReward(-rewardCost);
            _collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        
        var text = "";
        if (InputManager.Instance.playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            text = InputManager.Instance.playerInput.actions["Interact"].bindings[0].ToDisplayString();
        }
        else if (InputManager.Instance.playerInput.currentControlScheme == "Gamepad")
        {
            text = "A";
        }
        _playerRewards = Player.Instance.GetKilledReward();
        if (col.CompareTag("Player"))
        {
            if (_playerRewards >= rewardCost)
            {
                Player.Instance.ShowPlayerUI(true, "Press " + text + " to increase your max health by 50.");
                _canBuy = true;
            }
            else
            {
                Player.Instance.ShowPlayerUI(true, "Can't buy the health increase.");
                _canBuy = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            _canBuy = false;
        }
    }
}
