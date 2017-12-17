using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [HideInInspector]
    public int index; 

    public string name; 

    public AudioClip clip;

    public bool getRandomValues; 

    [Range(0f, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;

    [HideInInspector]
    public bool playingSound;

    public float maxRollOff;
    public float minRollOff; 

    [HideInInspector]
    public AudioSource source;
    public bool loop;
}
