using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class DataSaver : MonoBehaviour
{
    public static DataSaver Instance { get; private set; }
    public UserModel userModel;

    private DatabaseReference dbRef;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SetUserId(string userId)
    {
        userModel.UserId = userId;
    }

    public void InitNewUser()
    {
        userModel.Star = 0;

        userModel.LevelStars = new List<int>();
        for (int i = 0; i < 24; i++)
        {
            userModel.LevelStars.Add(1);
        }
    }

    public void SaveDataFn()
    {
        string json = JsonUtility.ToJson(userModel);
        dbRef.Child("users").Child(userModel.UserId).SetRawJsonValueAsync(json);
    }

    public async void LoadDataFn()
    {
        DataSnapshot snapshot = await dbRef.Child("users").Child(userModel.UserId).GetValueAsync();

        if (snapshot.Exists)
        {
            string jsonData = snapshot.GetRawJsonValue();
            userModel = JsonUtility.FromJson<UserModel>(jsonData);
        }
    }

    public async void CheckIfFacebookUserExists()
    {
        DataSnapshot snapshot = await dbRef.Child("users").Child(userModel.UserId).GetValueAsync();

        if (!snapshot.Exists)
        {
            InitNewUser();
            SaveDataFn();
        }
        else
        {
            LoadDataFn();
        }
    }
}