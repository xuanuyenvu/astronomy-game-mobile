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
    }

    public void StartUI()
    {
        UpdateFinalEnergy();
    }

    private void UpdateFinalEnergy()
    {
        int health = healthManager.health; // * 10
        for (int i = 0; i < health; i++)
        {
            energyManager.ChangeEnergy(10);
        }

        float time = timerManager.GetRemainingTimePercentage(); // * 50
        energyManager.ChangeEnergy(time * 50);
    }


}
