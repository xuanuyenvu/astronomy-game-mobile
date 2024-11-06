using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Database;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string userID;
    private DatabaseReference dbReference;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == Firebase.DependencyStatus.Available)
            {
                userID = SystemInfo.deviceUniqueIdentifier;
                Debug.Log("Firebase Database Manager Start");
                if (FirebaseDatabase.DefaultInstance.RootReference == null)
                {
                    Debug.Log("Firebase Database Reference is null");
                }
                else
                {
                    Debug.Log("Firebase Database Reference is not null");
                }

                dbReference = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });
    }

    public void CreateUser()
    {
        UserModel newUserModel = new UserModel("Guest", 0, 1, new List<int>());
        string json = JsonUtility.ToJson(newUserModel);
        dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
    }
}