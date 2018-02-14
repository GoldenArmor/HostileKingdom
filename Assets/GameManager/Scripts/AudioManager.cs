using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public static class AudioManager
{
    public static AudioMixer mainMixer;
    public static AudioMixerSnapshot currentSnapshot;

    public static void Initialize()
    {
        mainMixer = Resources.Load("AudioMixers/MainMixer") as AudioMixer;
        SetSnapshot("MainSnap",0);
        Debug.Log("Audio Mixer initialized: " + mainMixer.name + "\nCurrent Snapshot: " + currentSnapshot.name);
    }

    #region Mixer
    public static void SetMasterVolume(float v)
    {
        try
        {
            mainMixer.SetFloat("Master", v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }        
    }
    public static void SetMusicVolume(float v)
    {
        try
        {
            mainMixer.SetFloat("Music", v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    public static void SetSFXVolume(float v)
    {
        try
        {
            mainMixer.SetFloat("SFX", v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }
    public static void SetAmbientVolume(float v)
    {
        try
        {
            mainMixer.SetFloat("Ambient", v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    public static float GetMasterVolume()
    {
        float v = 0;
        try
        {
            mainMixer.GetFloat("Master", out v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
        return v;
    }
    public static float GetMusicVolume()
    {
        float v = 0;
        try
        {
            mainMixer.GetFloat("Music", out v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
        return v;
    }
    public static float GetSFXVolume()
    {
        float v = 0;
        try
        {
            mainMixer.GetFloat("SFX", out v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
        return v;
    }
    public static float GetAmbientVolume()
    {
        float v = 0;
        try
        {
            mainMixer.GetFloat("Ambient", out v);
        }
        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
        return v;
    }

    public static void SetSnapshot(string name, float timeToReach)
    {
        currentSnapshot = mainMixer.FindSnapshot(name);
        currentSnapshot.TransitionTo(timeToReach);
    }
    public static string GetSnapshotName()
    {
        return currentSnapshot.name;
    }
    #endregion

    #region Audiosource
    public static AudioSource AddAudioSource(this GameObject obj)
    {
        AudioSource source = obj.AddComponent<AudioSource>();

        source.volume = 1;
        source.pitch = 1;

        source.spread = 150;
        source.panStereo = 0;
        source.spatialBlend = 1;
        source.dopplerLevel = 0;
        source.rolloffMode = AudioRolloffMode.Logarithmic;
        source.minDistance = 5;
        source.maxDistance = 8;

        return source;
    }
    public static AudioSource AddAudioSource(this GameObject obj, AudioClip clip, float v, float p, bool play)
    {
        AudioSource source = obj.AddAudioSource();

        source.clip = clip;

        source.volume = v;
        source.pitch = p;

        if(play) source.TryPlay();

        return source;
    }

    public static void SetOutputMixer(this AudioSource source, string groupName)
    {
        source.outputAudioMixerGroup = mainMixer.FindMatchingGroups(groupName)[0];
    }

    public static bool TryPlay(this AudioSource source)
    {
        if(!source.isPlaying)
        {
            source.Play();
            return true;
        }
        Debug.Log("Playing already");
        return false;
    }
    public static bool TryStop(this AudioSource source)
    {
        if(source.isPlaying)
        {
            source.Stop();
            return true;
        }
        Debug.Log("Stopped already");
        return false;
    }
    
    public static void Play(this AudioSource source, AudioClip clip, float volume, float pitch, bool loop, bool audio2D, string mixerGroupName)
    {
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;

        if(audio2D) source.spatialBlend = 0;
        else
        {
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
        }

        source.loop = loop;
        source.outputAudioMixerGroup = mainMixer.FindMatchingGroups(mixerGroupName)[0];

        source.TryPlay();
    }
    #endregion
}