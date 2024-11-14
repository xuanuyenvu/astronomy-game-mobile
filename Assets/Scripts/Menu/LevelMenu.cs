using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public void BackToChapterMenu()
    {
        SceneManager.LoadScene("chapterMenu");
    }
}
