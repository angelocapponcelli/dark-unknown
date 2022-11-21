using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    private PlayerInput _playerInput;
    private PlayerAnimation _playerAnimation;
    
    private Vector2 _direction;    
    private Vector2 _pointerPos;
    private WeaponParent _weaponParent;
    [SerializeField] private float _health = 100;

    [SerializeField] private HealthBar _healthBar;

    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerInput = GetComponent<PlayerInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _weaponParent = GetComponentInChildren<WeaponParent>();
        
        _playerInput.LeftClick += () => _weaponParent.Attack();
        _healthBar.SetMaxHealth(_health);
    }

    // Update handles the animation changes based on the mouse pointer 
    void Update()
    {
        _weaponParent.PointerPosition = _playerInput.PointerPosition;
        _playerAnimation.AnimatePlayer(_playerInput.MovementDirection.x, _playerInput.MovementDirection.y, _playerInput.PointerPosition, _playerMovement.GetRBPos());

    }
    
    // FixedUpdate handles the movement 
    private void FixedUpdate()
    {
        _playerMovement.MovePlayer(_playerInput.MovementDirection, _playerInput.GetShiftDown());
    }

    public void TakeDamage(float damage)
    {
        
        _health -= damage;
        _healthBar.SetHealth(_health);
        StartCoroutine(FlashRed());
        PlayerEvents.playerHit.Invoke();
    }

    private IEnumerator FlashRed()
    {
        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();
        playerRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        playerRenderer.color = Color.white;
    }
}

