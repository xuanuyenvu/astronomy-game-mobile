using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UserModel
{
    public int star;
    public List<int> levelStars;
    public string userId;

    public UserModel(string userId, int star, List<int> levelStars)
    {
        this.UserId = this.userId;
        this.Star = star;
        this.LevelStars = levelStars;
    }
    
    public int Star
    {
        get => star;
        set => star = value;
    }
    
    public List<int> LevelStars
    {
        get => levelStars;
        set => levelStars = value;
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