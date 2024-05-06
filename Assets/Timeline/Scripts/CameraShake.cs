using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera CinemachineVirtualCamera;
    private float shakeIntensity = 2f;
    private float shakeTime = 3;

    private float timer;
    private CinemachineBasicMultiChannelPerlin mPerlin;

    private void Awake()
    {
        CinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        mPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        mPerlin.m_AmplitudeGain = shakeIntensity;

        timer = shakeTime;
    }

    public void StopShake()
    {
        mPerlin = CinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        mPerlin.m_AmplitudeGain = 0;

        timer = 0;
    }

    private void Update()
    {
        if (timer > 0) 
        {
            timer -= Time.deltaTime;

            if (timer <= 0) 
            { 
                StopShake();
            }
        }
    }
}
