using UnityEngine;

public class LevelSelector2 : BaseLevelSelector
{
    protected override void Start()
    {
        levelOffset = 19; // Chapter 2 bắt đầu từ Level 19
        base.Start();
    }

    protected override int GetChapterIndex()
    {
        return 2; // Chapter 2
    }

    protected override string GetSceneName()
    {
        return "chapter2";
    }
    
    protected override void ScrollToCurrentLevel()
    {
        if ((currentLevel >= 18 && currentLevel <= 22))
        {
            float offset = 1881.203f - (350 * (currentLevel - 18));
            contentRectTransform.localPosition = new Vector3(offset, 0, 0);
        }
        else if (currentLevel > transform.childCount - 2)
        {
            contentRectTransform.localPosition = new Vector3(-178.7757f, 0, 0);
        }
        else
        {
            contentRectTransform.localPosition = new Vector3(2296.836f, 0, 0);
        }
    }
}