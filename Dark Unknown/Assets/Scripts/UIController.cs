using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Slider _healtBar;
    [SerializeField] private Text _roomText;
    [SerializeField] private Text _speedMultiplierText;
    [SerializeField] private Text _strengthMultiplierText;

    public void SetMaxHealth(float health)
    {
        _healtBar.maxValue = health;
        _healtBar.value = health;
    }
    public void SetHealth(float value)
    {
        _healtBar.value = value;
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

}
