using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeIntensity = 3f;
    private float shakeTime = 0.9f;
    private float timer;
    private bool isShake = false;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    public global::System.Boolean IsShake { get => isShake; }

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        StopShake();
    }

    public void ShakeCamera()
    {
        if(!isShake)
        {
            isShake = true;
        }
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        timer = shakeTime;
    }
    void StopShake()
    {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = shakeTime;
        isShake = false;
    }
    void Update()
    {
        if (timer > 0 && isShake)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
