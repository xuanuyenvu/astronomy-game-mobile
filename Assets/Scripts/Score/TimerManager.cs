using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimerManager : MonoBehaviour
{
    public Slider timerSlider;
    public Image star;
    public ParticleSystem psPrefab;
    public Canvas uiCanvas;
    public EnergyManager energyManager;
    public Image fillObject;

    private float maxSliderTimer;
    private float sliderTimer;
    private bool stopeTimer = false;
    private bool isTimeOver = false;
    private bool isFlashing = false;

    public global::System.Boolean IsTimeOver { get => isTimeOver; set => isTimeOver = value; }

    public void SetUp(int time)
    {   
        // fillObject = this.transform.Find("fill");
        if (fillObject != null)
        {
            fillObject.gameObject.SetActive(true);
        }

        maxSliderTimer = (float)time;
        sliderTimer = (float)time;
        timerSlider.maxValue = sliderTimer;
        timerSlider.value = sliderTimer;
        stopeTimer = false;
    }

    public void AddTime()
    {
        // var fillObject = this.transform.Find("fill");
        if (fillObject != null)
        {
            fillObject.gameObject.SetActive(true);
        }
        
        sliderTimer = (float)(maxSliderTimer * 0.35f);
        timerSlider.value = sliderTimer;
        stopeTimer = false;
    }

    public void StartTimer()
    {
        stopeTimer = false;
        if (!gameObject.activeSelf)
        {
            return;
        }
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
                isFlashing = false;
                isTimeOver = true;
                stopeTimer = true;
                GameManager.Instance.CurrentGamePlay.OnTimeOver();
            }
            if (!stopeTimer)
            {
                timerSlider.value = sliderTimer;
                // Khi thời gian còn 15% thì đổi màu fill thành đỏ
                if (sliderTimer / maxSliderTimer <= 0.2f && fillObject != null && !isFlashing)
                {
                    isFlashing = true; // Đánh dấu đang nhấp nháy
                    StartFlashingEffect();
                }
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


    public void StartAddToEnergy()
    {
        if (sliderTimer <= 0)
        {
            return;
        }   

        ParticleSystem particle = Instantiate(psPrefab, uiCanvas.transform);

        // Đặt vị trí ParticleSystem trong UI
        RectTransform particleRect = particle.GetComponent<RectTransform>();
        RectTransform timerRect = this.GetComponent<RectTransform>();
        RectTransform starRect = star.GetComponent<RectTransform>();

        particleRect.position = timerRect.position; // Đặt vị trí ParticleSystem trùng với vị trí của trái tim

        Vector3[] path = new Vector3[]
        {
            timerRect.position,
            (timerRect.position + starRect.position) / 2 + Vector3.down * 90,
            starRect.position
        };

        particle.Play();

        // Tìm đối tượng "fill" và đảm bảo nó không null
        // var fillObject = this.transform.Find("fill");
        if (fillObject != null)
        {
            fillObject.gameObject.SetActive(false);
        }

        // Di chuyển ParticleSystem theo path
        particleRect.DOPath(path, 0.8f, PathType.CatmullRom)
            .SetEase(Ease.InOutQuint)
            .OnComplete(() =>
            {
                particle.Stop();
                Destroy(particle.gameObject, particle.main.duration);
                // energyManager.ChangeEnergy(GetRemainingTimePercentage() * 50);
            });
    }

    void StartFlashingEffect()
    {
        if (fillObject == null) return;
        
        fillObject.DOColor(new Color(1, 0, 0, 0.45f), 0.5f) 
            .SetLoops(-1, LoopType.Yoyo)  // Lặp vô hạn qua lại giữa 2 màu
            .SetEase(Ease.InOutSine); 
    }

}
