using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : Singleton<MonoBehaviour>
{
    [System.Serializable]
    public class AnimationSettings
    {
        public enum OpenStyle{ WidthToHeight, HeightToWidth, HeightAndWidth }
        public OpenStyle openStyle;
        public float widthSmooth = 4.6f, heightSmooth = 4.6f;
        public float textSmooth = 0.1f;

        [HideInInspector] public bool widthOpen = false, heightOpen = false;

        public void Initialize()
        {
            widthOpen = false;
            heightOpen = false;
        }
    }
    
    [System.Serializable]
    public class UISettings
    {
        public Image textBox;
        public Text text;
        public Vector2 initializeBoxSize = new Vector2(0.25f, 0.25f);
        public Vector2 openedBoxSize = new Vector2(400, 200);
        public float snapToSizeDistance = 0.25f;
        public float lifeSpan = 5;

        [HideInInspector] public bool opening = false;
        [HideInInspector] public Color textColor;
        [HideInInspector] public Color textBoxColor;
        [HideInInspector] public RectTransform textBoxRect;
        [HideInInspector] public Vector2 currentSize;

        public void Initialize()
        {
            textBoxRect = textBox.GetComponent<RectTransform>();
            textBoxRect.sizeDelta = initializeBoxSize;
            currentSize = textBoxRect.sizeDelta;
            opening = false;

            textColor = text.color;
            textColor.a = 0;
            text.color = textColor;
            textBoxColor = textBox.color;
            textBoxColor.a = 1;
            textBox.color = textBoxColor;
            
            textBox.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
        }
    }

    public AnimationSettings animSettings = new AnimationSettings();
    public UISettings uiSettings = new UISettings();

    private float _lifeTimer = 0;

    private void Start()
    {
        animSettings.Initialize();
        uiSettings.Initialize();
    }

    public void StartOpen()
    {
        uiSettings.opening = true;
        uiSettings.textBox.gameObject.SetActive(true);
        uiSettings.text.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (uiSettings.opening)
        {
            OpenToolTip();
            if (animSettings.widthOpen && animSettings.heightOpen)
            {
                _lifeTimer += Time.deltaTime;
                //Debug.Log(_lifeTimer);
                if (_lifeTimer > uiSettings.lifeSpan) FadeToolTipOut();
                else FadeTextIn();
            }
        }
    }

    private void OpenToolTip()
    {
        switch (animSettings.openStyle)
        {
            case AnimationSettings.OpenStyle.HeightToWidth:
                OpenHeightToWidth();
                break;
            case AnimationSettings.OpenStyle.WidthToHeight:
                OpenWidthToHeight();
                break;
            case AnimationSettings.OpenStyle.HeightAndWidth:
                OpenHeightAndWidth();
                break;
            default:
                Debug.LogError("No animation is set for the selected open style!");
                break;
        }

        uiSettings.textBoxRect.sizeDelta = uiSettings.currentSize;
    }

    private void OpenHeightAndWidth()
    {
        if (!animSettings.widthOpen) OpenWidth();
        if (!animSettings.heightOpen) OpenHeight();
    }

    private void OpenHeightToWidth()
    {
        if (!animSettings.heightOpen) OpenHeight();
        else OpenWidth();
    }

    private void OpenWidthToHeight()
    {
        if (!animSettings.widthOpen) OpenWidth();
        else OpenHeight();
    }
    
    private void OpenWidth()
    {
        uiSettings.currentSize.x = Mathf.Lerp(uiSettings.currentSize.x, uiSettings.openedBoxSize.x,
            animSettings.widthSmooth * Time.deltaTime);
        if (Mathf.Abs(uiSettings.currentSize.x - uiSettings.openedBoxSize.x) < uiSettings.snapToSizeDistance)
        {
            uiSettings.currentSize.x = uiSettings.openedBoxSize.x;
            animSettings.widthOpen = true;
        }
    }
    
    private void OpenHeight()
    {
        uiSettings.currentSize.y = Mathf.Lerp(uiSettings.currentSize.y, uiSettings.openedBoxSize.y,
            animSettings.widthSmooth * Time.deltaTime);
        if (Mathf.Abs(uiSettings.currentSize.y - uiSettings.openedBoxSize.y) < uiSettings.snapToSizeDistance)
        {
            uiSettings.currentSize.y = uiSettings.openedBoxSize.y;
            animSettings.heightOpen = true;
        }
    }

    private void FadeToolTipOut()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
        uiSettings.textBoxColor.a = Mathf.Lerp(uiSettings.textBoxColor.a, 0, animSettings.textSmooth * Time.deltaTime);
        uiSettings.textBox.color = uiSettings.textBoxColor;

        if (uiSettings.textBoxColor.a < 0.01f)
        {
            uiSettings.opening = false;
            animSettings.Initialize();
            uiSettings.Initialize();
            _lifeTimer = 0;
        }
    }

    private void FadeTextIn()
    {
        uiSettings.textColor.a = Mathf.Lerp(uiSettings.textColor.a, 1, animSettings.textSmooth * Time.deltaTime);
        uiSettings.text.color = uiSettings.textColor;
        Debug.Log("text fading in.");
    }
}
