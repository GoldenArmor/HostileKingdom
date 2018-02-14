using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class SettingsManager : MonoBehaviour
{
    float oldMasterVolume = AudioManager.GetMasterVolume(); 

    bool isFullScreen;

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

    public void SetFullscreen()
    {
        Screen.fullScreen = isFullScreen; 
    }
}

