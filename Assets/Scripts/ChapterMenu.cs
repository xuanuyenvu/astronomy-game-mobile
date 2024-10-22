using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterMenu : MonoBehaviour
{
    public ParticleSystem particleSystem1;
    public ParticleSystem particleSystem2;

    void Start()
    {
        particleSystem1.Stop();
        particleSystem2.Stop();
        particleSystem1.gameObject.SetActive(false);
        particleSystem2.gameObject.SetActive(false);
    }


    public void LoadChapter1()
    {
        particleSystem1.gameObject.SetActive(true);
        particleSystem1.Play();
        StartCoroutine(WaitAndLoadScene1());
    }

    private IEnumerator WaitAndLoadScene1()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Chapter 1");
        SceneManager.LoadScene("chapter1");  
    }


    public void LoadChapter2()
    {
        particleSystem2.gameObject.SetActive(true);
        particleSystem2.Play();
        StartCoroutine(WaitAndLoadScene2());
    }

    private IEnumerator WaitAndLoadScene2()
    {
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Chapter 2");
        SceneManager.LoadScene("chapter1");
    }
}
