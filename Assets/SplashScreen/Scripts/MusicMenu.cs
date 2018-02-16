using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicMenu : MonoBehaviour
{
    public AudioPlayer audioPlayer; 
	// Use this for initialization
	void Start ()
    {
        audioPlayer.PlayMusic(0); 
	}

}
