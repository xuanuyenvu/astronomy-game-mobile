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
    private int isShake = -1; // ban đầu = -1, start = 1, stop = 0
    // đặt vậy là vì cần phân biệt giữa trạng thái ban đầu và trạng thái stop
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    public global::System.Int32 IsShake { get => isShake; set => isShake = value; }

    void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        StopShake(-1);
    }

    public void ShakeCamera()
    {
        if(IsShake != 1)
        {
            IsShake = 1;
        }
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = shakeIntensity;
        timer = shakeTime;
    }
    void StopShake(int valueIsShake = 0)
    {
        _cbmcp = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = 0f;
        timer = shakeTime;
        IsShake = valueIsShake;
    }
    void Update()
    {
        if (timer > 0 && IsShake == 1)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StopShake();
            }
        }
    }
}
