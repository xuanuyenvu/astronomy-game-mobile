using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinGameUIController : MonoBehaviour
{
    public GameObject timeBarUI;
    public HealthManager healthManager;
    public GameObject energyUI;
    public GameObject background;
    public GameObject starSprite;
    public CameraShake cameraShake;
    public Canvas uiCanvas;

    private EnergyManager energyManager;
    private TimerManager timerManager;
    private RectTransform starRectTransform;

    void Awake()
    {
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
        timeBarUI.GetComponent<TimerManager>().StopTimer();
        timeBarUI.SetActive(false);
        healthManager.SetUp(0);

        GetChildByName(energyUI, "border").SetActive(false);
        GetChildByName(energyUI, "background").SetActive(false);
        // GetChildByName(this.gameObject, "backgroundWin").SetActive(true);

        Vector3 targetPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        starRectTransform.DOMove(targetPosition, 1f)
            .SetEase(Ease.OutQuad);

        Vector3 targetScale = starRectTransform.localScale * 3.3f;
        starRectTransform.DOScale(targetScale, 1f) // Thêm yield return để đảm bảo chờ cho DOScale hoàn thành
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Chuyển đổi vị trí từ UI sang World Space
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(starRectTransform.position);
                worldPosition.z = 0;

                // Tính toán tỷ lệ tương ứng để star trong World Space bằng với star trong UI
                float canvasScaleFactor = uiCanvas.scaleFactor;
                float screenToWorldRatio = Camera.main.orthographicSize * 2 / Screen.height;
                Vector3 worldScale = targetScale * screenToWorldRatio / canvasScaleFactor;

                // Đặt vị trí và tỷ lệ cho starSprite
                starSprite.transform.position = worldPosition;
                starSprite.transform.localScale = worldScale;
                starSprite.SetActive(true);
            }).WaitForCompletion(); // Đảm bảo chuỗi hoàn thành

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
