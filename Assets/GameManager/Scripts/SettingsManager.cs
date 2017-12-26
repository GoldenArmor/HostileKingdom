using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; 

public class SettingsManager : MonoBehaviour
{
    public AudioMixer audioMixer;

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

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); 
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

