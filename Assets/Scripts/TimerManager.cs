using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public Slider timerSlider;
    public float sliderTimer;
    public bool stopeTimer = false;

    public void SetUp(int time)
    {
        sliderTimer = (float) time;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        stopeTimer = false;
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(StartTheTimerTicker());
    }

    IEnumerator StartTheTimerTicker()
    {
        while (!stopeTimer)
        {
            sliderTimer -= Time.deltaTime;
            yield return new WaitForSeconds(0.001f);

            if (sliderTimer <= 0)
            {
                stopeTimer = true;
            }
            if(!stopeTimer)
            {
                timerSlider.value = sliderTimer;
            }
        }
    }

    public void StopTimer()
    {
        stopeTimer = true;
    }

    public void ResumeTimer()
    {
        stopeTimer = false;
        StartCoroutine(StartTheTimerTicker());
    }

}
