using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TextClip : PlayableAsset
{
    public string text;
    
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<TextBehaviour>.Create(graph);

        TextBehaviour behaviour = playable.GetBehaviour();
        behaviour.dialogueText = text;
        
        return playable;
    }
}
