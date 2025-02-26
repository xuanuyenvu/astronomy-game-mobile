using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using EasyTransition;

public class GameplayMenu : MonoBehaviour
{
    public UniversalLevelManager universalLevelManager;
    public TransitionSettings exitTransition;

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
        // SceneManager.LoadScene("chapter1");
        TransitionManager.Instance().Transition(DataSaver.Instance.sceneName, exitTransition, 0f);
    }
}
