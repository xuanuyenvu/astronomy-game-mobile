using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [HideInInspector]
    public int health = 5;
    public Sprite heart;
    public List<Image> hearts;
    void Update()
    {
        foreach (var heart in hearts)
        {
            heart.enabled = false;
        }
        for (int i = 0; i < health; i++)
        {
            hearts[i].enabled = true;
        }
    }
}
