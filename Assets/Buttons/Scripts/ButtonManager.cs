using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    LevelLogic levelLogic;

    [Header("Scenes")]
    const int titleScene = 1;
    const int gameplayScene = 2;
    const int lostScene = 3;
    const int wonScene = 4;

    //[SerializeField]
    //AudioSource audioSource;
    //[SerializeField]
    //AudioSource musicAudioSource;

	[SerializeField]
	AudioPlayer audioPlayer; 
	float musicCounter = 30f; 

    void Start()
    {
        levelLogic = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelLogic>();
		audioPlayer.PlayMusic (0);
    }

	void Update()
	{
		musicCounter -= Time.deltaTime; 

		if (musicCounter <= 0) 
		{
			audioPlayer.StopMusic (); 
			audioPlayer.PlayMusic(Random.Range (0, audioPlayer.musicClips.Length - 1));
			musicCounter = 30f; 
		}
	}

    public void ClickRandomSound()
    {
        //audioPlayer.Play(audioPlayer.sfxClips[Random.Range(0, audioPlayer.sfxClips.Length)], 1, 1, false, false, "Sounds");
        //audioSource.PlayOneShot(audioSource.clip); 
		audioPlayer.PlaySFX (0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region SceneLoad
    public void ChangetoNextScene()
    {
        //musicAudioSource.Stop(); 
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(levelLogic.nextScene);
    }

    public void ChangetoBackScene()
    {
        //musicAudioSource.Stop();
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(levelLogic.backScene);
    }

    public void ChangeToMenu()
    {
        //musicAudioSource.Stop();
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(titleScene);
    }

    public void ChangeToGameplay()
    {
        //musicAudioSource.Stop();
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(gameplayScene);
    }

    public void ChangeToLost()
    {
        //musicAudioSource.Stop();
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(lostScene);
    }

    public void ChangeToWin()
    {
        //musicAudioSource.Stop();
		audioPlayer.StopMusic (); 
        levelLogic.StartLoad(wonScene);
    }
    #endregion
}
