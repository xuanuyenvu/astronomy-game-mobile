using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using EasyTransition;

public class ChapterMenu : MonoBehaviour
{
    public int star = 10;
    public Button chapter2;
    public ParticleSystem particleSystem2;
    public TransitionSettings transition;
    
    void Start()
    {
        if (star >= 12)
        {

            chapter2.interactable = true;
            particleSystem2.Play();
        }
        else
        {
            chapter2.interactable = false;
            particleSystem2.Stop();
        }
    }
    public void LoadChapter1()
    {
        AudioManager.Instance.PlaySFX("Click");
        Debug.Log("Chapter 1");
        // SceneManager.LoadScene("chapter1");
        TransitionManager.Instance().Transition("chapter1", transition, 0f);
    }

    public void LoadChapter2()
    {
        AudioManager.Instance.PlaySFX("Click");
        Debug.Log("Chapter 2");
        // SceneManager.LoadScene("chapter2");
        TransitionManager.Instance().Transition("chapter2", transition, 0f);
    }
}
