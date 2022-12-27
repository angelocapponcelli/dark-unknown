using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurryText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text> ().font.material.mainTexture.filterMode = FilterMode.Point;
    }
}
