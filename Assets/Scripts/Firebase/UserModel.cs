using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelItem
{
    public string type;
    public string state;      

    public LevelItem(string type, string state)
    {
        this.Type = type;
        this.State = state;
    }
    
    public string Type
    {
        get => type;
        set => type = value;
    }

    public string State
    {
        get => state;
        set => state = value;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is LevelItem other)
        {
            return this.Type == other.Type && this.State == other.State;
        }
        return false;
    }
}


[System.Serializable]
public class UserModel
{
    public int star;
    public List<LevelItem> levels;
    public string userId;

    public UserModel(string userId, int star, List<LevelItem> levels)
    {
        this.UserId = this.userId;
        this.Star = star;
        this.Levels = levels;
    }
    
    public int Star
    {
        get => star;
        set => star = value;
    }
    
    public List<LevelItem> Levels
    {
        get => levels;
        set => levels = value;
    }

    public string UserId
    {
        get => userId;
        set => userId = value;
    }
}

[System.Serializable]
public class FacebookUserData
{
    private string name;
    private Texture2D  profilePic;
    
    public FacebookUserData()
    {
        this.Name = null;
        this.ProfilePic = null;
    }

    public FacebookUserData(string name, Texture2D profilePic)
    {
        this.Name = name;
        this.ProfilePic = profilePic;
    }
    
    public string Name
    {
        get => name;
        set => name = value;
    }

    public Texture2D ProfilePic
    {
        get => profilePic;
        set => profilePic = value;
    }

}