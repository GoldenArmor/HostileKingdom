using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds; 

	void Awake ()
    {
		foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop; 

            if (s.getRandomValues)
            {
                s.source.volume = UnityEngine.Random.Range(s.volume - 0.25f, s.volume + 0.25f);
                s.source.pitch = UnityEngine.Random.Range(s.pitch - 0.2f, s.pitch + 0.2f);
            }
        }
	}

    void Start()
    {
        //Sonidos que quiero playear al principio del juego
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            return; 
        }
        s.source.Play(); 
    }
}
