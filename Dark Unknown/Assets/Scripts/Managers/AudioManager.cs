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
    [SerializeField] private AudioSource _enemySound;
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

    //++++++ ENEMY ++++++++++++++++++++++++++++++
    //----- Skeleton -------------------------------------
    public void PlaySkeletonWalkSound()
    {
        _enemySound.PlayOneShot(_soundBank.SkeletonWalk);
    }
    public void PlaySkeletonAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.SkeletonAttack);
    }
    public void PlaySkeletonHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.SkeletonHurt[Random.Range(0, _soundBank.SkeletonHurt.Count)]);
    }
    public void PlaySkeletonDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.SkeletonDie);
    }
    //----- Spider -------------------------------------
    public void PlaySpiderAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.SpiderAttack);
    }
    public void PlaySpiderHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.SpiderHurt);
    }
    public void PlaySpiderDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.SpiderDie);
    }
    //----- Worm -------------------------------------
    public void PlayWormAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.WormAttack);
    }
    public void PlayWormHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.WormHurt);
    }
    public void PlayWormDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.WormDie);
    }
    //----- Viper -------------------------------------
    public void PlayViperAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.ViperAttack);
    }
    public void PlayViperHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.ViperHurt);
    }
    public void PlayViperDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.ViperDie);
    }
    //----- Undead -------------------------------------
    public void PlayUndeadAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.UndeadAttack);
    }
    public void PlayUndeadHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.UndeadHurt);
    }
    public void PlayUndeadDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.UndeadDie);
    }
    //----- MutantRat -------------------------------------
    public void PlayMutantRatAttackSound()
    {
        _enemySound.PlayOneShot(_soundBank.MutantRatAttack);
    }
    public void PlayMutantRatHurtSound()
    {
        _enemySound.PlayOneShot(_soundBank.MutantRatHurt);
    }
    public void PlayMutantRatDieSound()
    {
        _enemySound.PlayOneShot(_soundBank.MutantRatDie);
    }
    public void SetEnemyVolume(float value) //TODO 'skeleton' change into 'enemy'
    {
        _enemySound.volume = value;
    }
    public float GetEnemyVolumeSound()
    {
        return _enemySound.volume;
    }

    //-------------------------------------

    public void PlaySoundTrack()
    {
        _backgroundMusic.clip = _soundBank.SoundTrack;
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
    public void PlaySoundTrackIntro()
    {
        _backgroundMusic.clip = _soundBank.SoundTrackIntro;
        _backgroundMusic.Play();
    }

    public void StopSoundTrackIntro()
    {
        _backgroundMusic.Stop();
    }
    
    public void PlayTeleportSound()
    {
        _backgroundMusic.PlayOneShot(_soundBank.Teleport);
    }
    public void PlayEnterDoorSound()
    {
        _backgroundMusic.PlayOneShot(_soundBank.EnterDoor);
    }
}
