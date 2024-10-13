using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public Slider energySlider;
    public ParticleSystem particleSystem;
    public float changeSpeed = 1f;

    [HideInInspector] public float percent;
    [HideInInspector] public bool isFullEnergy;

    void Start()
    {
        particleSystem.gameObject.SetActive(false);
        particleSystem.Stop();
        percent = 0;
        energySlider.maxValue = 100;
        energySlider.value = percent;
        isFullEnergy = false;
    }

    public IEnumerator ChangeEnergy(float changeValue)
    {
        particleSystem.gameObject.SetActive(true);
        particleSystem.Play();

        float targetValue = Mathf.Clamp(percent + changeValue, 0, 100);
        while (Mathf.Abs(energySlider.value - targetValue) > 1f)
        {
            energySlider.value = Mathf.Lerp(energySlider.value, targetValue, Time.deltaTime * changeSpeed);
            percent = energySlider.value;
            yield return null;
        }
        energySlider.value = targetValue;
        percent = targetValue;

        particleSystem.Stop();
        particleSystem.Clear();
        particleSystem.gameObject.SetActive(false);

        isFullEnergy = percent >= 100;
    }
}