//using System.Numerics;

using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using TMPro;

public class Player : Singleton<Player>, IEffectable
{
    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    private SpriteRenderer _playerRenderer;
    
    private Vector2 _direction;    
    private Vector2 _pointerPos;
    private WeaponParent _weaponParent;
    [SerializeField] private float _maxHealth = 100;
    private float _currentHealth;
    [SerializeField] private GameObject _playerUI;
    
    private StatusEffectData _statusEffect;
    private float _currentEffectTime = 0;
    private float _nextTickTime = 0;
    private GameObject _statusEffectParticles;

    private float _healthMultiplier = 1;
    private float _speedMultiplier = 1;
    private float _strengthMultiplier = 1;

    private bool _canGetPotion;
    private bool _canGetWeapon;
    private WeaponParent _weaponToGet;
    private IUsable _weaponUsable;
    private GameObject _rewardToGet;

    private bool _hasPotion;
    private Potion _newPotion;
    private UsedPotion _usedPotion;

    // Start is called before the first frame update
    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _playerRenderer = GetComponent<SpriteRenderer>();

        _weaponParent = GetComponentInChildren<WeaponParent>();
        
        _playerInput.LeftClick += () => _weaponParent.Attack();
        //_playerInput.LeftClick += () => UIController.Instance.ClickActionButton("WeaponButton");
        
        _currentHealth = _maxHealth;
        UIController.Instance.SetMaxHealth(_currentHealth);
        UIController.Instance.SetSpeedMultiplierText("+ " + (_speedMultiplier - 1) * 100 + " %");
        UIController.Instance.SetStrengthMultiplierText("+ " + (_strengthMultiplier - 1) * 100 + " %");
        
        _newPotion = new Potion();
        _usedPotion = new UsedPotion();
    }

    // Update handles the animation changes based on the mouse pointer 
    private void Update()
    {
        if (PauseMenu.GameIsPaused) return;
        _weaponParent.PointerPosition = _playerInput.PointerPosition;
        _playerAnimation.AnimatePlayer(_playerInput.MovementDirection.x, _playerInput.MovementDirection.y, 
            _playerInput.PointerPosition, _playerMovement.GetRBPos());

        if (_canGetWeapon && InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
        {
            GameObject newReward = Instantiate(_weaponParent.getWeaponReward());
            newReward.transform.position = transform.position;
            //destroy current weapon
            Destroy(_weaponParent.gameObject);
            //instantiate new current weapon
            _weaponParent = Instantiate(_weaponToGet, transform, true);
            var weaponTransform = _weaponParent.transform;
            weaponTransform.localPosition = new Vector2(0f, 0.673f);
            weaponTransform.localScale = new Vector3(1, 1, 1);
            UIController.Instance.SetUsable(UIController.Instance.actionButtons[0], _weaponUsable);
            UIController.Instance.SetUsable(UIController.Instance.actionButtons[3], _weaponUsable);
            //destroy old reward already taken
            Destroy(_rewardToGet);
            _canGetWeapon = false;
        } else if (_canGetPotion && !_hasPotion && InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
        {
            //destroy potion taken game-object
            Destroy(_rewardToGet);
            UIController.Instance.SetUsable(UIController.Instance.actionButtons[2], _newPotion);
            _canGetPotion = false;
            _hasPotion = true;
        }
        
        if(_statusEffect != null) HandleEffect();

        // Use potion only when the player has one and has lower than max health
        if (InputManager.Instance.GetKeyDown(KeybindingActions.Potion))
        {
            UIController.Instance.ClickActionButton("PotionButton");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(100f);
        }
    }
    
    // FixedUpdate handles the movement 
    private void FixedUpdate()
    {
        _playerMovement.MovePlayer(_playerInput.MovementDirection, PlayerInput.GetDashKeyDown());
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UIController.Instance.SetHealth(_currentHealth);
        StartCoroutine(FlashRed());
        PlayerEvents.PlayerHit.Invoke();
        AudioManager.Instance.PlayPLayerHurtSound();
        
        //game over
        if (!(_currentHealth <= 0)) return;
        StartCoroutine(Death());
        _playerMovement.IncreaseSpeed(0);
        _playerMovement.enabled = false;
        _playerInput.enabled = false;
        PlayerEvents.PlayerHit.Invoke();
        AudioManager.Instance.PlayPLayerHurtSound();
        GameManager.Instance.LoadDeathScreen();
    }

    private IEnumerator FlashRed()
    {
        _playerRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        _playerRenderer.color = Color.white;
    }
    
    private IEnumerator Death()
    {
        for (float i = 0; i < 1f; i += 0.1f)
        {
            _playerRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            _playerRenderer.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FlashBlue()
    {
        _playerRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.2f);
        _playerRenderer.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        _playerRenderer.color = Color.cyan;
        yield return new WaitForSeconds(0.2f);
        _playerRenderer.color = Color.white;
    }

    public void SetPosition(Vector3 newPos)
    {
        transform.position = newPos;
    }

    public void IncreaseSpeed (float increaseMultiplier)
    {
        _speedMultiplier += increaseMultiplier;
        _playerMovement.IncreaseSpeed(_speedMultiplier);
        UIController.Instance.SetSpeedMultiplierText("+ " + Mathf.CeilToInt( (_speedMultiplier-1)*100 ) + " %");
        StartCoroutine(FlashBlue());
    }

    public void IncreaseHealth(float increaseMultiplier)
    {
        //TODO in future, for now not used
        _healthMultiplier += increaseMultiplier;
        _currentHealth = _currentHealth * _healthMultiplier;
        UIController.Instance.SetHealth(_currentHealth);
        StartCoroutine(FlashBlue());
    }

    public void RegenerateHealth(float value)
    {
        if (_currentHealth + value >= _maxHealth) _currentHealth = _maxHealth;
        else _currentHealth += value;
        UIController.Instance.SetHealth(_currentHealth);
        StartCoroutine(FlashBlue());
    }

    public void IncreaseStrength(float increaseMultiplier)
    {
        _strengthMultiplier += increaseMultiplier;
        UIController.Instance.SetStrengthMultiplierText("+ " + Mathf.CeilToInt( (_strengthMultiplier-1)*100 ) + " %");
        StartCoroutine(FlashBlue());
    }

    public void ChangeWeapon(WeaponParent weapon, GameObject reward, IUsable usable)
    {
        ShowPlayerUI(true, "Press " + InputManager.Instance.GetKeyForAction(KeybindingActions.Interact) + 
                           " to get new weapon.");
        _canGetWeapon = true;
        _weaponToGet = weapon;
        _weaponUsable = usable;
        _rewardToGet = reward;
    }
    
    public void PickUpPotion(GameObject reward)
    {
        // change to "Press keybindingAction.Interact.ToString() to get new weapon"
        ShowPlayerUI(true, "Press " + InputManager.Instance.GetKeyForAction(KeybindingActions.Interact) + 
                           " to pick up healing potion.");
        _canGetPotion = true;
        _rewardToGet = reward;
    }

    public void DisableCanGetWeapon()
    {
        _canGetWeapon = false;
    }
    
    public void DisableCanGetPotion()
    {
        _canGetPotion = false;
    }

    public void ShowPlayerUI(bool show, string text)
    {
        _playerUI.GetComponentInChildren<Text>().text = text;
        _playerUI.SetActive(show);
    }

    public float GetStrengthMultiplier()
    {
        return _strengthMultiplier;
    }

    public bool checkSwordWeapon()
    {
        return _weaponParent.CompareTag("Sword");
    }
    public bool checkBowWeapon()
    {
        return _weaponParent.CompareTag("Bow");
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    
    public float GetCurrentHealth()
    {
        return _currentHealth;
    }
    
    public bool HasPotion()
    {
        return _hasPotion;
    }
    
    public void SetHasPotion(bool value)
    {
        _hasPotion = value;
    }

    public WeaponParent GetCurrentWeapon()
    {
        return _weaponParent;
    }

    public void ApplyEffect(StatusEffectData _data)
    {
        _statusEffect = _data;
        _statusEffectParticles = Instantiate(_data.particles, transform);
    }

    public void RemoveEffect()
    {
        Destroy(_statusEffectParticles);
        _statusEffect = null;
        _currentEffectTime = 0;
        _nextTickTime = 0;
    }
    
    private void HandleEffect()
    {
        _currentEffectTime += Time.deltaTime;
        
        if(_currentEffectTime >= _statusEffect.time) RemoveEffect();
        if (_statusEffect == null) return;
        if (_currentEffectTime > _nextTickTime)
        {
            _nextTickTime += _statusEffect.tickSpeed;
            _currentHealth -= _statusEffect.damage;
            UIController.Instance.SetHealth(_currentHealth);
        }
    }
}

