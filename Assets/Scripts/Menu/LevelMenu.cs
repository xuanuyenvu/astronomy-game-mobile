using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    public GameObject settingUI;
    
    public void BackToChapterMenu()
    {
        SceneManager.LoadScene("chapterMenu");
    }

    public void OpenSettingUI()
    {
        settingUI.SetActive(true);
    }
    
    public void CloseSettingUI()
    {
        settingUI.SetActive(false);
    }
}
