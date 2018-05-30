using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSound : MonoBehaviour
{
    [Header("Terrain")]
    [SerializeField]
    GameObject terrain; 

    [Header("Sounds")]
    [SerializeField]
    AudioPlayer audioPlayer;

    float otherSoundCounter = 15f;

    void Start()
    {
        //DEBUG 
        AudioManager.Initialize();
        terrain = GameObject.Find("Terrain"); 
    }

    void Update()
    {
        otherSoundCounter -= Time.deltaTime;
        
        if (otherSoundCounter <= 0)
        {
            otherSoundCounter = 15f;

            audioPlayer.PlayFootSteps(1); 
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == terrain)
        {
            audioPlayer.PlayFootSteps(0);
        }
    }
}
