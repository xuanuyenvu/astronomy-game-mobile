using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonHandler : IButtonHandler
{
    public Sprite card;
    public ParticleSystem glowPS;
    
    private Button button;
    private Image buttonImg;

    private void Start()
    {
        type = "card";
        button = GetComponent<Button>();
        buttonImg = GetComponent<Image>();
        glowPS.gameObject.SetActive(false);
    }

    public override void UpdateState(string levelState, int chapter)
    {
        switch (levelState)
        {
            case "locked":
                SetLocked();
                break;
            case "unlocked":
                SetUnlocked();
                break;
            case "opened":
                SetOpened();
                break;
        }
    }
    
    private void SetLocked()
    {
        button.interactable = false;
        button.enabled = false;
    }

    private void SetUnlocked()
    {
        button.enabled = true;
        button.interactable = true;
        buttonImg.sprite = card;
        buttonImg.color = new Color(1, 1, 1, 1);
        glowPS.gameObject.SetActive(true);
        glowPS.Play();
    }

    private void SetOpened()
    {
        glowPS.Pause();
        glowPS.gameObject.SetActive(false);
        button.enabled = true;
        button.interactable = false;
        buttonImg.sprite = card;
    }

    public override int GetLevel()
    {
        return 0;
    }
}
