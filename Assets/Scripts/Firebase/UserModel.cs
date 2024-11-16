using System;
using System.Collections.Generic;
using UnityEngine;

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