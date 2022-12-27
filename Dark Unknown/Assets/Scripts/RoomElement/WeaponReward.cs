using UnityEngine;

public class WeaponReward : Reward
{
    [SerializeField] private WeaponParent _weaponPrefab;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        Collider2D[] hitCharacters = collision.GetComponents<Collider2D>();
        foreach (Collider2D character in hitCharacters)
        {
            if (character.gameObject.CompareTag("Player"))
            {
                character.GetComponentInParent<Player>().ChangeWeapon(_weaponPrefab, gameObject);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            Player.Instance.disableCanGetWeapon();
        }
    }
}
