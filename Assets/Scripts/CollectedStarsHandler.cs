using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        numOfStars.text = DataSaver.Instance.userModel.Star.ToString();
    }
}
