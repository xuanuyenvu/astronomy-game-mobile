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
          4, 4, 3, 4, 3, 3, 4, 4, 1, 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
        //L, L, L, L, C, L, L, L, C, L, L, L, L, C, L, L, L, L, C, L, L, C, L, L
    };
    [SerializeField] private int currentLevel = 1; 

    void Start()
    {
        InitializeLevelButtons();
    }

    private void InitializeLevelButtons()
    {
        // transform.childCount
        for (int i = 0; i < transform.childCount; i++)
        {
            // Lấy đối tượng con tại vị trí i
            IButtonHandler child = transform.GetChild(i).GetComponent<IButtonHandler>();
            child.UpdateState(levelDatabase[i]);
            
            if (levelDatabase[i] == 2 && child.type == "levelBtn")
            {
                currentLevel = child.GetLevel();
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
            float offset = 3420 - (550 * (currentLevel - 4));
            contentRectTransform.localPosition  = new Vector3(offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-4305.662f, 0, 0);
        }
        else 
        {
            contentRectTransform.localPosition = new Vector3(4305.662f, 0, 0);
        }
    }

    public void OpenLevel(int level)
    {
        selectedLevel = level;
        Debug.Log("openLevel : " + selectedLevel);
        SceneManager.LoadScene("game");
    }
}