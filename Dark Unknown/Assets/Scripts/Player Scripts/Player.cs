//using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : Singleton<Player>
{
    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    
    private Vector2 _direction;    
    private Vector2 _pointerPos;
    private WeaponParent _weaponParent;
    [SerializeField] private float _maxHealth = 100;
    private float _currentHealth;
    [SerializeField] private GameObject _playerUI;

    private float _healthMultiplier = 1;
    private float _speedMultiplier = 1;
    private float _strengthMultiplier = 1;

    private bool _canGetWeapon = false;
    private WeaponParent _weaponToGet;
    private GameObject _rewardToGet;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();

        _weaponParent = GetComponentInChildren<WeaponParent>();


        _playerInput.LeftClick += () => _weaponParent.Attack();

        _currentHealth = _maxHealth;
        UIController.Instance.SetMaxHealth(_currentHealth);
        UIController.Instance.SetSpeedMultiplierText("+ " + (_speedMultiplier - 1) * 100 + " %");
        UIController.Instance.SetStrengthMultiplierText("+ " + (_strengthMultiplier - 1) * 100 + " %");
    }

    // Update handles the animation changes based on the mouse pointer 
    void Update()
    {
        _weaponParent.PointerPosition = _playerInput.PointerPosition;
        _playerAnimation.AnimatePlayer(_playerInput.MovementDirection.x, _playerInput.MovementDirection.y, _playerInput.PointerPosition, _playerMovement.GetRBPos());

        if(_canGetWeapon && Input.GetKeyDown(KeyCode.E))
        {
            //instantiate new reward
            GameObject newReward = Instantiate(_weaponParent.getWeaponReward());
            newReward.transform.position = transform.position;
            //destroy current weapon
            Destroy(_weaponParent.gameObject);
            //instantiate new current weapon
            _weaponParent = Instantiate(_weaponToGet);
            _weaponParent.transform.parent = transform;
            _weaponParent.transform.localPosition = new Vector2(0.1f, 0.7f);
            //destroy old reward akready taken
            Destroy(_rewardToGet);            
            _canGetWeapon = false;
        }
    }
    
    // FixedUpdate handles the movement 
    private void FixedUpdate()
    {
        _playerMovement.MovePlayer(_playerInput.MovementDirection, _playerInput.GetShiftDown());
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        UIController.Instance.SetHealth(_currentHealth);
        StartCoroutine(FlashRed());
        PlayerEvents.playerHit.Invoke();
        AudioManager.Instance.PlayPLayerHurtSound();
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerRenderer.color = Color.white;
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
    }

    public void IncreaseHealth(float increaseMultiplier)
    {
        //TODO in future, for now not used
        _healthMultiplier += increaseMultiplier;
        _currentHealth = _currentHealth * _healthMultiplier;
        UIController.Instance.SetHealth(_currentHealth);
    }

    public void RegenerateHealth()
    {
        _currentHealth = _maxHealth;
        UIController.Instance.SetHealth(_currentHealth);
    }

    public void IncreaseStrenght(float increaseMultiplier)
    {
        _strengthMultiplier += increaseMultiplier;
        UIController.Instance.SetStrengthMultiplierText("+ " + Mathf.CeilToInt( (_strengthMultiplier-1)*100 ) + " %");
    }

    public void ChangeWeapon(WeaponParent weapon, GameObject reward)
    {
        ShowPlayerUI(true, "Press E to get new weapon");
        _canGetWeapon = true;
        _weaponToGet = weapon;
        _rewardToGet = reward;
    }

    public void disableCanGetWeapon()
    {
        _canGetWeapon = false;
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
}

