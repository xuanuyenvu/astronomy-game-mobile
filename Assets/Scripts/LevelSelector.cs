using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelSelector : MonoBehaviour
{
    public static int selectedLevel;
    public RectTransform contentRectTransform; 

    private List<int> levelDatabase = new List<int> 
    { 
        4, 4, 3, 4, 3, 3, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 
    };
    [SerializeField] private int currentLevel = 1; 

    void Start()
    {
        InitializeLevelButtons();
    }

    private void InitializeLevelButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Lấy đối tượng con tại vị trí i
            LevelButtonHandler child = transform.GetChild(i).GetComponent<LevelButtonHandler>();
            child.UpdateState(levelDatabase[i]);
            
            if (levelDatabase[i] == 2)
            {
                currentLevel = i + 1;
            }
        }

        ScrollToCurrentLevel();
    }
    
    // private void OnValidate()
    // {
    //     ScrollToCurrentLevel(); // Cập nhật giao diện khi giá trị state thay đổi từ Inspector
    // }

    private void ScrollToCurrentLevel()
    {
        if (currentLevel >= 4 && currentLevel <= transform.childCount - 2)
        {
            float offset = 810 + (500 * (currentLevel - 4));
            contentRectTransform.localPosition  = new Vector3(4327.6f - offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-4327.6f, 0, 0);
        }
        else 
        {
            contentRectTransform.localPosition = new Vector3(4327.6f, 0, 0);
        }
    }

    public void OpenLevel(int level)
    {
        selectedLevel = level;
        Debug.Log("openLevel : " + selectedLevel);
        SceneManager.LoadScene("game");
    }
}