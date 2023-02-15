using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLine : DialogueBaseClass
{
    [SerializeField] private string text;
    private Text textHolder;

    private void Awake()
    {
        textHolder = GetComponent<Text>();

        StartCoroutine(WriteText(text, textHolder));
    }
}
