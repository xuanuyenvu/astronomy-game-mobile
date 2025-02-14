using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChapterUIController : MonoBehaviour
{
    public GameObject settingBg;
    public GameObject stars;
    public GameObject btn1;
    public GameObject btn2;
    public GameObject settingBtn;
    public GameObject bagBtn;
    
    public GameObject loginButton;
    public TextMeshProUGUI userName;
    public RawImage userPic;
    
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        SetUpVolume();
    }
    
    private void SetUpVolume()
    {
        bgmSlider.value = AudioManager.Instance.GetMusicVolume();
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
    }

    public void OpenSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        settingBg.SetActive(true);
        
        btn1.SetActive(false);
        btn2.SetActive(false);
        stars.SetActive(false);
        settingBtn.SetActive(false);
        bagBtn.SetActive(false);
    }
    
    public void CloseSetting()
    {
        AudioManager.Instance.PlaySFX("Click");
        settingBg.SetActive(false);
        
        btn1.SetActive(true);
        btn2.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
        bagBtn.SetActive(true);
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
    
    public void GoToPlanetInfo()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene("planetInfo");
        // TransitionManager.Instance().Transition("planetInfo", transition, 0f);
    }
}
