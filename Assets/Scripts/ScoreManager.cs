using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float maxScore = 3000;
    private float elapsedTime = 0f;
    private float score = 0;
    private bool playing;

    [HideInInspector] public int changeResultPlanet = 0;

    public void StartGame()
    {
        ResetValue();
        playing = true;
    }
    void Update()
    {
        if (playing)
        {
            elapsedTime += Time.deltaTime;
        }
        if (elapsedTime > 10f && changeResultPlanet == 0)
        {
            changeResultPlanet = 1;
        }
    }

    public void FinalScore(int health)
    {
        score = (maxScore / (elapsedTime * 0.3f)) + (float)(health * 60);
        playing = false;
        Debug.Log("score: " + Mathf.Round(score));
        ResetValue();
    }

    private void ResetValue()
    {
        score = 0f;
        elapsedTime = 0f;
    }
}
