using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class WarpSpeedController : MonoBehaviour
{
    public BackgroundScroller backgroundScroller;
    public VisualEffect warpSpeedVFX1;
    public VisualEffect warpSpeedVFX2;
    public float rate = 0.9f;
    public CameraShake cameraShake;
    private bool warpActive;
    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        warpSpeedVFX1.Stop();
        warpSpeedVFX1.SetFloat("WarpAmount", 0);
        warpSpeedVFX2.Stop();
        warpSpeedVFX2.SetFloat("WarpAmount", 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            warpActive = true;
            StartCoroutine(ActivateParticles());
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            warpActive = false;
            StartCoroutine(DeactivateParticles());
        }
    }

    private IEnumerator ActivateParticles()
    {
        warpSpeedVFX1.Play();

        float amount1 = warpSpeedVFX1.GetFloat("WarpAmount");
        while (amount1 < 1 & warpActive)
        {
            amount1 += rate;
            warpSpeedVFX1.SetFloat("WarpAmount", amount1);
            cameraShake.ShakeCamera(1.6f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DeactivateParticles()
    {
        float amount1 = warpSpeedVFX1.GetFloat("WarpAmount");
        while (amount1 > 0 & !warpActive)
        {
            amount1 -= rate;
            warpSpeedVFX1.SetFloat("WarpAmount", amount1);
            yield return new WaitForSeconds(0.1f);

            if (amount1 <= 0 + rate)
            {
                amount1 = 0;
                warpSpeedVFX1.SetFloat("WarpAmount", amount1);
                warpSpeedVFX1.Stop();
            }
        }
    }

    public IEnumerator ActivateForThreeSeconds()
    {
        warpSpeedVFX1.Play();
        warpSpeedVFX2.Play();
        var bgSpeed = backgroundScroller.speed;

        cameraShake.ShakeCamera(6f);
        float amount1 = warpSpeedVFX1.GetFloat("WarpAmount");
        float amount2 = warpSpeedVFX2.GetFloat("WarpAmount");
        while (amount1 < 1)
        {
            amount1 += rate;
            amount2 += rate / 9;

            warpSpeedVFX1.SetFloat("WarpAmount", amount1);
            warpSpeedVFX2.SetFloat("WarpAmount", amount2);

            backgroundScroller.speed = -2f;
            yield return new WaitForSeconds(0.1f);
        }
        
        amount1 = 0;
        amount2 = 0;

        warpSpeedVFX1.SetFloat("WarpAmount", amount1);
        warpSpeedVFX2.SetFloat("WarpAmount", amount2);

        warpSpeedVFX2.Stop();
        warpSpeedVFX1.Stop();

        backgroundScroller.speed = bgSpeed;
        yield return new WaitForSeconds(2f);
        cameraShake.StopShake();
        yield return null;
    }
}
