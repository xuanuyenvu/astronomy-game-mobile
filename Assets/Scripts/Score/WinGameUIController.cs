using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Reward
{
    public GameObject star;
    public ParticleSystem shineLightPS;
    public TextMeshProUGUI text;
}


public class WinGameUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public HealthManager healthManager;
    public GameObject energyUI;
    public GameObject stageUI;
    public GameObject tutorialUI;
    
    public ParticleSystem starEffectPS;

    public GameObject darkBg;
    public CameraShake cameraShake;
    public ParticleSystem glowPS;
    public List<Reward> rewards;

    public TextMeshProUGUI tapText;
    public Button tapBtn;
    public GameObject btnGroup;

    private EnergyManager energyManager;
    private TimerManager timerManager;
    private RectTransform starRectTransform;

    void Start()
    {
        glowPS.Stop();
        energyManager = energyUI.GetComponent<EnergyManager>();
        if (timeBarUI != null && timeBarUI.activeSelf)
        {
            timerManager = timeBarUI.GetComponent<TimerManager>();
        }
        else
        {
            timerManager = null;
        }

        starRectTransform = GetChildByName(energyUI, "star").GetComponent<RectTransform>();
    }

    public void StartUI(float energy)
    {
        btnGroup.SetActive(false);
        tutorialUI.SetActive(false);
        // stageUI.SetActive(false);

        int health = healthManager.Health;
        float timeRemaining = 0;
        if (timerManager != null)
        {
            timeRemaining = timerManager.GetRemainingTimePercentage();
        }

        energyManager.ChangeEnergy(energy + (health * 10) + (timeRemaining * 50), () => ModifyUI());
        StartCoroutine(UpdateFinalEnergy());
    }

    private IEnumerator UpdateFinalEnergy()
    {
        if (timerManager != null)
        {
            timerManager.StartAddToEnergy();
        }
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
        stageUI.SetActive(false);
        // tutorialUI.SetActive(false);

        GetChildByName(energyUI, "border").SetActive(false);
        GetChildByName(energyUI, "background").SetActive(false);
        darkBg.SetActive(true);
        // GetChildByName(this.gameObject, "backgroundWin").SetActive(true);
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
            starRectTransform.DOScale(new Vector3(0.55f, 0.45f, 1f), 0.3f)
                .SetEase(Ease.OutQuad);
        }

        yield return new WaitForSeconds(0.2f);

        starRectTransform.anchoredPosition = Vector2.zero;
        starEffectPS.transform.localPosition = Vector3.zero;

        starRectTransform.gameObject.SetActive(false);
        starEffectPS.gameObject.SetActive(false);

        AudioManager.Instance.PlaySFX("Win");
        glowPS.Play();
        yield return new WaitForSeconds(0.2f);

        Sequence sequence = DOTween.Sequence();

        if (!energyManager.isFullEnergy)
        {
            rewards[0].shineLightPS.gameObject.SetActive(true);
            rewards[0].shineLightPS.Play();
            sequence.Append(rewards[0].star.transform.DOScale(new Vector3(0.065f, 0.065f, 0.065f), 0.3f)
                    .SetEase(Ease.OutBounce))
                .Join(rewards[0].shineLightPS.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.3f)
                    .SetEase(Ease.OutQuad))
                .Join(rewards[0].text.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f)
                    .SetEase(Ease.OutBounce));
            
            DataSaver.Instance.CompletedLevel("rock");
        }
        else
        {
            rewards[1].shineLightPS.gameObject.SetActive(true);
            rewards[1].shineLightPS.Play();
            sequence.Append(rewards[1].star.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.3f)
                    .SetEase(Ease.OutBounce))
                .Join(rewards[1].shineLightPS.transform.DOScale(new Vector3(2.2f, 2.2f, 2.2f), 0.3f)
                    .SetEase(Ease.OutQuad))
                .Join(rewards[1].text.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f)
                    .SetEase(Ease.OutBounce));
   
            DataSaver.Instance.CompletedLevel("star");
        }

        tapText.gameObject.SetActive(true);
        OnScaletapText();
        tapBtn.gameObject.SetActive(true);
    }

    private void OnScaletapText()
    {
        Vector3 scaleTo = tapText.transform.localScale * 1.15f;
        float singleLoopDuration = 0.8f;

        tapText.transform.DOScale(scaleTo, singleLoopDuration / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
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