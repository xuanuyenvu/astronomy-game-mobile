using System;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEditor;

[Serializable]
public class ConfigData
{
    public string username;
    public float gameVersion;
    public int star;
    public int unlockLevel;
    public List<int> levelStars;
}

public class RemoteConfigManager : MonoBehaviour
{
    public ConfigData allConfigData;
    
    
    
    private void Awake()
    {
        print("json: " + JsonUtility.ToJson(allConfigData));
        CheckRemoteConfigValues();
    }

    public Task CheckRemoteConfigValues()
    {
        Debug.Log("Fetching remote config values...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Fetch failed!");
            return;
        }
        
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError("Fetch failed!");
            return;
        }
        
        remoteConfig.ActivateAsync()
            .ContinueWithOnMainThread(
                task => {
                    Debug.Log("Remote data loaded and ready for use. Last fetch time: " + info.FetchTime); 
                    
                    string configData = remoteConfig.GetValue("all_game_data").StringValue;
                    allConfigData = JsonUtility.FromJson<ConfigData>(configData);
                    // print(remoteConfig.AllValues.Count);
                    //
                    // foreach (var item in remoteConfig.AllValues)
                    // {
                    //     print("Key: " + item.Key + " Value: " + item.Value.StringValue);
                    // }
                });
    }
}
