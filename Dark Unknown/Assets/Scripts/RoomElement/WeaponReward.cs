using Unity.Collections;
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
                IUsable usable;
                if (_weaponPrefab.gameObject.CompareTag("Axe")) usable = new AxeUsable();
                else if (_weaponPrefab.gameObject.CompareTag("Sword")) usable = new SwordUsable();
                else usable = new BowUsable();
                character.GetComponentInParent<Player>().ChangeWeapon(_weaponPrefab, gameObject, usable);
            }
        }
    }
    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Player.Instance.ShowPlayerUI(false, "");
            Player.Instance.DisableCanGetWeapon();
        }
    }
}
