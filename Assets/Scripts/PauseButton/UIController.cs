using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public RectTransform energyUIRect;
    public RectTransform timerUIRect;
    public RectTransform healthUIRect;
    public RectTransform cardContainerUIRect;
    public RectTransform pauseTextRect;
    public GameObject gamePlayObjectsGroup;

    public GameObject settingUI;
    public GameObject gameBtnGroup;

    public TimerManager timerManager;
    public CardController cardController;
    public SettingsButtonMenu settingsButtonMenu;
    
    public Button pauseBtn;
    public Sprite pauseImg;
    public Sprite playImg;
    
    public Slider bgmSlider;
    public Slider sfxSlider;
    
    private bool isPaused = false;
    private bool isButtonCooldown = false;
    
    private Vector2 energyUIStartPos;
    private Vector2 timerUIStartPos;
    private Vector2 healthUIStartPos;
    private RectTransform pauseBtnRect;
    
    void Start()
    {
        energyUIStartPos = energyUIRect.anchoredPosition;
        timerUIStartPos = timerUIRect.anchoredPosition;
        healthUIStartPos = healthUIRect.anchoredPosition;
        StartGame();
        SetUpVolume();
    }
    
    private void SetUpVolume()
    {
        bgmSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
    }

    private void StartGame()
    {
        Vector2 pauseBtnStartPos = MoveUIOffScreen();
        MoveUIBackToScreenWithDotween(pauseBtnStartPos);
    }

    private Vector2 MoveUIOffScreen()
    {
        energyUIRect.anchoredPosition = new Vector2(energyUIStartPos.x, -energyUIStartPos.y);
        timerUIRect.anchoredPosition = new Vector2(timerUIStartPos.x, -timerUIStartPos.y);
        healthUIRect.anchoredPosition = new Vector2(healthUIStartPos.x, -healthUIStartPos.y);
        
        pauseBtnRect = pauseBtn.GetComponent<RectTransform>();
        Vector2 pauseBtnStartPos = pauseBtnRect.anchoredPosition;
        pauseBtnRect.anchoredPosition = new Vector2(pauseBtnStartPos.x, -pauseBtnStartPos.y);
        
        return pauseBtnStartPos;
    }

    private void MoveUIBackToScreenWithDotween(Vector2 pauseBtnStartPos)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(energyUIRect.DOAnchorPos(energyUIStartPos, 0.25f).SetEase(Ease.InOutQuad))
            .Join(timerUIRect.DOAnchorPos(timerUIStartPos, 0.25f).SetEase(Ease.InOutQuad))
            .Join(healthUIRect.DOAnchorPos(healthUIStartPos, 0.25f).SetEase(Ease.InOutQuad))
            .Join(pauseBtnRect.DOAnchorPos(pauseBtnStartPos, 0.25f).SetEase(Ease.InOutQuad));
    }


    
    private void HideElements()
    {
        Vector2 offScreenHorizontalPosition = new Vector2(-Screen.width, energyUIRect.anchoredPosition.y);
        Vector2 offScreenVerticalPosition  = new Vector2(0, -Screen.height);
        
        gamePlayObjectsGroup.transform.position = new Vector3(0, 0, 10);
        
        Sequence sequence = DOTween.Sequence();
        sequence.Append(energyUIRect.DOAnchorPos(offScreenHorizontalPosition, 0.6f).SetEase(Ease.InOutQuad))
            .Join(timerUIRect.DOAnchorPos(offScreenHorizontalPosition, 0.6f).SetEase(Ease.InOutQuad))
            .Join(healthUIRect.DOAnchorPos(offScreenHorizontalPosition, 0.6f).SetEase(Ease.InOutQuad))
            .Join(cardContainerUIRect.DOAnchorPos(offScreenVerticalPosition, 0.6f).SetEase(Ease.InOutQuad))
            .Join(pauseTextRect.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce));
        // .Join(gamePlayObjectsGroup.transform.DOScale(Vector3.zero, 0.6f).SetEase(Ease.InOutQuad));;
    }
    
    private void ShowElements()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pauseTextRect.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad))
            .Join(energyUIRect.DOAnchorPos(energyUIStartPos, 0.6f).SetEase(Ease.InOutQuad))
            .Join(timerUIRect.DOAnchorPos(timerUIStartPos, 0.6f).SetEase(Ease.InOutQuad))
            .Join(healthUIRect.DOAnchorPos(healthUIStartPos, 0.6f).SetEase(Ease.InOutQuad))
            .Join(cardContainerUIRect.DOAnchorPos(Vector2.zero, 0.6f).SetEase(Ease.InOutQuad))
            .OnComplete(() => 
            {
                gamePlayObjectsGroup.transform.position = new Vector3(0, 0, 0);
            });
    }

    public void UpdatePauseButtonSprite()
    {
        if (isButtonCooldown) return; 
        
        AudioManager.Instance.PlaySFX("Click");
        if (!isPaused && !GameManager.Instance.CurrentGamePlay.IsResultPlaying)
        {
            isButtonCooldown = true;
            Invoke(nameof(ResetButtonCooldown), 0.6f); // Đặt lại biến sau 0.6 giây để cho phép nhấn lại
            
            pauseBtn.image.sprite = playImg;
            settingsButtonMenu.ToggleMenu(isPaused);
            isPaused = true;
        
            cardController.OnPauseButtonPressed();
            timerManager.StopTimer();
            HideElements();
        }
        else if (isPaused)
        {
            isButtonCooldown = true; 
            Invoke(nameof(ResetButtonCooldown), 0.6f); // Đặt lại biến sau 0.6 giây để cho phép nhấn lại
            
            pauseBtn.image.sprite = pauseImg;
            settingsButtonMenu.ToggleMenu(isPaused);
            isPaused = false;
        
            timerManager.StartTimer();
            ShowElements();
        }
    }

    private void ResetButtonCooldown()
    {
        isButtonCooldown = false;
    }

    public void Reset()
    {
        pauseBtn.image.sprite = pauseImg;
        settingsButtonMenu.ToggleMenu(true);
        isPaused = false;
        
        pauseTextRect.localScale = Vector3.zero;
        gamePlayObjectsGroup.transform.position = new Vector3(0, 0, 0);

        energyUIRect.anchoredPosition = energyUIStartPos;
        timerUIRect.anchoredPosition = timerUIStartPos;
        healthUIRect.anchoredPosition = healthUIStartPos;
        cardContainerUIRect.anchoredPosition = Vector2.zero;
    }
    
    public void OpenSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        pauseTextRect.gameObject.SetActive(false);
        gameBtnGroup.SetActive(false);
        settingUI.SetActive(true);
    }
    
    public void CloseSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        pauseTextRect.gameObject.SetActive(true);
        gameBtnGroup.SetActive(true);
        settingUI.SetActive(false);
    }
    
    public void MusicVolumeChange()
    {
        AudioManager.Instance.SetMusicVolume(bgmSlider.value);
    }
    
    public void SFXVolumeChange()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }
}
