using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelButtonHandler : MonoBehaviour
{
    public Button unlockedButton;
    public Button lockedButton;

    public int level;

    private void Awake()
    {
        if (unlockedButton == null)
        {
            unlockedButton = transform.Find("unlockedLevel").GetComponent<Button>();
        }
        if (lockedButton == null)
        {
            lockedButton = transform.Find("lockedLevel").GetComponent<Button>();
        }

        string levelText;
        if (level < 10)
        {
            levelText = "0" + level.ToString();
        }
        else
        {
            levelText = level.ToString();
        }

        TextMeshProUGUI ubText = unlockedButton.GetComponentInChildren<TextMeshProUGUI>();
        ubText.text = levelText;

        TextMeshProUGUI lbText = lockedButton.GetComponentInChildren<TextMeshProUGUI>();
        lbText.text = levelText;
    }

    public void LockThisButton()
    {
        unlockedButton.gameObject.SetActive(false);
        lockedButton.gameObject.SetActive(true);
        lockedButton.interactable = false;
    }

    public void UnlockThisButton()
    {
        unlockedButton.gameObject.SetActive(true);
        lockedButton.gameObject.SetActive(false);
    }
}

