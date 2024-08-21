using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelSelector : MonoBehaviour
{
    public static int selectedLevel;

    public static int unlockedLevel = 3;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Lấy đối tượng con tại vị trí i
            LevelButtonHandler child = transform.GetChild(i).GetComponent<LevelButtonHandler>();

            if (child.level <= unlockedLevel)
            {
                child.UnlockThisButton();
            }
            else
            {
                child.LockThisButton();
            }
        }
    }

    public void OpenLevel(int level)
    {
        selectedLevel = level;
        SceneManager.LoadScene("game");
    }
}
