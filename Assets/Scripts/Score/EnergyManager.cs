using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnergyManager : MonoBehaviour
{
    public Slider energySlider;
    public ParticleSystem changeEnergyPS;
    public Image star;
    public float changeSpeed = 1f;

    [HideInInspector] public float currentValue;
    [HideInInspector] public bool isFullEnergy;


    private Vector3 originalScale;
    private Vector3 scaleTo;

    void Start()
    {
        originalScale = star.transform.localScale;
        scaleTo = originalScale * 1.2f;
        changeEnergyPS.Stop();
        energySlider.maxValue = 100;
        SetUp();
    }

    public void SetUp()
    {
        currentValue = 0;
        energySlider.value = currentValue;
        isFullEnergy = false;
    }

    public Tween ChangeEnergy(float increment, Action onComplete = null)
    {
        currentValue = Mathf.Clamp(currentValue + increment, 0, 100);
        float duration = (float)2.5 + (increment / 120);
        Debug.Log("duration " + duration);

        changeEnergyPS.gameObject.SetActive(true);
        changeEnergyPS.Play();

        OnScaleStar(duration);

        var energyTween = energySlider.DOValue(currentValue, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                isFullEnergy = currentValue >= 100;
                if (isFullEnergy)
                {
                    GameManager.Instance.CurrentGamePlay.OnFullEnergy();
                }

                changeEnergyPS.Stop();
                changeEnergyPS.gameObject.SetActive(false);
                onComplete?.Invoke();
            });

        return energyTween;
    }

    private void OnScaleStar(float duration)
    {
        float singleLoopDuration = 0.7f;
        int loops = Mathf.CeilToInt(duration / singleLoopDuration);

        star.transform.DOScale(scaleTo, singleLoopDuration / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(loops * 2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                star.transform.localScale = originalScale;
            });
    }
}