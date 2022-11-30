using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundBank _soundBank;

    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _skeletonSound;
    [SerializeField] private AudioSource _playerSound;

    public void Awake()
    {
        _backgroundMusic.clip = _soundBank.SoundTrack;
    }

    //----- Player -------------------------------------
    public void PlayPLayerWalkSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerWalk);
    }
    public void PlayPLayerAttackSwordSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerAttackSword);
    }
    public void PlayPLayerAttackBowSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerAttackBow);
    }
    public void PlayPLayerRewardSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerReward);
    }
    public void PlayPLayerDashSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerDash);
    }
    public void PlayPLayerHurtSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerHurt[Random.Range(0, _soundBank.PlayerHurt.Count)]);
    }
    public void PlayPLayerDieSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerDie);
    }

    //----- Skeleton -------------------------------------
    public void PlaySkeletonWalkSound()
    {
        _skeletonSound.PlayOneShot(_soundBank.SkeletonWalk);
    }
    public void PlaySkeletonAttackSound()
    {
        _skeletonSound.PlayOneShot(_soundBank.SkeletonAttack);
    }
    public void PlaySkeletonHurtSound()
    {
        _skeletonSound.PlayOneShot(_soundBank.SkeletonHurt[Random.Range(0, _soundBank.SkeletonHurt.Count)]);
    }
    public void PlaySkeletonDieSound()
    {
        _skeletonSound.PlayOneShot(_soundBank.SkeletonDie);
    }

    //-------------------------------------

    public void playSoundTrack()
    {
        _backgroundMusic.Play();
    }
}
