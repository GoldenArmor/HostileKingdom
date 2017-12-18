using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; 

    void Start()
    {
        //Sonidos que quiero playear al principio del juego
        if (LevelLogic.currentScene == 0)
        {
            Play(1); 
        }
    }

    public void Play(int index)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        Sound currentSound = sounds[index]; 
        audioSource.clip = currentSound.clip;

        audioSource.volume = currentSound.volume;
        audioSource.pitch = currentSound.pitch;

        if (currentSound.getRandomValues)
        {
            audioSource.volume = UnityEngine.Random.Range(currentSound.volume - 0.25f, currentSound.volume + 0.25f);
            audioSource.pitch = UnityEngine.Random.Range(currentSound.pitch - 0.2f, currentSound.pitch + 0.2f);
        }

        audioSource.minDistance = currentSound.minRollOff;
        audioSource.maxDistance = currentSound.maxRollOff;
        audioSource.loop = currentSound.loop;
        currentSound.playingSound = true;

        audioSource.Play();
        Destroy(audioSource, currentSound.clip.length); 
    }
}
