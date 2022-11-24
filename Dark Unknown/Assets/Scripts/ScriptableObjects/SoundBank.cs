using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DarkUnknown/SoundBank")]
public class SoundBank : ScriptableObject
{
    [Header ("Player")]
    public AudioClip PlayerWalk;
    public AudioClip PlayerDash;
    public AudioClip PlayerHurt;
    public AudioClip PlayerDie;
    public AudioClip PlayerAttack;

    [Header("Skeleton")]
    public AudioClip SkeletonWalk;
    public AudioClip SkeletonAttack;
    public List<AudioClip> SkeletonHurt;
    public AudioClip SkeletonDie;
    
    [Header("Other")]
    public AudioClip QuietBeat;
    public AudioClip ExcitingBeat;

    public float QuietBeatTiming = 1f;
    public float ExcitingBeatTiming = .5f;
}