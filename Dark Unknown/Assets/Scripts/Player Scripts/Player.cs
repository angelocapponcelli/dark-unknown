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
    private SpriteRenderer _playerRenderer;
    
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
        _playerRenderer = GetComponent<SpriteRenderer>();

        _weaponParent = GetComponentInChildren<WeaponParent>();


        _playerInput.LeftClick += () => _weaponParent.Attack();

        _currentHealth = _maxHealth;
        UIController.Instance.SetMaxHealth(_currentHealth);
        UIController.Instance.SetSpeedMultiplierText("+ " + (_speedMultiplier - 1) * 100 + " %");
        UIController.Instance.SetStrengthMultiplierText("+ " + (_strengthMultiplier - 1) * 100 + " %");
    }

    // Update handles the animation changes based on the mouse pointer 
    private void Update()
    {
        if (PauseMenu.GameIsPaused == false)
        {
            _weaponParent.PointerPosition = _playerInput.PointerPosition;
            _playerAnimation.AnimatePlayer(_playerInput.MovementDirection.x, _playerInput.MovementDirection.y, 
                _playerInput.PointerPosition, _playerMovement.GetRBPos());

            if (_canGetWeapon && InputManager.Instance.GetKeyDown(KeybindingActions.Interact))
            {
                //instantiate new reward
                GameObject newReward = Instantiate(_weaponParent.getWeaponReward());
                newReward.transform.position = transform.position;
                //destroy current weapon
                Destroy(_weaponParent.gameObject);
                //instantiate new current weapon
                _weaponParent = Instantiate(_weaponToGet, transform, true);
                var weaponTransform = _weaponParent.transform;
                weaponTransform.localPosition = new Vector2(0f, 0.673f);
                weaponTransform.localScale = new Vector3(1, 1, 1);
                //destroy old reward already taken
                Destroy(_rewardToGet);
                _canGetWeapon = false;
            }
        }
        
        // Use while testing to suicide
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(_currentHealth);
        }*/
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

    public void RegenerateHealth()
    {
        _currentHealth = _maxHealth;
        UIController.Instance.SetHealth(_currentHealth);
        StartCoroutine(FlashBlue());
    }

    public void IncreaseStrength(float increaseMultiplier)
    {
        _strengthMultiplier += increaseMultiplier;
        UIController.Instance.SetStrengthMultiplierText("+ " + Mathf.CeilToInt( (_strengthMultiplier-1)*100 ) + " %");
        StartCoroutine(FlashBlue());
    }

    public void ChangeWeapon(WeaponParent weapon, GameObject reward)
    {
        // change to "Press keybindingAction.Interact.ToString() to get new weapon"
        ShowPlayerUI(true, "Press " + InputManager.Instance.GetKeyForAction(KeybindingActions.Interact) + 
                           " to get new weapon");
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

    public bool checkSwordWeapon()
    {
        return _weaponParent.CompareTag("Sword");
    }
    public bool checkBowWeapon()
    {
        return _weaponParent.CompareTag("Bow");
    }
}

