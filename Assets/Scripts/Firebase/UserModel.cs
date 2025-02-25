using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    public override int GetHashCode()
    {
        unchecked 
        {
            int hash = 17;
            hash = hash * 23 + (Type != null ? Type.GetHashCode() : 0);  
            hash = hash * 23 + (State != null ? State.GetHashCode() : 0);  
            return hash;
        }
    }
}

[System.Serializable]
public class ChapterItem
{
    public int id;
    public string state;      

    public ChapterItem(int id, string state)
    {
        this.Id = id;
        this.State = state;
    }
    
    public int Id  
    {
        get => id;
        set => id = value;
    }

    public string State
    {
        get => state;
        set => state = value;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is ChapterItem other)
        {
            return this.Id == other.Id && this.State == other.State;
        }
        return false;
    }

    public override int GetHashCode()
    {
        unchecked 
        {
            int hash = 17;
            hash = hash * 23 + Id.GetHashCode();  
            hash = hash * 23 + (State != null ? State.GetHashCode() : 0);  
            return hash;
        }
    }
} 

[System.Serializable]
public class UserModel
{
    public int star;
    public int card;
    public List<LevelItem> levels;
    public List<ChapterItem> chapters;
    public string userId;

    public UserModel(string userId, int star, int card, List<LevelItem> levels, List<ChapterItem> chapters)
    {
        this.userId = userId; 
        this.Star = star;
        this.Card = card;
        this.Levels = levels;
        this.Chapters = chapters;
    }
    
    public int Star
    {
        get => star;
        set => star = value;
    }
    
    public int Card
    {
        get => card;
        set => card = value;
    }
    
    public List<LevelItem> Levels
    {
        get => levels;
        set => levels = value;
    }
    
    public List<ChapterItem> Chapters
    {
        get => chapters;
        set => chapters = value;
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
    private Texture2D profilePic;
    
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