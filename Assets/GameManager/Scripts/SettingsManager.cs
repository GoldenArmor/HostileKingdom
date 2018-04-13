using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class SettingsManager : MonoBehaviour
{
    float oldMasterVolume; 

    bool isFullScreen = true;
    [SerializeField]
    GameObject menu;

    void Start()
    {
        oldMasterVolume = AudioManager.GetMasterVolume();
    }

    public void SetFirstResolution()
    {
        Screen.SetResolution(1280, 720, isFullScreen);
    }
    public void SetSecondResolution()
    {
        Screen.SetResolution(1600, 1200, isFullScreen);
    }
    public void SetThirdResolution()
    {
        Screen.SetResolution(1920, 1080, isFullScreen);
    }

    public void SetResolution(int label)
    {
        if (label == 1)
        {
            Screen.SetResolution(1280, 720, isFullScreen);
        }
        if (label == 2)
        {
            Screen.SetResolution(1600, 1200, isFullScreen);
        }
        if (label == 3)
        {
            Screen.SetResolution(1920, 1080, isFullScreen);
        }
    }

    public void SetMusicVolume(float volume)
    {
        AudioManager.SetMusicVolume(volume);
        AudioManager.SetAmbientVolume(volume);
    }

    public void SetFXVolume(float volume)
    {
        AudioManager.SetSFXVolume(volume); 
    }

    public void DisableAudio(bool isEnabled)
    {
        if (isEnabled)
        {
            AudioManager.SetMasterVolume(oldMasterVolume);
        }
        else
        {          
            oldMasterVolume = AudioManager.GetMasterVolume();
            AudioManager.SetMasterVolume(0);
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex); 
    }

    public void SetFullscreen(bool newFullScreen)
    {
        Screen.fullScreen = newFullScreen; 
    }

    public void Disable()
    {
        gameObject.SetActive(false);
        menu.SetActive(true); 
    }
    public void Enable()
    {
        gameObject.SetActive(true);
        menu.SetActive(false);
    }
}

