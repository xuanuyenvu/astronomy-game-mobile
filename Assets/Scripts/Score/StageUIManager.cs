using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StageUIManager : MonoBehaviour
{
    // Hiện 2 hay 3 cái --> set up ban đầu
    // qua từng màn thì hiệu ứng thế nào

    public GameObject[] stageUIList;
    public Image[] circleUIList;
    
    private int completedStages;
    private int numOfStages;
    
    
    public void Initialize(int _numOfStages)
    {
        numOfStages = _numOfStages;
        SetUp();
    }
    
    public void SetUp()
    {
        completedStages = 0;
        
        for (int i = 0; i < numOfStages; i++)
        {
            GameObject el = GetChildByName(stageUIList[i], "smallRing");
            if (el != null)
            {
                el.SetActive(true);
                SetImageAlpha(el.GetComponent<Image>());
            }
        }
        SetImageAlpha(GetChildByName(stageUIList[numOfStages-1], "smallRing").GetComponent<Image>(), 255f);
        
        
        switch (numOfStages)
        {
            case 2:
                circleUIList[0].gameObject.SetActive(true);
                SetImageAlpha(circleUIList[0].GetComponent<Image>());
                break;
            case 3:
                circleUIList[0].gameObject.SetActive(true);
                circleUIList[1].gameObject.SetActive(true);
                
                SetImageAlpha(circleUIList[0].GetComponent<Image>());
                SetImageAlpha(circleUIList[1].GetComponent<Image>());
                break;
        }
    }
    
    private GameObject GetChildByName(GameObject parent, string childName)
    {
        Transform child = parent.transform.Find(childName);
        if (child != null)
        {
            return child.gameObject;
        }
        else
        {
            Debug.Log($"Child with the name '{childName}' does not exist in '{parent.name}'.");
            return null;
        }
    }
    
    private void SetImageAlpha(Image image, float alpha = 50f)
    {
        if (image != null)
        {
            Color color = image.color;
            color.a = alpha / 255f; 
            image.color = color;
        }
        else
        {
            Debug.LogWarning("SetImageAlpha: Image component is null!");
        }
    }

    public void UpdateStageUI()
    {
        GameObject star = GetChildByName(stageUIList[numOfStages - 1 - completedStages], "star");
        if (star != null)
        {
            star.SetActive(true);
            star.transform.localScale = Vector3.one * 2.5f; 
            
            // Scale về 1 bằng DOTween
            star.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        }

        completedStages++;
        Debug.Log($"Stage UI Updated on {completedStages} stages.");
        
        if (completedStages < numOfStages)
        {
            Debug.Log($"el {numOfStages - 1 - completedStages} stages.");
            GameObject circle = GetChildByName(stageUIList[numOfStages - 1 - completedStages], "smallRing");
            if (circle != null)
            {
                SetImageAlpha(circle.GetComponent<Image>(), 255f);
            }

            SetImageAlpha(circleUIList[numOfStages - 1 - completedStages].GetComponent<Image>(), 255f);
        }
    }
}
