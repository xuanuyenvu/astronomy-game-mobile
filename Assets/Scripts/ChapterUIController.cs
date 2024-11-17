using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChapterUIController : MonoBehaviour
{
    public GameObject settingBg;
    public GameObject stars;
    public GameObject btn1;
    public GameObject btn2;
    public GameObject settingBtn;
    
    public GameObject loginButton;
    public TextMeshProUGUI userName;
    public RawImage userPic;
    
    public void OpenSetting()
    {
        settingBg.SetActive(true);
        
        btn1.SetActive(false);
        btn2.SetActive(false);
        stars.SetActive(false);
        settingBtn.SetActive(false);
    }
    
    public void CloseSetting()
    {
        settingBg.SetActive(false);
        
        btn1.SetActive(true);
        btn2.SetActive(true);
        stars.SetActive(true);
        settingBtn.SetActive(true);
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
}
