using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using EasyTransition;
using Unity.VisualScripting;
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
    public GameObject bagBtn;
    
    public ParticleSystem psPrefab;
    public Canvas uiCanvas;
    
    public TextMeshProUGUI tapText;
    public Button tapBtn;
    public Button loginButton;
    public TextMeshProUGUI userName;
    public RawImage userPic;
    
    public Slider bgmSlider;
    public Slider sfxSlider;

    public TransitionSettings backBtnTransition;
    public TransitionSettings bagBtnTransition;
    
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
            case 14:
                ShowCard("uranus");
                break;
            case 24:
                ShowCard("saturn");
                break;
            case 26:
                ShowCard("jupiter");
                break;
        }
    }
    
    private void ShowCard(string name)
    {
        AudioManager.Instance.PlaySFX("NewCard");
        
        newCardBg.SetActive(true);
        primaryBg.SetActive(false);
        
        scrollView.SetActive(false);
        backBtn.SetActive(false);
        stars.SetActive(false);
        settingBtn.SetActive(false);
        bagBtn.SetActive(false);
        
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
        AddCardToBag();
        // HideCard();
        DataSaver.Instance.OpenedCard(indexCard);
        levelSelector.UpdateAfterOpenCard(indexCard);
    }
    
    private void AddCardToBag()
    {
        bagBtn.SetActive(true);

        // Tạo hiệu ứng ParticleSystem trong UI
        ParticleSystem particle = Instantiate(psPrefab, uiCanvas.transform);
        RectTransform particleRect = particle.GetComponent<RectTransform>();
        
        particleRect.localScale = new Vector3(3f, 3f, 3f);
        RectTransform bagRect = bagBtn.GetComponent<RectTransform>();

        // Đặt vị trí ban đầu cho ParticleSystem
        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Vector2 uiCenter;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            uiCanvas.GetComponent<RectTransform>(), screenCenter, null, out uiCenter
        );
        particleRect.anchoredPosition = uiCenter;


        // Định nghĩa đường đi của ParticleSystem
        Vector3[] path = new Vector3[]
        {
            particleRect.position, // Bắt đầu từ vị trí của thẻ
            (particleRect.position + bagRect.position) / 2 + Vector3.down * 90, // Điểm giữa, lệch xuống một chút
            bagRect.position // Kết thúc tại Bag Button
        };

        // Xóa thẻ, chạy particle
        DestroyCard();
        particle.Play();

        DOVirtual.DelayedCall(0.2f, BackToLevelScene);
        
        // Di chuyển ParticleSystem theo path
        particleRect.DOPath(path, 0.6f, PathType.CatmullRom)
            .SetEase(Ease.InOutQuint)
            .OnComplete(() =>
            {
                particle.Stop();
                Destroy(particle.gameObject, particle.main.duration);
                bagBtn.transform.DOScale(1.2f, 0.1f)
                    .SetLoops(2, LoopType.Yoyo);
                // .OnComplete(() =>
                // {
                //     BackToLevelScene();
                // });
            });
    }

    private void DestroyCard()
    {
        tapText.gameObject.SetActive(false);
        tapBtn.gameObject.SetActive(false);
    
        if (currentCard != null)
        {
            Destroy(currentCard.gameObject);
        }
    }

    
    private void BackToLevelScene()
    {
        primaryBg.SetActive(true);
        newCardBg.SetActive(false);
        scrollView.SetActive(true);
        backBtn.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
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
        backBtn.SetActive(false);
        bagBtn.SetActive(false);
    }
    
    public void CloseSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        settingBg.SetActive(false);
        
        scrollView.SetActive(true);
        backBtn.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
        backBtn.SetActive(true);
        bagBtn.SetActive(true);
    }
    
    public void BackToChapterMenu()
    {
        AudioManager.Instance.PlaySFX("Click");
        // SceneManager.LoadScene("chapterMenu");
        TransitionManager.Instance().Transition("chapterMenu", backBtnTransition, 0f);
    }
    
    public void GoToPlanetInfo(string sceneName)
    {
        AudioManager.Instance.PlaySFX("Click");
        // SceneManager.LoadScene("chapterMenu");
        DataSaver.Instance.sceneName = sceneName;
        TransitionManager.Instance().Transition("planetInfo", bagBtnTransition, 0f);
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
