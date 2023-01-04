using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseHealth : MonoBehaviour
{
    [SerializeField] private int rewardCost = 0;
    [SerializeField] private float healthIncrease = 50f;
    private int _playerRewards = 0;
    private bool _canBuy;
    private Collider2D _collider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerRewards = Player.Instance.GetKilledReward();
        _canBuy = false;
        _collider2D = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canBuy && Input.GetKeyDown(InputManager.Instance.GetKeyForAction(KeybindingActions.Interact)))
        {
           _collider2D.enabled = false;
            Player.Instance.IncreaseHealth(healthIncrease);
            Player.Instance.ModifyKilledReward(-rewardCost);
            _collider2D.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (_playerRewards >= rewardCost)
            {
                Player.Instance.ShowPlayerUI(true, "Press " + 
                                                   InputManager.Instance.GetKeyForAction(KeybindingActions.Interact) + 
                                                   " to increase your max health");
                _canBuy = true;
            }
            else
            {
                Player.Instance.ShowPlayerUI(true, "Can't buy the health increase");
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
