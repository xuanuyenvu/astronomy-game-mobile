using DG.Tweening;
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
        DOTween.KillAll();
        // SceneManager.LoadScene("chapter1");
    }
}
