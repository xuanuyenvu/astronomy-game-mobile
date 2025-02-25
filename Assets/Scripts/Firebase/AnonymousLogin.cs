using System.Threading.Tasks;
using EasyTransition;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class AnonymousLogin : MonoBehaviour
{
    private Firebase.Auth.FirebaseAuth auth;
    public TransitionSettings transition;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        // SignOut();
    }
    
    public void SignOut()
    {
        Debug.Log("SignOut!");
        auth.SignOut();
    }
    
    public async void Login()
    {
        AudioManager.Instance.PlaySFX("Click");
        if (auth.CurrentUser != null)
        {
            DataSaver.Instance.SetUserId(auth.CurrentUser.UserId); 
            Debug.Log("DataSaver Instance Start" + auth.CurrentUser.UserId);
            DataSaver.Instance.LoadDataFn();
            
            if (TransitionManager.Instance() == null)
            {
                Debug.LogError("TransitionManager is NULL!");
                return;
            }
            TransitionManager.Instance().Transition("chapterMenu", transition, 0f);
        }
        else
        {
            // await AnonymousLoginFn().ContinueWithOnMainThread(task => SceneManager.LoadScene("chapterMenu"));
            await AnonymousLoginFn().ContinueWithOnMainThread(task =>  TransitionManager.Instance().Transition("chapterMenu", transition, 0f));
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