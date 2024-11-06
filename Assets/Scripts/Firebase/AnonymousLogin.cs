using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine;

public class AnonymousLogin : MonoBehaviour
{
    public async void Login()
    {
        // Kiểm tra xem userId đã được lưu trong PlayerPrefs chưa
        if (PlayerPrefs.HasKey("userId"))
        {
            Debug.Log("User is already logged in with ID: " + PlayerPrefs.GetString("userId"));
            return;
        }

        // Nếu chưa đăng nhập, thực hiện đăng nhập ẩn danh
        await AnonymousLoginBtn();
    }

    async Task AnonymousLoginBtn()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
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

            Debug.Log("Login successful");
            AuthResult result = task.Result;
            Debug.Log("Guest name: " + result.User.DisplayName);
            Debug.Log("Guest id: " + result.User.UserId);

            // Lưu userId vào PlayerPrefs sau khi đăng nhập thành công
            GuestLoginSuccess(result.User.UserId);
        });
    }
    
    void GuestLoginSuccess(string userId)
    {
        // Lưu userId vào PlayerPrefs
        PlayerPrefs.SetString("userId", userId);
        PlayerPrefs.Save(); // Lưu PlayerPrefs để đảm bảo dữ liệu được ghi
        Debug.Log("User ID saved to PlayerPrefs: " + userId);
    }
}