using System;
using System.Collections;
using UnityEngine;
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
    public Animator playerUIAnimator;
    
    [Header ("Boss UI")]
    [SerializeField] private GameObject _bossUIObject;
    [SerializeField] private Slider _bossHealthBar;
    [SerializeField] private Text bossName;
    public Animator bossUIAnimator;
    private static readonly int Deactivate = Animator.StringToHash("Deactivate");

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
        SetKeyActionSlot(actionButtons[1], KeybindingActions.Spell);
        SetKeyActionSlot(actionButtons[2], KeybindingActions.Potion);

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
    public IEnumerator SetMana(float value)
    {
        do
        {
            _manaBar.value = Mathf.MoveTowards(_manaBar.value, value, Time.deltaTime);
            yield return new WaitForSecondsRealtime(0.01f);
        } while (_manaBar.value < value);

        if (_manaBar.value == _manaBar.maxValue) StartCoroutine(pulseMana());
    }

    IEnumerator pulseMana()
    {
        Color a = new Color32(30, 171, 200, 255);
        Color b = new Color32(172,242,255,255);
        do
        {
            Color lerpedColor = Color.Lerp(a, b, Mathf.PingPong(Time.time, 1));
            _manaBar.fillRect.GetComponent<Image>().color = lerpedColor;
            yield return new WaitForSecondsRealtime(0.005f);
        } while (_manaBar.value == _manaBar.maxValue);
        _manaBar.fillRect.GetComponent<Image>().color = a;
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
        Debug.Log("Cooldown started");
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

    public void SetKeyActionSlot(ActionButton btn, KeybindingActions action)
    {
        var keybind = InputManager.Instance.GetKeyForAction(action).ToString();
        var keySprite = Array.Find(_availableLetters, x => x.name == keybind);
        btn.MyKeyIcon.sprite = keySprite;
    }
}
