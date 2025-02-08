using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using EasyTransition;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
    public CameraShake cameraShake;
    public LevelSelector levelSelector;

    public GameObject primaryBg;
    public GameObject newCardBg;
    public GameObject settingBg;
    
    public GameObject scrollView;
    public GameObject backBtn;
    public GameObject stars;
    public GameObject settingBtn;
    
    public TextMeshProUGUI tapText;
    public Button tapBtn;
    public Button loginButton;
    public TextMeshProUGUI userName;
    public RawImage userPic;
    
    public Slider bgmSlider;
    public Slider sfxSlider;

    public TransitionSettings transition;
    
    public List<Card> cardPrefabs;
    private Card currentCard;
    private int indexCard;

    private void Start()
    {
        newCardBg.SetActive(false);
        settingBg.SetActive(false);
        
        tapText.gameObject.SetActive(false);
        tapBtn.gameObject.SetActive(false);
        
        HideLoginButton();
        
        AudioManager.Instance.PlayMusic("In Menu");
        SetUpVolume();
    }
    
    private void SetUpVolume()
    {
        bgmSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
    }
    
    public void OpenCard(int index)
    {
        AudioManager.Instance.PlaySFX("Click");
        indexCard = index;
        switch (indexCard)
        {
            case 1:
                ShowCard("mercury");
                break;
            case 3:
                ShowCard("venus");
                break;
            case 6:
                ShowCard("mars");
                break;
            case 10:
                ShowCard("neptune");
                break;
            case 15:
                ShowCard("uranus");
                break;
            case 20:
                ShowCard("saturn");
                break;
            case 23:
                ShowCard("jupiter");
                break;
        }
    }
    
    private void ShowCard(string name)
    {
        newCardBg.SetActive(true);
        primaryBg.SetActive(false);
        
        scrollView.SetActive(false);
        backBtn.SetActive(false);
        stars.SetActive(false);
        settingBtn.SetActive(false);
        
        Card card = cardPrefabs.Find(c => c.name == name);
        if (card != null)
        {
            currentCard = Instantiate(card, Vector3.zero, Quaternion.identity);
            
            cameraShake.ShakeCamera(0.12f);
            currentCard.StartAnimation();
            
            tapText.gameObject.SetActive(true);
            OnScaletapText();
            tapBtn.gameObject.SetActive(true);
        }
    }

    public void ReceiveCard()
    {
        AudioManager.Instance.PlaySFX("Click");
        HideCard();
        DataSaver.Instance.OpenedCard(indexCard);
        levelSelector.UpdateAfterOpenCard(indexCard);
    }
    
    private void HideCard()
    {
        tapText.gameObject.SetActive(false);
        tapBtn.gameObject.SetActive(false);
        
        primaryBg.SetActive(true);
        newCardBg.SetActive(false);
        scrollView.SetActive(true);
        backBtn.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
        
        Destroy(currentCard.gameObject);
    }
    
    private void OnScaletapText()
    {
        Vector3 scaleTo = tapText.transform.localScale * 1.15f;
        float singleLoopDuration = 0.8f;

        tapText.transform.DOScale(scaleTo, singleLoopDuration / 2)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void OpenSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        settingBg.SetActive(true);
        
        scrollView.SetActive(false);
        backBtn.SetActive(false);
        stars.SetActive(false);
        settingBtn.SetActive(false);
    }
    
    public void CloseSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        settingBg.SetActive(false);
        
        scrollView.SetActive(true);
        backBtn.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
    }
    
    public void BackToChapterMenu()
    {
        AudioManager.Instance.PlaySFX("Click");
        // SceneManager.LoadScene("chapterMenu");
        TransitionManager.Instance().Transition("chapterMenu", transition, 0f);
    }

    public void LoginFB()
    {
        FacebookManager.Instance.Facebook_LogIn();
    }
    
    private void HideLoginButton()
    {
        if (FacebookManager.Instance.IsFacebookUserLoggedIn())
        {
            loginButton.gameObject.SetActive(false);

            userName.text = DataSaver.Instance.facebookUserData.Name;
            userPic.texture = DataSaver.Instance.facebookUserData.ProfilePic;
            userName.gameObject.SetActive(true);
        }
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
