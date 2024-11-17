using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Facebook.Unity;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FacebookManager : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;
    public static FacebookManager Instance { get; private set; }

    #region Initialize
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject); 
        
        FB.Init(SetInit, FbLoginSuccess);

        if (!FB.IsInitialized)
        {
            FB.Init(() =>
                {
                    if (FB.IsInitialized)
                        FB.ActivateApp();
                    else
                        Debug.Log("Couldn't initialize");
                },
                isGameShown =>
                {
                    if (!isGameShown)
                        Time.timeScale = 0;
                    else
                        Time.timeScale = 1;
                });
        }
        else
            FB.ActivateApp();
    }

    private void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public bool IsFacebookUserLoggedIn()
    {
        if (auth.CurrentUser != null && !auth.CurrentUser.IsAnonymous)
        {
            return true;
        }

        return false;
    }

    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is Login!");
            string s = "client token" + FB.ClientToken + "User Id" + AccessToken.CurrentAccessToken.UserId +
                       "token string" + AccessToken.CurrentAccessToken.TokenString;
        }
        else
        {
            Debug.Log("Facebook is not Logged in!");
        }

        DealWithFbMenus(FB.IsLoggedIn);
    }

    void FbLoginSuccess(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            DataSaver.Instance.facebookUserData = new FacebookUserData();
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
        }
        else
        {
            Debug.Log("Not logged in");
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            string name = "" + result.ResultDictionary["first_name"];
            DataSaver.Instance.facebookUserData.Name = name;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            // Debug.Log("Profile Pic");
            // rawImg.gameObject.SetActive(true);
            // rawImg.texture = result.Texture;
            
            DataSaver.Instance.facebookUserData.ProfilePic = result.Texture;
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    #endregion


    //login
    public void Facebook_LogIn()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }

    void AuthCallBack(IResult result)
    {
        if (FB.IsLoggedIn)
        {
            SetInit();
            //AccessToken class will have session details
            var aToken = AccessToken.CurrentAccessToken;
       
            if (auth.CurrentUser != null && auth.CurrentUser.IsAnonymous)
            {
                LinkAnonymousAccountWithFacebook(aToken.TokenString);
            }
            else if (auth.CurrentUser == null)
            {
                SignInWithFacebook(aToken.TokenString);
            }
        }
        else
        {
            Debug.Log("Failed to log in");
        }
    }

    public void LinkAnonymousAccountWithFacebook(string facebookAccessToken)
    {
        var user = auth.CurrentUser;
        Credential credential = FacebookAuthProvider.GetCredential(facebookAccessToken);

        user.LinkWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted)
            {
                Debug.Log("Successfully linked anonymous account with Facebook.");
                DataSaver.Instance.SetUserId(user.UserId);
                DataSaver.Instance.LoadDataFn();
                SceneManager.LoadScene("chapterMenu");
            }
            else if (task.IsFaulted)
            {
                var innerException = task.Exception?.Flatten().InnerExceptions[0];
                if (innerException is Firebase.Auth.FirebaseAccountLinkException linkException)
                {
                    Debug.Log("Account linking failed: This credential is already associated with a different user account.");
                    SignInWithFacebook(facebookAccessToken);
                }
                else
                {
                    Debug.LogError("Task failed with exception: " + task.Exception?.ToString());
                }
            }
        });
    }

    private void SignInWithFacebook(string accessToken)
    {
        Firebase.Auth.Credential credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
        auth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.AuthResult result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);

            DataSaver.Instance.SetUserId(result.User.UserId);
            DataSaver.Instance.CheckIfFacebookUserExists();
            SceneManager.LoadScene("chapterMenu");
        });
    }

    //logout
    public void Facebook_LogOut()
    {
        auth.SignOut();
        StartCoroutine(LogOut());
    }

    IEnumerator LogOut()
    {
        FB.LogOut();
        while (FB.IsLoggedIn)
        {
            Debug.Log("Logging Out");
            yield return null;
        }

        Debug.Log("Logout Successful");
    }


    #region other

    public void FacebookSharefeed()
    {
        string url = "https:developers.facebook.com/docs/unity/reference/current/FB.ShareLink";
        FB.ShareLink(
            new Uri(url),
            "Checkout COCO 3D channel",
            "I just watched " + "22" + " times of this channel",
            null,
            ShareCallback);
    }

    private static void ShareCallback(IShareResult result)
    {
        Debug.Log("ShareCallback");
        SpentCoins(2, "sharelink");
        if (result.Error != null)
        {
            Debug.LogError(result.Error);
            return;
        }

        Debug.Log(result.RawResult);
    }

    public static void SpentCoins(int coins, string item)
    {
        var param = new Dictionary<string, object>();
        param[AppEventParameterName.ContentID] = item;
        FB.LogAppEvent(AppEventName.SpentCredits, (float)coins, param);
    }

    /*public void GetFriendsPlayingThisGame()
    {
        string query = "/me/friends";
        FB.API(query, HttpMethod.GET, result =>
        {
            Debug.Log("the raw" + result.RawResult);
            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
            var friendsList = (List<object>)dictionary["data"];

            foreach (var dict in friendsList)
            {
                GameObject go = Instantiate(friendstxtprefab);
                go.GetComponent<Text>().text = ((Dictionary<string, object>)dict)["name"].ToString();
                go.transform.SetParent(GetFriendsPos.transform, false);
                FriendsText[1].text += ((Dictionary<string, object>)dict)["name"];
            }
        });
    }*/

    #endregion
}