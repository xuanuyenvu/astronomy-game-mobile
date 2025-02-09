using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public HealthManager healthManager;
    public GameObject energyUI;
    public GameObject stageUI;
    
    public GameObject background;
    public ParticleSystem brokenStarPS;
    public CameraShake cameraShake;

    public GameObject btnGroup;

    private RectTransform starRectTransform;
    private RectTransform titleBtnGroupRectTransform;

    void Start()
    {
        brokenStarPS.Stop();
        background.SetActive(false);
        titleBtnGroupRectTransform = GetChildByName(this.gameObject, "titleBtnGroup").GetComponent<RectTransform>();
        starRectTransform = GetChildByName(energyUI, "star").GetComponent<RectTransform>();
    }

    public void StartUI()
    {
        btnGroup.SetActive(false);
        
        timeBarUI.GetComponent<TimerManager>().StopTimer();
        timeBarUI.SetActive(false);
        healthManager.SetUp(0);
        stageUI.SetActive(false);

        GetChildByName(energyUI, "border").SetActive(false);
        GetChildByName(energyUI, "background").SetActive(false);
        GetChildByName(this.gameObject, "backgroundGO").SetActive(true);

        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        starRectTransform.DOMove(targetPosition, 1f)
            .SetEase(Ease.OutQuad);

        Vector3 targetScale = starRectTransform.localScale * 3.3f;
        starRectTransform.DOScale(targetScale, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                brokenStarPS.Play();
                StartCoroutine(PlayShakeAndShowTitle());
            });
    }

    private IEnumerator PlayShakeAndShowTitle()
    {
        StartCoroutine(WaitAndDeactivateStarUI());
        yield return new WaitForSeconds(1.6f);
        ShowTitleButtonGroup();
    }

    private void ShowTitleButtonGroup()
    {
        titleBtnGroupRectTransform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBounce);
    }

    private void HideTitleButtonGroup(TweenCallback onComplete = null)
    {
        titleBtnGroupRectTransform.DOScale(Vector3.zero, 0.2f)
            .SetEase(Ease.Linear)
            .OnComplete(onComplete);
    }

    private IEnumerator WaitAndDeactivateStarUI()
    {
        brokenStarPS.Play();
        yield return new WaitForSeconds(0.8f);
        AudioManager.Instance.PlaySFX("Lose");
        cameraShake.ShakeCamera(0.3f);
        starRectTransform.gameObject.SetActive(false);
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

    public void StopUI(bool isShowTimerSlider = true)
    {
        HideTitleButtonGroup(() =>
        {
            brokenStarPS.Stop();
            if (isShowTimerSlider)
            {
                timeBarUI.SetActive(true);
            }
            if (starRectTransform != null)
            {
                starRectTransform.localScale = new Vector3(0.55f, 0.45f, 1f);
                starRectTransform.anchoredPosition = Vector2.zero;
            }
            GetChildByName(energyUI, "border").SetActive(true);
            GetChildByName(energyUI, "background").SetActive(true);
            GetChildByName(energyUI, "star").SetActive(true);
            
            stageUI.gameObject.SetActive(true);
            btnGroup.SetActive(true);
            background.SetActive(false);
        });
    }
}
