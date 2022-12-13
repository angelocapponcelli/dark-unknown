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
}
