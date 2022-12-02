using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Text _roomText;
    [SerializeField] private Text _speedMultiplierText;
    [SerializeField] private Text _strengthMultiplierText;
    [Header ("Boss UI")]
    [SerializeField] private GameObject _bossUIObject;
    [SerializeField] private Slider _bossHealthBar;

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

    public void setActiveBossHealth(bool value)
    {
        _bossUIObject.SetActive(value);
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

    public void DashCoolDown()
    {
        GetComponent<Animator>().Play("DashLoading", 0);
    }

    public void EnableDash()
    {
        Player.Instance.GetComponent<PlayerMovement>().EnableDash();
    }
}
