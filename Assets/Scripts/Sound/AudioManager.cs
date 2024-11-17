using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel.Design.Serialization;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    public List<Sound> musicSounds;
    public List<Sound> sfxSounds;
    
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("In Menu");
    }

    public void PlayMusic(string name)
    {
        Sound s = musicSounds.Find(x => x.name == name);
        
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = sfxSounds.Find(x => x.name == name);
        
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found!");
            return;
        }
        
        sfxSource.PlayOneShot(s.clip);
        
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
    
    public float GetMusicVolume()
    {
        return musicSource.volume;
    }
    
    public float GetSFXVolume()
    {
        return sfxSource.volume;
    }
}