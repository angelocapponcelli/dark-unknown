using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ShakeOnHit : MonoBehaviour
{
    private CinemachineVirtualCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;
    [SerializeField] private float hitAmplitudeGain, hitFrequencyGain = 1;
    [SerializeField] private float shakeTime = 1;
    private float shakeTimeElapsed;
    private bool isShaking = false;

    private void Awake()
    {
        vCam = GetComponent<CinemachineVirtualCamera>();
        noise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        PlayerEvents.playerHit += () => Shake();
        shakeTimeElapsed = 0;
    }

    private void OnDestroy()
    {
        PlayerEvents.playerHit -= () => Shake();
    }

    private void Update()
    {
        shakeTimeElapsed += Time.deltaTime;

        if (shakeTimeElapsed > shakeTime)
        {
            StopShake();
        }

    }

    private void Shake()
    {
        noise.m_AmplitudeGain = hitAmplitudeGain;
        noise.m_FrequencyGain = hitFrequencyGain;
        shakeTimeElapsed = 0;
        isShaking = true;
    }

    private void StopShake()
    {
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        isShaking = false;
    }
}
