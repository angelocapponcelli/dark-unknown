using System;
using Menu;
using UnityEngine;

public class RebindPanelScript : MonoBehaviour
{
    private void OnEnable()
    {
        MenuManager.IsChangingKey = true;
        Debug.Log("enabled");
    }

    private void OnDisable()
    {
        MenuManager.IsChangingKey = false;
        Debug.Log("disabled");
    }
}
