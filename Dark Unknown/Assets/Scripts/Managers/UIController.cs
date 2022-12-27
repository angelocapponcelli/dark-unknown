using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _roomText;
    [SerializeField] private Text enemyLeftCounter;
    [SerializeField] private Text _speedMultiplierText;
    [SerializeField] private Text _strengthMultiplierText;
    public Animator playerUIAnimator;
    
    [Header ("Boss UI")]
    [SerializeField] private GameObject _bossUIObject;
    [SerializeField] private Slider _bossHealthBar;
    public Animator bossUIAnimator;
    private static readonly int Deactivate = Animator.StringToHash("Deactivate");

    [SerializeField] public ActionButton[] actionButtons;
    private readonly Color32 _emptyActionButton = new Color32(35, 38, 63, 255);
    private SwordUsable _newSwordUsable;
    private Sprite[] _availableLetters;

    protected override void Awake()
    {
        base.Awake();
        _availableLetters = Resources.LoadAll<Sprite>("ActionBarIcons/KeybindIcons");
    }

    private void Start()
    {
        /*foreach (var sprite in _availableLetters)
        {
            Debug.Log(sprite.name);
        }*/
        
        _newSwordUsable = new SwordUsable();
        //set sword as default icon
        SetUsable(actionButtons[0], _newSwordUsable);
        SetUsable(actionButtons[3], _newSwordUsable);
        SetKeyActionSlot(actionButtons[1], KeybindingActions.Spell);
        SetKeyActionSlot(actionButtons[2], KeybindingActions.Potion);

        foreach (var letter in _availableLetters)
        {
            Resources.UnloadAsset(letter);
        }
    }

    public void SetMaxHealth(float health)
    {
        _healthBar.maxValue = health;
        _healthBar.value = health;

    }
    public void SetHealth(float value)
    {
        _healthBar.value = value;
    }
    public void SetRoomText(string text)
    {
        _roomText.text = text;
    }
    public void SetSpeedMultiplierText(string text)
    {
        _speedMultiplierText.text = text;
    }
    public void SetStrengthMultiplierText(string text)
    {
        _strengthMultiplierText.text = text;
    }

    public void DeactivatePlayerUI()
    {
        playerUIAnimator.SetTrigger(Deactivate);
    }

    public void SetActiveBossHealth()
    {
        _bossUIObject.SetActive(true);
    }
    public void SetInactiveBossHealth()
    {
        bossUIAnimator.SetTrigger(Deactivate);
        StartCoroutine(DeactivateBossHealthBar());
    }

    private IEnumerator DeactivateBossHealthBar()
    {
        yield return new WaitForSeconds(1);
        _bossUIObject.SetActive(false);   
    }
    public void SetMaxBossHealth(float health)
    {
        _bossHealthBar.maxValue = health;
        _bossHealthBar.value = health;
    }
    public void SetBossHealth(float value)
    {
        _bossHealthBar.value = value;
    }

    public void SetEnemyCounter(int value)
    {
        enemyLeftCounter.text = "Enemies left: " + value;
    }
    
    public void DashCoolDown()
    {
        GetComponent<Animator>().Play("DashLoading", 0);
    }

    public void EnableDash()
    {
        Player.Instance.GetComponent<PlayerMovement>().EnableDash();
    }

    public void ClickActionButton(string buttonName)
    {
        Array.Find(actionButtons, x=>x.gameObject.name == buttonName).MyButton.onClick.Invoke();
    }

    public void SetUsable(ActionButton btn, IUsable usable = null)
    {
        if (usable != null)
        {
            btn.MyIcon.sprite = usable.MyIcon;
            btn.MyIcon.color = Color.white;
            btn.MyUsable = usable;
        }
        else
        {
            btn.MyIcon.sprite = null;
            btn.MyIcon.color = _emptyActionButton;
            btn.MyUsable = null;
        }
    }

    public void SetKeyActionSlot(ActionButton btn, KeybindingActions action)
    {
        var keybind = InputManager.Instance.GetKeyForAction(action).ToString();
        var keySprite = Array.Find(_availableLetters, x => x.name == keybind);
        btn.MyKeyIcon.sprite = keySprite;
    }
}
