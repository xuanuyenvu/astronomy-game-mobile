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
    public Image star;

    public void SetUp(int _health)
    {
        health = _health;
    }
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

    // public void AddToEngergy()
    // {
    //     health--;
    //     Vector3[] path = new Vector3[]
    //     {
    //         hearts[health].transform.position, 
    //         (hearts[health].transform.position + star.transform.position) / 2 + (Vector3.up * 10), 
    //         star.transform.position
    //     };
    //     hearts[health - 1].DOPath(path, 1f, PathType.CatmullRom);
    // }
}
