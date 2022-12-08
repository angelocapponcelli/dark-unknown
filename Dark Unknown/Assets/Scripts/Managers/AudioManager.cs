using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundBank _soundBank;

    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _UISound;
    [SerializeField] private AudioSource _skeletonSound;
    [SerializeField] private AudioSource _playerSound;

    protected new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public void Start()
    {
        _backgroundMusic.clip = _soundBank.SoundTrack;
    }

    public void PlayOverUIButtonSound()
    {
        _UISound.PlayOneShot(_soundBank.OverUIButton);
    }
    public void PlayClickUIButtonSound()
    {
        _UISound.PlayOneShot(_soundBank.ClickUIButton);
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
    public void SetPlayerVolume(float value)
    {
        _playerSound.volume = value;
    }
    public float GetPlayerVolumeSound()
    {
        return _playerSound.volume;
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
        print("SOUND PLAYED");
    }
    public void SetSkeletonVolume(float value) //TODO 'skeleton' change into 'enemy'
    {
        _skeletonSound.volume = value;
    }
    public float GetEnemyVolumeSound()
    {
        return _skeletonSound.volume;
    }

    //-------------------------------------

    public void PlaySoundTrack()
    {
        _backgroundMusic.Play();
    }

    public void StopSoundTrack()
    {
        _backgroundMusic.Stop();
    }
    public void SetBackgroundVolume(float value)
    {
        _backgroundMusic.volume = value;
    }
    public float GetBackgroundVolumeSound()
    {
        return _backgroundMusic.volume;
    }

    public void PlayEnterDoorSound()
    {
        _UISound.PlayOneShot(_soundBank.EnterDoor);
    }
}
