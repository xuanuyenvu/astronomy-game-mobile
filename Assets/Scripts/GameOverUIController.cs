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

    private RectTransform starRectTransform;

    void Awake()
    {
        brokenStarPS.Stop();
        background.SetActive(false);
    }

    public void StartUI()
    {
        timeBarUI.SetActive(false);
        healthContainerUI.SetActive(false);

        InitializeEnergyUIChildren();
        
        background.SetActive(true);

        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        starRectTransform.DOMove(targetPosition, 1f)
            .SetEase(Ease.OutQuad);

        Vector3 targetScale = new Vector3(2.5f, 2.5f, 1);
        starRectTransform.DOScale(targetScale, 1f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                brokenStarPS.Play();
                StartCoroutine(WaitAndDeactivatestarUI());
            });
    }

    private IEnumerator WaitAndDeactivatestarUI()
    {
        yield return new WaitForSeconds(0.7f);
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
        GameObject energyFill = GetChildByName(energyUI, "fill");
        GameObject energyBorder = GetChildByName(energyUI, "border");
        GameObject energyBackground = GetChildByName(energyUI, "background");

        if (energyStar != null) starRectTransform = energyStar.GetComponent<RectTransform>();
        if (energyFill != null) energyFill.SetActive(false);
        if (energyBorder != null) energyBorder.SetActive(false);
        if (energyBackground != null) energyBackground.SetActive(false);
    }
}