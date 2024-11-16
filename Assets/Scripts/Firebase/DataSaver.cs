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
    public FacebookUserData facebookUserData;
    public int selectedLevel;
    public int selectedLevelIndex;
    
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
        userModel.Levels = new List<LevelItem>();

        for (int i = 0; i < 24; i++)
        {
            // Mặc định tất cả đều bị khóa
            userModel.Levels.Add(new LevelItem("game", "locked"));
        }

        // Cập nhật các vị tr lưu card
        userModel.Levels[4] = new LevelItem("card", "locked");
        userModel.Levels[8] = new LevelItem("card", "locked");
        userModel.Levels[13] = new LevelItem("card", "locked");
        userModel.Levels[18] = new LevelItem("card", "locked");
        userModel.Levels[21] = new LevelItem("card", "locked");

        // Unlock level đầu tiên
        userModel.Levels[0] = new LevelItem("game", "unlocked");
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
    
    public void CompletedLevel(string type)
    {
        int gameIndex = selectedLevelIndex;
        
        if (userModel.Levels[gameIndex].Type == "game")
        {
            if (type == "rock")
            {
                userModel.Levels[gameIndex].State = "rock";
            }
            else if (type == "star")
            {
                userModel.Levels[gameIndex].State = "star";
                userModel.Star += 1;
            }
            
            SetCurrentLevel(gameIndex + 1);
            SaveDataFn();
        }
    }

    public void OpenedCard(int cardIndex)
    {
        if (userModel.Levels[cardIndex].Type == "card")
        {
            userModel.Levels[cardIndex].State = "opened";
            
            SetCurrentLevel(cardIndex + 1);
            SaveDataFn();
        }
    }
    
    private void SetCurrentLevel(int levelIndex)
    {
        if (levelIndex >= userModel.Levels.Count)
        {
            return;
        }
        
        userModel.Levels[levelIndex].State = "unlocked";
    }

}