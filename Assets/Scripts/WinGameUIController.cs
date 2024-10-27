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

    void Awake()
    {
        // GetChildByName(this.gameObject, "backgroundWin").SetActive(false);
    }

    // private void UpdateFinalEnergy()
    // {
    //     int health = currentGamePlay.healthManager.health; // * 10
    //     for (int i = 0; i < health; i++)
    //     {
    //         currentGamePlay.energyManager.ChangeEnergy(10);
    //     }

    //     float time = currentGamePlay.timerManager.GetRemainingTimePercentage(); // * 10
    //     currentGamePlay.energyManager.ChangeEnergy(time * 50);
    // }
}
