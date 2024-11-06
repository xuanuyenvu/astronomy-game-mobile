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
    public GameObject gamePlayObjectsGroup;

    public RectTransform pauseTextRect;
    public TimerManager timerManager;
    
    public Button pauseBtn;
    public Sprite pauseImg;
    public Sprite playImg;
    
    private bool isPaused = false;
    private Vector2 energyUIStartPos;
    private Vector2 timerUIStartPos;
    private Vector2 healthUIStartPos;
    
    void Start()
    {
        energyUIStartPos = energyUIRect.anchoredPosition;
        timerUIStartPos = timerUIRect.anchoredPosition;
        healthUIStartPos = healthUIRect.anchoredPosition;
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

    public void UpdatePauseButtonSprite ()
    {
        if (!isPaused)
        {
            pauseBtn.image.sprite = playImg;
            isPaused = true;
            
            timerManager.StopTimer();
            HideElements();
        }
        else
        {
            pauseBtn.image.sprite = pauseImg;
            isPaused = false;
            
            timerManager.StartTimer();
            ShowElements();
        }
    }

}
