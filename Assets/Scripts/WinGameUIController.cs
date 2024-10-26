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
        
        GetChildByName(this.gameObject, "backgroundGO").SetActive(false);
        titleBtnGroupRectTransform = GetChildByName(this.gameObject, "titleBtnGroup").GetComponent<RectTransform>();
        starRectTransform = GetChildByName(energyUI, "star").GetComponent<RectTransform>();
    }

    private void UpdateFinalEnergy()
    {

    }
}
