using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardButtonHandler : IButtonHandler
{
    public Image card;
    private Button button;
    private Image buttonImg;

    private void Start()
    {
        type = "cardBtn";
        button = GetComponent<Button>();
        buttonImg = GetComponent<Image>();
    }

    public override void UpdateState(int _state)
    {
        switch (_state)
        {
            case 1:
                SetLocked();
                break;
            case 2:
                SetUnlocked();
                break;
            default:
                SetOpened();
                break;
        }
    }
    
    private void SetLocked()
    {
        button.interactable = false;
        buttonImg.enabled = true;
    }

    private void SetUnlocked()
    {
        button.interactable = true;
        buttonImg.enabled = true;
    }

    private void SetOpened()
    {
        button.interactable = false;
        buttonImg.enabled = false;
    }

    public override int GetLevel()
    {
        return 0;
    }
}
