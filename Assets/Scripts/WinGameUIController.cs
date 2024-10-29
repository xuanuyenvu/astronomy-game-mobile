using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinGameUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public HealthManager healthManager;
    public GameObject energyUI;
    public ParticleSystem starEffectPS;
    public GameObject background;
    public CameraShake cameraShake;
    public Canvas uiCanvas;
    public ParticleSystem glowPS;

    private EnergyManager energyManager;
    private TimerManager timerManager;
    private RectTransform starRectTransform;

    void Awake()
    {
        glowPS.Stop();
        energyManager = energyUI.GetComponent<EnergyManager>();
        timerManager = timeBarUI.GetComponent<TimerManager>();
        starRectTransform = GetChildByName(energyUI, "star").GetComponent<RectTransform>();
    }

    public void StartUI(float energy)
    {
        int health = healthManager.health;
        float timeRemaining = timerManager.GetRemainingTimePercentage();
        energyManager.ChangeEnergy(energy + (health * 10) + (timeRemaining * 50), () => ModifyUI());
        StartCoroutine(UpdateFinalEnergy());
    }

    private IEnumerator UpdateFinalEnergy()
    {
        timerManager.StartAddToEnergy();
        yield return new WaitForSeconds(1f);
        StartCoroutine(healthManager.StartAddToEnergy());
    }

    private void ModifyUI()
    {
        HideUIElements();
        ActivateStarEffect();
        MoveAndScaleStar();
    }

    private void HideUIElements()
    {
        timeBarUI.GetComponent<TimerManager>().StopTimer();
        timeBarUI.SetActive(false);
        healthManager.SetUp(0);

        GetChildByName(energyUI, "border").SetActive(false);
        GetChildByName(energyUI, "background").SetActive(false);
        GetChildByName(this.gameObject, "backgroundWin").SetActive(true);
    }

    private void ActivateStarEffect()
    {
        starEffectPS.gameObject.SetActive(true);
        starEffectPS.Play();
    }

    private void MoveAndScaleStar()
    {
        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        
        starEffectPS.transform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);
        starRectTransform.DOMove(targetPosition, 1f).SetEase(Ease.OutQuad);

        Vector3 targetScalePS = starEffectPS.transform.localScale * 3.3f;
        starEffectPS.transform.DOScale(targetScalePS, 1f);

        Vector3 targetScale = starRectTransform.localScale * 3.3f;
        starRectTransform.DOScale(targetScale, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => StartCoroutine(ShowLevelResult()))
            .WaitForCompletion();
    }

    private IEnumerator ShowLevelResult()
    {
        cameraShake.ShakeCamera(0.3f);
        yield return new WaitForSeconds(0.2f);
        if (starRectTransform != null)
        {
            starRectTransform.DOScale(new Vector3(0.55f, 0.45f, 1f), 0.5f).SetEase(Ease.OutQuad);
        }
        starEffectPS.transform.DOScale(new Vector3(73, 73, 73), 0.5f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(0.5f);

        starRectTransform.anchoredPosition = Vector2.zero;
        starEffectPS.transform.localPosition = Vector3.zero;

        starRectTransform.gameObject.SetActive(false);
        starEffectPS.gameObject.SetActive(false);

        glowPS.Play();
    }


    private GameObject GetChildByName(GameObject parent, string childName)
    {
        Transform child = parent.transform.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }
        else
        {
            Debug.Log($"Child with the name '{childName}' does not exist in '{parent.name}'.");
            return null;
        }
    }
}
