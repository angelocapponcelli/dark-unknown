using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TextBehaviour : PlayableBehaviour
{
    public string dialogueText;
    private string checkText;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Text text = playerData as Text;
        
        var progress = (float)(playable.GetTime() / (playable.GetDuration()-1.5f));
        var subStringLength = Mathf.RoundToInt(Mathf.Clamp01(progress) * dialogueText.Length);
        
        checkText = dialogueText.Substring(0, subStringLength);
        char[] line = checkText.ToCharArray();

        text.text = checkText;
        text.color = new Color(text.color.r, text.color.g, text.color.b, info.weight);
    }
}
