using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public HealthManager healthManager;
    public GameObject energyUI;
    public GameObject background;
    public CameraShake cameraShake;

    private EnergyManager energyManager;
    private TimerManager timerManager;
    void Awake()
    {
        energyManager = energyUI.GetComponent<EnergyManager>();
        timerManager = timeBarUI.GetComponent<TimerManager>();
    }

    public void StartUI()
    {
        int health = healthManager.health;
        float timeRemaining = timerManager.GetRemainingTimePercentage();
        energyManager.ChangeEnergy((health * 10) + (timeRemaining * 50));
        StartCoroutine(UpdateFinalEnergy());
    }

    private IEnumerator UpdateFinalEnergy()
    {
        timerManager.StartAddToEnergy();
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(healthManager.StartAddToEnergy());
    }


}
