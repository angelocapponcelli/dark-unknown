using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public IUsable MyUsable { get; set; }

    public Button MyButton { get; private set; }

    public Image MyIcon
    {
        get => icon;
        set => icon = value;
    }
    
    public Image MyKeyIcon
    {
        get => keyIcon;
        set => keyIcon = value;
    }

    [SerializeField] private Image icon;
    [SerializeField] private Image keyIcon;
    
    private void Awake()
    {
        MyButton = GetComponent<Button>();
        MyButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MyUsable?.Use();
    }
}
