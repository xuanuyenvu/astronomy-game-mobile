using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetInfoUIManager : MonoBehaviour
{
    public void GoToChapterMenu()
    {
        AudioManager.Instance.PlaySFX("Click");
        SceneManager.LoadScene("chapterMenu");
        // TransitionManager.Instance().Transition("planetInfo", transition, 0f);
    }
}
