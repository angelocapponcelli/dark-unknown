using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public IUsable MyUsable { get; set; }

    public Button MyButton { get; private set; }
    
    //TODO: implement dynamic key sprite

    public Image MyIcon
    {
        get => icon;
        set => icon = value;
    }

    [SerializeField] private Image icon;
    
    private void Awake()
    {
        MyButton = GetComponent<Button>();
        // fix infinitely usable potion on mouse click
        MyButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        MyUsable?.Use();
    }
}
