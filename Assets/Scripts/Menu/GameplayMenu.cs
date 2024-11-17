using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayMenu : MonoBehaviour
{
    public UniversalLevelManager universalLevelManager;

    public void Retry()
    {
        AudioManager.Instance.PlaySFX("Click");
        universalLevelManager.Retry();
    }

    public void LoadGame()
    {
        AudioManager.Instance.PlaySFX("Click");
        universalLevelManager.LoadGame();
    }

    public void Exit()
    {
        AudioManager.Instance.PlaySFX("Click");
        Debug.Log("Exit");
        DOTween.KillAll();
        SceneManager.LoadScene("chapter1");
    }
}
