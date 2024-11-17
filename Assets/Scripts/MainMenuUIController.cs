using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public Button loginButton;
    
    private void Start()
    {
        if (FacebookManager.Instance.IsFacebookUserLoggedIn())
        {
            HideLoginButton();
        }
    }
    
    private void HideLoginButton()
    {
        if (loginButton != null)
        {
            loginButton.gameObject.SetActive(false);
        }
    }
}
