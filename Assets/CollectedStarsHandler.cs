using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollectedStarsHandler : MonoBehaviour
{
    public TextMeshProUGUI numOfStars;

    private void Start()
    {
        LoadText();
    }
    
    private void LoadText()
    {
        numOfStars.text = DataSaver.Instance.userModel.Star.ToString();
    }
}
