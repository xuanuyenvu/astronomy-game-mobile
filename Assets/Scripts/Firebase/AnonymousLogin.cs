using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnonymousLogin : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        // auth.SignOut();
    }
    
    public void SignOut()
    {
        auth.SignOut();
    }
    
    public async void Login()
    {
        if (auth.CurrentUser != null)
        {
            DataSaver.Instance.SetUserId(auth.CurrentUser.UserId);
            DataSaver.Instance.LoadDataFn();
            SceneManager.LoadScene("chapterMenu");
        }
        else
        {
            await AnonymousLoginFn().ContinueWithOnMainThread(task => SceneManager.LoadScene("chapterMenu"));
        }
    }

    async Task AnonymousLoginFn()
    {
        await auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            AuthResult result = task.Result;

            DataSaver.Instance.SetUserId(result.User.UserId);
            DataSaver.Instance.InitNewUser();
            DataSaver.Instance.SaveDataFn();
        });
    }
}