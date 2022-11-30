using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DarkUnknown/SoundBank")]
public class SoundBank : ScriptableObject
{
    [Header ("Player")]
    public AudioClip PlayerWalk;
    public AudioClip PlayerDash;
    public List<AudioClip> PlayerHurt;
    public AudioClip PlayerDie;
    public AudioClip PlayerAttackSword;
    public AudioClip PlayerAttackBow;
    public AudioClip PlayerReward;

    [Header("Skeleton")]
    public AudioClip SkeletonWalk;
    public AudioClip SkeletonAttack;
    public List<AudioClip> SkeletonHurt;
    public AudioClip SkeletonDie;

    [Header("UI")]
    public AudioClip ClickUIButton;
    public AudioClip OverUIButton;

    [Header("Other")]
    public AudioClip SoundTrack;
    

}