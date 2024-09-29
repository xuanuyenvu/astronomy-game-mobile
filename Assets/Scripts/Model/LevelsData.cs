using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public int stage;
    public int[] elements;
}

[System.Serializable]
public class Level
{
    public int level;
    public int total_stages;
    public Stage[] stages;
    public int lives;
    public string total_time;
    public int cards_displayed;
}

[System.Serializable]
public class LevelsData
{
    public Level[] levels;
}