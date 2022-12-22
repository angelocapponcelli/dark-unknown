using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
    public IUsable MyUsable { get; set; }

    public Button MyButton { get; private set; }
    
    // Start is called before the first frame update
    private void Awake()
    {
        MyButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        MyUsable?.Use();
    }
}
