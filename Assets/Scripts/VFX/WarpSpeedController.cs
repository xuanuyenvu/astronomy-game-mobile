using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using DG.Tweening;

public class WarpSpeedController : MonoBehaviour
{
    public BackgroundScroller backgroundScroller;
    public VisualEffect warpSpeedVFX;
    public float rate = 0.1f;
    public CameraShake cameraShake;
    private bool warpActive;
    void Start()
    {
        cameraShake = FindObjectOfType<CameraShake>();
        warpSpeedVFX.Stop();
        warpSpeedVFX.SetFloat("WarpAmount", 0);
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
        warpSpeedVFX.Play();

        float amount = warpSpeedVFX.GetFloat("WarpAmount");
        while (amount < 1 & warpActive)
        {
            amount += rate;
            warpSpeedVFX.SetFloat("WarpAmount", amount);
            cameraShake.ShakeCamera(1.6f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator DeactivateParticles()
    {
        float amount = warpSpeedVFX.GetFloat("WarpAmount");
        while (amount > 0 & !warpActive)
        {
            amount -= rate;
            warpSpeedVFX.SetFloat("WarpAmount", amount);
            yield return new WaitForSeconds(0.1f);

            if (amount <= 0 + rate)
            {
                amount = 0;
                warpSpeedVFX.SetFloat("WarpAmount", amount);
                warpSpeedVFX.Stop();
            }
        }
    }

    public IEnumerator ActivateForThreeSeconds()
    {
        warpSpeedVFX.Play();
        var bgSpeed = backgroundScroller.speed;

        cameraShake.ShakeCamera(6f);
        float amount = warpSpeedVFX.GetFloat("WarpAmount");
        while (amount < 1)
        {
            amount += rate;
            warpSpeedVFX.SetFloat("WarpAmount", amount);
            backgroundScroller.speed = -2f;
            yield return new WaitForSeconds(0.1f);
        }
        // cameraShake.StopShake(0);
        // warpSpeedVFX.DOFloat(1, "WarpAmount", 1.5f)
        //     .OnUpdate(() =>
        //         {
        //             cameraShake.ShakeCamera(1.5f);
        //         })
        //     .SetEase(Ease.OutCubic);

        // yield return new WaitForSeconds(0.5f);

        // while (amount > 0)
        // {
        //     amount -= rate;
        //     warpSpeedVFX.SetFloat("WarpAmount", amount);
        //     yield return new WaitForSeconds(0.1f);
        // }
        amount = 0;
        warpSpeedVFX.SetFloat("WarpAmount", amount);

        warpSpeedVFX.Stop();
        backgroundScroller.speed = bgSpeed;
        cameraShake.StopShake();
        yield return null;
    }
}
