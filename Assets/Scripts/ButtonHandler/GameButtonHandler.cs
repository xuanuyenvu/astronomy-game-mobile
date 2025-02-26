using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameButtonHandler : IButtonHandler
{
    public Image largeCircle;
    public Image mediumCircle;
    public Image star;
    public Image rock;
    public TextMeshProUGUI levelText;
    [SerializeField] public int state = 1;
    public int level = 1;
    
    public Button button;

    private void Start()
    {
        type = "level";
        SetLevel(level);
    }

    private void SetLevel(int level)
    {
        if (level < 10)
        {
            levelText.text = "0" + level;
        }
        else
        {
            levelText.text = level.ToString();
        }
    }
    
    public override int GetLevel()
    {
        return level;
    }

    // private void OnValidate()
    // {
    //     UpdateState(); // Cập nhật giao diện khi giá trị state thay đổi từ Inspector
    // }

    public override void UpdateState(string levelState, int chapter)
    {
        switch (levelState)
        {
            case "locked":
                SetLocked();
                break;
            case "unlocked":
                SetUnlockedUnplayed();
                break;
            case "rock":
                SetPlayedRocket(chapter);
                break;
            case "star":
                SetPlayedStar(chapter);
                break;
        }
    }

    private void SetLocked()
    {
        button.interactable = false;
        rock.gameObject.SetActive(false);
        star.gameObject.SetActive(false);
        
        largeCircle.color = new Color(208 / 255f, 224 / 255f, 236f / 255f, 50f / 255f);
        mediumCircle.color = new Color(208 / 255f, 224 / 255f, 236f / 255f, 15f / 255f);
        levelText.color = new Color(208 / 255f, 224 / 255f, 236f / 255f, 30f / 255f);

        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = Vector2.zero;
            // rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    private void SetUnlockedUnplayed()
    {
        button.interactable = true;
        rock.gameObject.SetActive(false);
        star.gameObject.SetActive(false);

        largeCircle.color = Color.white;
        largeCircle.color = new Color(largeCircle.color.r, largeCircle.color.g, largeCircle.color.b, 255f / 255f);
        largeCircle.color = Color.white;
        mediumCircle.color = new Color(mediumCircle.color.r, mediumCircle.color.g, mediumCircle.color.b, 80f / 255f);

        levelText.color = Color.white;
        levelText.alpha = 225f / 255f;
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = Vector2.zero;
            // rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    private void SetPlayedRocket(int chapter)
    {
        button.interactable = true;
        rock.gameObject.SetActive(true);
        star.gameObject.SetActive(false);

        switch (chapter)
        {
            case 1:
                SetColorWhenPlayed_Chapter1();
                break;
            default:
                SetColorWhenPlayed_Chapter2();
                break;
        }
        
        
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = new Vector2(-96, -30);
            // rectTransform.offsetMax = new Vector2(96, 30);
            rectTransform.anchoredPosition = new Vector3(144, -95, 0);
        }
    }

    private void SetPlayedStar(int chapter)
    {
        button.interactable = true;
        rock.gameObject.SetActive(false);
        star.gameObject.SetActive(true);
        
        switch (chapter)
        {
            case 1:
                SetColorWhenPlayed_Chapter1();
                break;
            default:
                SetColorWhenPlayed_Chapter2();
                break;
        }
        
        
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = new Vector2(-96, -30);
            // rectTransform.offsetMax = new Vector2(96, 30);
            rectTransform.anchoredPosition = new Vector3(144, -95, 0);
        }
    }

    private void SetColorWhenPlayed_Chapter1()
    {
        // largeCircle.color = new Color(177f / 255f, 192f / 255f, 255f / 255f, 200f / 255f);
        // mediumCircle.color = new Color(50f / 255f, 83f / 255f, 157f / 255f, 255f / 255f);
        
        largeCircle.color = new Color(91f / 255f, 166f / 255f, 232f / 255f, 200f / 255f);
        mediumCircle.color = new Color(50f / 255f, 83f / 255f, 157f / 255f, 255f / 255f);
        
        levelText.color = Color.white;
        levelText.alpha = 225f / 255f;
    }
    
    private void SetColorWhenPlayed_Chapter2()
    {
        largeCircle.color = new Color(177f / 255f, 192f / 255f, 255f / 255f, 200f / 255f);
        mediumCircle.color = new Color(81f / 255f, 122f / 255f, 215f / 255f, 255f / 255f);

        levelText.color = Color.white;
        levelText.alpha = 225f / 255f;
    }
}
