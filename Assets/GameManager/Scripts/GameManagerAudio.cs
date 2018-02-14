using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerAudio : MonoBehaviour
{
    AudioListener audioListener;

    bool isPlayingAmbient;


    void Start ()
    {
        audioListener = GetComponent<AudioListener>();
    }
	
	void Update ()
    {
        //if (LevelLogic.currentScene == 2) audioListener.enabled = false;

        //if (LevelLogic.currentScene == 3 && !isPlayingAmbient)
        //{
        //    for (int i = 0; i < audioManager.sounds.Length; i++)
        //    {
        //        AudioSource[] audioSources = GetComponents<AudioSource>();
        //        for (int v = 0; v < audioSources.Length; v++)
        //        {
        //            audioSources[v].Stop();
        //            Destroy(audioSources[v]);
        //        }
        //    }
        //    audioManager.Play(2);
        //    isPlayingAmbient = true; 
        //}
	}
}
