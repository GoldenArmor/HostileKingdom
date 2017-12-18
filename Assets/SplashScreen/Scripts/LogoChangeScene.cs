using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; 

public class LogoChangeScene : MonoBehaviour
{
    LevelLogic levelLogic;
    VideoPlayer video;

    float videoCounter; 

	void Start ()
    {
        levelLogic = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelLogic>();
        video = GetComponent<VideoPlayer>(); 
    }
	
	void Update ()
    {
        videoCounter += Time.deltaTime; 

        if (videoCounter > video.clip.length)
        {
            levelLogic.StartLoad(levelLogic.nextScene); 
        }
	}
}
