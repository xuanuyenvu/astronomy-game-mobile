using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    public Image largeCircle;
    public Image mediumCircle;
    public Image star;
    public Image rock;
    public TextMeshProUGUI levelText;
    [SerializeField] public int state = 1;
    [SerializeField] private int level = 1;
    
    public Button button;

    private void Start()
    {
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
    
    public int GetLevel()
    {
        return level;
    }

    // private void OnValidate()
    // {
    //     UpdateState(); // Cập nhật giao diện khi giá trị state thay đổi từ Inspector
    // }

    public void UpdateState(int _state)
    {
        switch (_state)
        {
            case 1:
                SetLocked();
                break;
            case 2:
                SetUnlockedUnplayed();
                break;
            case 3:
                SetPlayedRocket();
                break;
            case 4:
                SetPlayedStar();
                break;
        }
    }

    private void SetLocked()
    {
        button.interactable = false;
        rock.gameObject.SetActive(false);
        star.gameObject.SetActive(false);

        largeCircle.color = Color.white;
        largeCircle.color = new Color(largeCircle.color.r, largeCircle.color.g, largeCircle.color.b, 80f / 255f);
        mediumCircle.color = Color.white;
        mediumCircle.color = new Color(mediumCircle.color.r, mediumCircle.color.g, mediumCircle.color.b, 30f / 255f);
        levelText.alpha = 80f / 255f;

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
        mediumCircle.color = Color.white;
        mediumCircle.color = new Color(mediumCircle.color.r, mediumCircle.color.g, mediumCircle.color.b, 50f / 255f);

        levelText.alpha = 225f / 255f;
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = Vector2.zero;
            // rectTransform.offsetMax = Vector2.zero;
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }
    }

    private void SetPlayedRocket()
    {
        button.interactable = true;
        rock.gameObject.SetActive(true);
        star.gameObject.SetActive(false);
        
        SetColorWhenPlayed();
        
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = new Vector2(-96, -30);
            // rectTransform.offsetMax = new Vector2(96, 30);
            rectTransform.anchoredPosition = new Vector3(170, -93, 0);
        }
    }

    private void SetPlayedStar()
    {
        button.interactable = true;
        rock.gameObject.SetActive(false);
        star.gameObject.SetActive(true);
        
        SetColorWhenPlayed();
        
        RectTransform rectTransform = levelText.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // rectTransform.offsetMin = new Vector2(-96, -30);
            // rectTransform.offsetMax = new Vector2(96, 30);
            rectTransform.anchoredPosition = new Vector3(170, -93, 0);
        }
    }

    private void SetColorWhenPlayed()
    {
        largeCircle.color = new Color(177f / 255f, 192f / 255f, 255f / 255f, 200f / 255f);
        mediumCircle.color = new Color(50f / 255f, 83f / 255f, 157f / 255f, 255f / 255f);
        levelText.alpha = 225f / 255f;
    }
}
