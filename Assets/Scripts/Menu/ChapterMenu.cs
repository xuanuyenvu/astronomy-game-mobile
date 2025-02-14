using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using EasyTransition;

public class ChapterMenu : MonoBehaviour
{
    private int star = 8;
    
    public Button chapter2;
    public ParticleSystem particleSystem2;
    public GameObject lock2;

    public Button chapter3;
    public ParticleSystem particleSystem3;
    public GameObject lock3;
    
    public TransitionSettings transition;
    
    void Start()
    {
        Debug.Log("start" + star);
        if (star >= 9)
        {
            chapter2.interactable = true;
            chapter2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            particleSystem2.gameObject.SetActive(true);
            particleSystem2.Play();
            lock2.gameObject.SetActive(false);
        }
        
        if (star >= 18)
        {
            chapter3.interactable = true;
            chapter3.GetComponent<Image>().color = new Color32(167, 219, 255, 255);
            particleSystem3.gameObject.SetActive(true);
            particleSystem3.Play();
            lock3.gameObject.SetActive(false);
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
