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
    
    [Header("Spider")]
    public AudioClip SpiderAttack;
    public AudioClip SpiderHurt;
    public AudioClip SpiderDie;
    
    [Header("Worm")]
    public AudioClip WormAttack;
    public AudioClip WormHurt;
    public AudioClip WormDie;
    
    [Header("Viper")]
    public AudioClip ViperAttack;
    public AudioClip ViperHurt;
    public AudioClip ViperDie;
    
    [Header("Undead")]
    public AudioClip UndeadAttack;
    public AudioClip UndeadHurt;
    public AudioClip UndeadDie;
    
    [Header("MutantRat")]
    public AudioClip MutantRatAttack;
    public AudioClip MutantRatHurt;
    public AudioClip MutantRatDie;

    [Header("UI")]
    public AudioClip ClickUIButton;
    public AudioClip OverUIButton;

    [Header("Other")]
    public AudioClip SoundTrack;
    public AudioClip SoundTrackIntro;
    public AudioClip Teleport;
    public AudioClip EnterDoor;
}