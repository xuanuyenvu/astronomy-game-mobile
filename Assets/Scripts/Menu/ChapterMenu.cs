using UnityEngine;
using UnityEngine.UI;
using EasyTransition;

public class ChapterMenu : MonoBehaviour
{
    public Button chapter2;
    public ParticleSystem particleSystem2;
    public GameObject lock2;

    public Button chapter3;
    public ParticleSystem particleSystem3;
    public GameObject lock3;
    
    public TransitionSettings transition;
    
    void Start()
    {
        int star = DataSaver.Instance.userModel.Star;
        
        if (star >= 0)// CẦN CHỈNH 0 --> 10
        {
            chapter2.interactable = true;
            chapter2.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            particleSystem2.gameObject.SetActive(true);
            particleSystem2.Play();
            lock2.gameObject.SetActive(false);
        }
        
        if (star >= 19)
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
