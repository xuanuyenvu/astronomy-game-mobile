using System;
using System.Collections.Generic;
using UnityEngine;

public class UserModel
{
    private string username;
    private int star;
    private int unlockLevel;
    private List<int> levelStars;

    public UserModel(string username, int star, int unlockLevel, List<int> levelStars)
    {
        this.Username = username;
        this.Star = star;
        this.UnlockLevel = unlockLevel;
        this.LevelStars = levelStars;
    }

    public string Username
    {
        get => username;
        set => username = value;
    }

    public int Star
    {
        get => star;
        set => star = value;
    }

    public int UnlockLevel
    {
        get => unlockLevel;
        set => unlockLevel = value;
    }

    public List<int> LevelStars
    {
        get => levelStars;
        set => levelStars = value;
    }
}