using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public GameObject healthContainerUI;
    public GameObject energyUI;
    public GameObject background;
    public ParticleSystem brokenStarPS;
    public CameraShake cameraShake;

    private RectTransform starRectTransform;
    private RectTransform titleBtnGroupRectTransform;

    void Awake()
    {
        brokenStarPS.Stop();
        GetChildByName(this.gameObject, "backgroundGO").SetActive(false);
        titleBtnGroupRectTransform = GetChildByName(this.gameObject, "titleBtnGroup").GetComponent<RectTransform>();
        titleBtnGroupRectTransform.localScale = Vector3.zero;
    }

    public void StartUI()
    {
        timeBarUI.GetComponent<TimerManager>().StopTimer();
        timeBarUI.SetActive(false);
        healthContainerUI.SetActive(false);

        InitializeEnergyUIChildren();
        GetChildByName(this.gameObject, "backgroundGO").SetActive(true);

        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        starRectTransform.DOMove(targetPosition, 1f)
            .SetEase(Ease.OutQuad);

        Vector3 targetScale = new Vector3(3f, 3f, 1);
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
        titleBtnGroupRectTransform.DOScale(Vector3.one, 0.6f)
            .SetEase(Ease.OutBounce);
    }

    private IEnumerator WaitAndDeactivateStarUI()
    {
        brokenStarPS.Play();
        yield return new WaitForSeconds(0.7f);
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

    public void InitializeEnergyUIChildren()
    {
        GameObject energyStar = GetChildByName(energyUI, "star");
        GameObject energyBorder = GetChildByName(energyUI, "border");
        GameObject energyBackground = GetChildByName(energyUI, "background");

        if (energyStar != null) starRectTransform = energyStar.GetComponent<RectTransform>();
        if (energyBorder != null) energyBorder.SetActive(false);
        if (energyBackground != null) energyBackground.SetActive(false);
    }
}
