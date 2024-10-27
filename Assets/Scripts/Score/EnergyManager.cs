using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnergyManager : MonoBehaviour
{
    public Slider energySlider;
    public ParticleSystem particleSystem;
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

        particleSystem.gameObject.SetActive(false);
        particleSystem.Stop();

        currentValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = currentValue;

        isFullEnergy = false;
    }

    public Tween ChangeEnergy(float increment)
    {
        currentValue = Mathf.Clamp(currentValue + increment, 0, 100);
        float duration = (float)(increment / 8);

        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();

        OnScaleStar(duration);
        var energyTween = energySlider.DOValue(currentValue, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);

                isFullEnergy = currentValue >= 100;
                if (isFullEnergy)
                {
                    GameManager.Instance.CurrentGamePlay.OnFullEnergy();
                }
            });

        return energyTween;
    }

    private void OnScaleStar(float duration)
    {
        float singleLoopDuration = duration / 6;
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