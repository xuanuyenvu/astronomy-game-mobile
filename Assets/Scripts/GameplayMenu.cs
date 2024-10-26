using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenu : MonoBehaviour
{
    public UniversalLevelManager universalLevelManager;

    public void Retry()
    {
        universalLevelManager.Retry();
    }

    public void LoadGame()
    {
        universalLevelManager.LoadGame();
    }

    public void Exit()
    {
        Debug.Log("Exit");
        SceneManager.LoadScene("chapter1");
    }
}
