using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    public Slider timerSlider;
    private float maxSliderTimer;
    private float sliderTimer;
    private bool stopeTimer = false;

    private bool isTimeOver = false;

    public global::System.Boolean IsTimeOver { get => isTimeOver; set => isTimeOver = value; }

    public void SetUp(int time)
    {
        maxSliderTimer = (float)time;
        sliderTimer = (float)time;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        stopeTimer = false;
    }

    public void StartTimer()
    {
        stopeTimer = false;
        StartCoroutine(StartTheTimerTicker());
        // StartTheTimerTickerByDotween();
    }

    IEnumerator StartTheTimerTicker()
    {
        while (!stopeTimer)
        {
            sliderTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (sliderTimer <= 0)
            {
                isTimeOver = true;
                stopeTimer = true;
                GameManager.Instance.CurrentGamePlay.OnTimeOver();
            }
            if (!stopeTimer)
            {
                timerSlider.value = sliderTimer;
            }
        }
    }

    public void StopTimer()
    {
        if (stopeTimer)
        {
            return;
        }
        stopeTimer = true;
    }

    public void StartTheTimerTickerByDotween()
    {
        DOTween.To(() => sliderTimer, x => sliderTimer = x, 0, sliderTimer).OnUpdate(() =>
        {
            if (!stopeTimer)
            {
                timerSlider.value = sliderTimer;
            }
        }).OnComplete(() =>
        {
            stopeTimer = true;
        });
    }

    public float GetRemainingTimePercentage()
    {
        return sliderTimer / maxSliderTimer;
    }

    public void AddToEnergy()
    {
        stopeTimer = true;
        while (sliderTimer > 0)
        {
            
        }
    }
}
