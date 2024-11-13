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
        4, 4, 3, 4, 3, 3, 4, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
    };
    [SerializeField] private int currentLevel = 1; 

    void Start()
    {
        InitializeLevelButtons();
    }

    private void InitializeLevelButtons()
    {
        // transform.childCount
        for (int i = 0; i < 19; i++)
        {
            // Lấy đối tượng con tại vị trí i
            IButtonHandler child = transform.GetChild(i).GetComponent<IButtonHandler>();
            Debug.Log("levelDatabase[i] : " + levelDatabase[i] + "type : " + child.type);
            child.UpdateState(levelDatabase[i]);
            
            if (levelDatabase[i] == 2 && child.type == "levelBtn")
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
            float offset = 3633 - (600 * (currentLevel - 4));
            contentRectTransform.localPosition  = new Vector3(offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-4707.061f, 0, 0);
        }
        else 
        {
            contentRectTransform.localPosition = new Vector3(4707.061f, 0, 0);
        }
    }

    public void OpenLevel(int level)
    {
        selectedLevel = level;
        Debug.Log("openLevel : " + selectedLevel);
        SceneManager.LoadScene("game");
    }
}