using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    LevelLogic levelLogic;

    [Header("Scenes")]
    const int titleScene = 2;
    const int gameplayScene = 3;
    const int optionsScene = 4;
    const int tavernScene = 8;
    const int lostScene = 5;
    const int wonScene = 6;

    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioSource musicAudioSource;

    void Start()
    {
        levelLogic = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelLogic>();
    }

    public void ClickRandomSound()
    {
        //audioPlayer.Play(audioPlayer.sfxClips[Random.Range(0, audioPlayer.sfxClips.Length)], 1, 1, false, false, "Sounds");
        audioSource.PlayOneShot(audioSource.clip); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region SceneLoad
    public void ChangetoNextScene()
    {
        musicAudioSource.Stop(); 
        levelLogic.StartLoad(levelLogic.nextScene);
    }

    public void ChangetoBackScene()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(levelLogic.backScene);
    }

    public void ChangeToMenu()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(titleScene);
    }

    public void ChangeToGameplay()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(gameplayScene);
    }

    public void ChangeToOptions()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(optionsScene);
    }

    public void ChangeToTavern()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(tavernScene);
    }

    public void ChangeToLost()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(lostScene);
    }

    public void ChangeToWin()
    {
        musicAudioSource.Stop();
        levelLogic.StartLoad(wonScene);
    }
    #endregion
}
