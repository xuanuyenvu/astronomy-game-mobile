using System.Collections;
using UnityEngine;
using EasyTransition;

public abstract class BaseLevelSelector : MonoBehaviour
{
    public RectTransform contentRectTransform;
    public TransitionSettings transition;
    
    [SerializeField] protected int currentLevel = 1;
    protected int levelOffset = 0; // Định nghĩa mức độ lệch của Level (VD: 0 cho chapter 1, 19 cho chapter 2)

    protected virtual void Start()
    {
        InitializeLevelButtons();
    }

    protected virtual void InitializeLevelButtons()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            IButtonHandler child = transform.GetChild(i).GetComponent<IButtonHandler>();
            
            // Lấy dữ liệu cấp độ theo offset (cho LevelSelector = 0, LevelSelector2 = 19)
            child.UpdateState(DataSaver.Instance.userModel.Levels[i + levelOffset].State, GetChapterIndex());
            
            if (DataSaver.Instance.userModel.Levels[i + levelOffset].Equals(new LevelItem("game", "unlocked")))
            {
                currentLevel = child.GetLevel();
            }
            else if (DataSaver.Instance.userModel.Levels[i + levelOffset].Equals(new LevelItem("card", "unlocked")))
            {
                currentLevel = transform.GetChild(i + 1).GetComponent<IButtonHandler>().GetLevel();
            }
        }

        ScrollToCurrentLevel();
    }
    
    public void UpdateAfterOpenCard(int cardIndex)
    {
        IButtonHandler child = transform.GetChild(cardIndex - levelOffset).GetComponent<IButtonHandler>();
        child.UpdateState("opened", GetChapterIndex());

        IButtonHandler nextChild = transform.GetChild((cardIndex - levelOffset) + 1).GetComponent<IButtonHandler>();
        nextChild.UpdateState("unlocked", GetChapterIndex());
    }

    protected abstract void ScrollToCurrentLevel();

    public void PlayGame(int level)
    {
        AudioManager.Instance.PlaySFX("Click");
        DataSaver.Instance.selectedLevel = level;
        
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<IButtonHandler>().GetLevel() == level)
            {
                DataSaver.Instance.selectedLevelIndex = i + levelOffset;
            }
        }

        DataSaver.Instance.sceneName = GetSceneName();
        TransitionManager.Instance().Transition("game", transition, 0f, true);
    }

    protected abstract int GetChapterIndex();
    protected abstract string GetSceneName();
}