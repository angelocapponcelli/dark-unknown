using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _healthText;
    [SerializeField] private Slider _manaBar;
    
    [SerializeField] private Text _roomText;
    [SerializeField] private Text enemyLeftCounter;
    
    [SerializeField] private Text _speedMultiplierText;
    [SerializeField] private Text _strengthMultiplierText;
    [SerializeField] private Text _killedRewardText;
    [SerializeField] private Text _toolTipText;
    public Animator playerUIAnimator;
    
    [Header ("Boss UI")]
    [SerializeField] private GameObject _bossUIObject;
    [SerializeField] private Slider _bossHealthBar;
    [SerializeField] private Text bossName;
    public Animator bossUIAnimator;
    private static readonly int Deactivate = Animator.StringToHash("Deactivate");
    private bool _bossUIState;

    [SerializeField] public ActionButton[] actionButtons;
    private readonly Color32 _emptyActionButton = new Color32(35, 38, 63, 255);
    private SwordUsable _newSwordUsable;
    private Sprite[] _availableLetters;

    private Animator _dashAnimator;
    private static readonly int Dash = Animator.StringToHash("Dash");

    protected override void Awake()
    {
        base.Awake();
        _availableLetters = Resources.LoadAll<Sprite>("ActionBarIcons/KeybindingIcons");
    }

    private void Start()
    {
        _dashAnimator = GetComponent<Animator>();
        
        _newSwordUsable = new SwordUsable();
        //set sword as default icon
        SetUsable(actionButtons[0], _newSwordUsable);
        SetKeyActionSlot(actionButtons[1], "Spell");
        SetKeyActionSlot(actionButtons[2], "Potion");

        /*foreach (var letter in _availableLetters)
        {
            Resources.UnloadAsset(letter);
        }*/
    }

    public void SetMaxHealth(float maxHealth, float currentHealth)
    {
        _healthBar.maxValue = maxHealth;
        _healthBar.value = currentHealth;
        _healthText.text = "HP " + currentHealth + "/" + maxHealth;
    }
    public void SetHealth(float value)
    {
        _healthBar.value = value;
        _healthText.text = "HP " + value + "/" + _healthBar.maxValue;
    }
    public void SetMaxMana(float mana)
    {
        _manaBar.maxValue = mana;
        _manaBar.value = 0;
    }
    public void SetMana(float value)
    {
        _manaBar.value = value;
        
        if (Math.Abs(_manaBar.value - _manaBar.maxValue) < 0.1f) StartCoroutine(PulseMana());
    }

    private IEnumerator PulseMana()
    {
        Color a = new Color32(30, 171, 200, 255);
        Color b = new Color32(172,242,255,255);
        do
        {
            var lerpedColor = Color.Lerp(a, b, Mathf.PingPong(Time.time, 1));
            _manaBar.fillRect.GetComponent<Image>().color = lerpedColor;
            yield return new WaitForSecondsRealtime(0.005f);
        } while (Math.Abs(_manaBar.value - _manaBar.maxValue) < 0.1f);
        _manaBar.fillRect.GetComponent<Image>().color = a;
    }

    public void SetRoomText(string text)
    {
        _roomText.text = text;
    }
    
    /*public void ShowHealthPotionSpawnedText(bool showing)
    {
        if (showing) healthPotionSpawned.enabled = true;
        else healthPotionSpawned.enabled = false;
    }*/
    public void SetSpeedMultiplierText(string text)
    {
        _speedMultiplierText.text = text;
    }
    public void SetStrengthMultiplierText(string text)
    {
        _strengthMultiplierText.text = text;
    }

    public void SetKilledRewardText(string text)
    {
        _killedRewardText.text = text;
    }

    public void DeactivatePlayerUI()
    {
        playerUIAnimator.SetTrigger(Deactivate);
    }

    public void SetActiveBossHealth()
    {
        _bossUIObject.SetActive(true);
        _bossUIState = true;
    }
    public void SetInactiveBossHealth()
    {
        if (!_bossUIState) return;
        bossUIAnimator.SetTrigger(Deactivate);
        StartCoroutine(DeactivateBossHealthBar());
        _bossUIState = false;
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
    
    public void SetBossName(string str)
    {
        bossName.text = str;
    }

    public void SetEnemyCounter(int value)
    {
        enemyLeftCounter.text = "Enemies left: " + value;
    }
    
    public void DashCoolDown()
    {
        //Debug.Log("Cooldown started");
        _dashAnimator.SetTrigger(Dash);
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

    private void SetKeyActionSlot(ActionButton btn, string action)
    {
        //var keyBind = InputManager.Instance.GetKeyForAction(action).ToString();
        var keyBind = InputManager.Instance.playerInput.actions[action].bindings[0].ToDisplayString();
        var keySprite = Array.Find(_availableLetters, x => x.name == keyBind);
        btn.MyKeyIcon.sprite = keySprite;
    }

    public void ShowMessage(string message)
    {
        _toolTipText.text = message;
        Tooltip.Instance.GetComponent<Tooltip>().StartOpen();
    }
}
