using UnityEngine;

public class LevelSelector : BaseLevelSelector
{
    protected override void Start()
    {
        levelOffset = 0; // Không có độ lệch cho chapter 1
        base.Start();
    }

    protected override int GetChapterIndex()
    {
        return 1; // Chapter 1
    }

    protected override string GetSceneName()
    {
        return "chapter1";
    }
    
    protected override void ScrollToCurrentLevel()
    {
        if ((currentLevel >= 3 && currentLevel <= 12))
        {
            float offset = 2966 - (350 * (currentLevel - 3));
            contentRectTransform.localPosition = new Vector3(offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-1254.875f, 0, 0);
        }
        else
        {
            contentRectTransform.localPosition = new Vector3(3372.919f, 0, 0);
        }
    }
}