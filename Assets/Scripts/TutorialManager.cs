using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public TextMeshProUGUI[] tutorialText;
    
    private int currentTutorial = -1;
    private float fadeDuration = 0.6f;

    void Start()
    {
        foreach (var el in tutorialText)
        {
            Color color = el.color;
            color.a = 0f;  
            el.color = color;
            
            Image icon = el.transform.GetChild(0).GetComponent<Image>();
            if (icon != null)
            {
                Color iconColor = icon.color;
                iconColor.a = 0f;
                icon.color = iconColor;
            }
        }
    }
    
    private IEnumerator ShowTutorialWithDelay(TextMeshProUGUI text, Image icon, float delayTime = 0.4f)
    {
        yield return new WaitForSeconds(delayTime); 
        ShowTextWithFade(text, icon);
    }
    
    private void ShowTextWithFade(TextMeshProUGUI text, Image icon)
    {
        text.gameObject.SetActive(true);
        icon.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence(); 
        seq.Join(text.DOFade(200f / 255f, fadeDuration))  
            .Join(icon.DOFade(180f / 255f, fadeDuration)); 

        seq.Play(); 
    }


    private void HideTextWithFade(TextMeshProUGUI text)
    {
        text.DOFade(0f, fadeDuration).OnComplete(() => text.gameObject.SetActive(false));
    }
    
    public void StartTutorialLevel1()
    {
        currentTutorial = 0;

        Image icon = tutorialText[0].transform.GetChild(0).GetComponent<Image>(); 
        StartCoroutine(ShowTutorialWithDelay(tutorialText[0], icon));
    }

    public void StartTutorialLevel2()
    {
        currentTutorial = 2;
        
        Image icon = tutorialText[2].transform.GetChild(0).GetComponent<Image>(); 
        StartCoroutine(ShowTutorialWithDelay(tutorialText[2], icon));
    }

    public void StartTutorialLevel4()
    {
        currentTutorial = 3;
        
        Image icon = tutorialText[3].transform.GetChild(0).GetComponent<Image>(); 
        StartCoroutine(ShowTutorialWithDelay(tutorialText[3], icon));
    }

    public void StartTutorialLevel15()
    {
        currentTutorial = 4;
        
        Image icon = tutorialText[4].transform.GetChild(0).GetComponent<Image>(); 
        StartCoroutine(ShowTutorialWithDelay(tutorialText[4], icon));
    }
    public void StopTutorialLevel()
    {
        if (currentTutorial == 0)
        {
            tutorialText[0].gameObject.SetActive(false);
            currentTutorial = 1;
        
            Image icon = tutorialText[1].transform.GetChild(0).GetComponent<Image>(); 
            StartCoroutine(ShowTutorialWithDelay(tutorialText[1], icon, 0.5f));
        }
        else
        {
            if (currentTutorial >= 0 || currentTutorial < tutorialText.Length)
            {
                tutorialText[currentTutorial].transform.DOScale(0f, fadeDuration);
                currentTutorial = -1;
            }
        }
    }
}
