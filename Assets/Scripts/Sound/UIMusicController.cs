using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMusicController : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    public void ToggleMusic()
    {
        AudioManager.Instance.ToggleMusic();
    }
    
    public void ToggleSFX()
    {
        AudioManager.Instance.ToggleSFX();
    }
    
    public void SetMusicVolume()
    {
        AudioManager.Instance.SetMusicVolume(musicSlider.value);
    }
    
    public void SetSFXVolume()
    {
        AudioManager.Instance.SetSFXVolume(sfxSlider.value);
    }
}
