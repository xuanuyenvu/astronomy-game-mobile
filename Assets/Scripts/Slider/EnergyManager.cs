using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnergyManager : MonoBehaviour
{
    public Slider energySlider;
    public ParticleSystem particleSystem;
    public float changeSpeed = 1f;

    [HideInInspector] public float currentValue;
    [HideInInspector] public bool isFullEnergy;

    void Start()
    {
        particleSystem.gameObject.SetActive(false);
        particleSystem.Stop();
        currentValue = 0;
        energySlider.maxValue = 100;
        energySlider.value = currentValue;
        isFullEnergy = false;
    }

    public Tween ChangeEnergy(float increment)
    {
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();

        currentValue += increment;

        return energySlider.DOValue(currentValue, increment / 30)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                particleSystem.Stop();
                Debug.Log("PS" + particleSystem.gameObject.name);
                particleSystem.gameObject.SetActive(false);

                isFullEnergy = currentValue >= 100;
            });
    }
}