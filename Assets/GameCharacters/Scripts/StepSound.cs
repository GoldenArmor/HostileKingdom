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

    void Start()
    {
        //DEBUG 
        AudioManager.Initialize(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == terrain)
        {
            audioPlayer.PlayFootSteps(RandomStepSound());
        }
        Debug.Log(other);
    }

    int RandomStepSound()
    {
        return Random.Range(0, audioPlayer.footStepsClips.Length - 1); 
    }
}
