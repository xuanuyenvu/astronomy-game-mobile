using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnergyManager : MonoBehaviour
{
    public Slider energySlider;
    public ParticleSystem changeEnergyPS;
    public ParticleSystem hitPS;
    public Image star;
    public float changeSpeed = 1f;

    [HideInInspector] public float currentValue;
    [HideInInspector] public bool isFullEnergy;


    private Vector3 originalScale;
    private Vector3 scaleTo;

    void Awake()
    {
        originalScale = star.transform.localScale;
        scaleTo = originalScale * 1.2f;
    }

    void Start()
    {
        // changeEnergyPS.Stop();
        hitPS.Stop();

        currentValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = currentValue;

        isFullEnergy = false;
    }

    public Tween ChangeEnergy(float increment)
    {
        currentValue = Mathf.Clamp(currentValue + increment, 0, 100);
        float duration = (float)(increment / 10);

        changeEnergyPS.gameObject.SetActive(true);
        changeEnergyPS.Play();

        OnScaleStar(duration);

        var energyTween = energySlider.DOValue(currentValue, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                changeEnergyPS.Stop();

                isFullEnergy = currentValue >= 100;
                if (isFullEnergy)
                {
                    GameManager.Instance.CurrentGamePlay.OnFullEnergy();
                }
            });
        StartCoroutine(StopChangeEnergyPS());
        return energyTween;
    }

    private IEnumerator StopChangeEnergyPS()
    {
        yield return new WaitForSeconds(10f);
        changeEnergyPS.gameObject.SetActive(false);
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