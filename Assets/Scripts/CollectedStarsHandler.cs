using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CollectedStarsHandler : MonoBehaviour
{
    public TextMeshProUGUI numOfStars;
    private int count = 0;

    private void Start()
    {
        LoadText();
    }

    private void Update()
    {
        int numOfStarsValue;
        if (count == 0 && int.TryParse(numOfStars.text, out numOfStarsValue))
        {
            if (DataSaver.Instance.userModel.Star != numOfStarsValue)
            {
                LoadText();
                count++;
            }
        }
    }

    private void LoadText()
    {
        int k = 0;
        numOfStars.text = DataSaver.Instance.userModel.Star.ToString();

        foreach (LevelItem el in DataSaver.Instance.userModel.Levels) 
        {
            if (el.State == "star") 
            {
                k++;
            }
        }
        Debug.Log("k " + k);
        DataSaver.Instance.userModel.Star = k;
        numOfStars.text = DataSaver.Instance.userModel.Star.ToString();
        DataSaver.Instance.SaveDataFn();
    }

}
