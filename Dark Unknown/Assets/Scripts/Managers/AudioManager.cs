using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private SoundBank _soundBank;

    [SerializeField] private AudioSource _backgroundMusic;
    [SerializeField] private AudioSource _skeletonSound;
    [SerializeField] private AudioSource _playerSound;

    //----- Player -------------------------------------
    public void PlayPLayerWalkSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerWalk);
    }
    public void PlayPLayerAttackSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerAttack);
    }
    public void PlayPLayerDashSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerDash);
    }
    public void PlayPLayerHurtSound()
    {
        _playerSound.PlayOneShot(_soundBank.PlayerHurt);
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
}
