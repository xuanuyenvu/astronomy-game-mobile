using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class LevelSelector : MonoBehaviour
{
    public RectTransform contentRectTransform;

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
            IButtonHandler child = transform.GetChild(i).GetComponent<IButtonHandler>();
            
            child.UpdateState(DataSaver.Instance.userModel.Levels[i].State);
            
            if (DataSaver.Instance.userModel.Levels[i].Equals(new LevelItem("game", "unlocked")))
            {
                currentLevel = child.GetLevel();
            } else if (DataSaver.Instance.userModel.Levels[i].Equals(new LevelItem("card", "unlocked")))
            {
                currentLevel = transform.GetChild(i+1).GetComponent<IButtonHandler>().GetLevel();
            }
        }

        ScrollToCurrentLevel();
    }
    
    public void UpdateAfterOpenCard(int cardIndex)
    {
        IButtonHandler child = transform.GetChild(cardIndex).GetComponent<IButtonHandler>();
        child.UpdateState("opened");
    }
    
    // private void OnValidate()
    // {
    //     ScrollToCurrentLevel(); // Cập nhật giao diện khi giá trị state thay đổi từ Inspector
    // }

    private void ScrollToCurrentLevel()
    {
        if (currentLevel >= 4 && currentLevel <= transform.childCount - 2)
        {
            float offset = 4366 - (550 * (currentLevel - 4));
            contentRectTransform.localPosition  = new Vector3(offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-5311.663f, 0, 0);
        }
        else 
        {
            contentRectTransform.localPosition = new Vector3(5311.663f, 0, 0);
        }
    }

    public void PlayGame(int level)
    {
        DataSaver.Instance.selectedLevel = level;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<IButtonHandler>().GetLevel() == level)
            {
                DataSaver.Instance.selectedLevelIndex = i;
            }
        }
        
        SceneManager.LoadScene("game");
    }
}