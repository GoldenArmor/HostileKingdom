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
    const int tavernScene = 5;
    const int lostScene = 6;
    const int wonScene = 7;

    void Start()
    {
        levelLogic = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelLogic>();
    }

    public void ClickRandomSound()
    {
        //audioManager.Play(Random.Range(0, audioManager.sounds.Length));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #region SceneLoad
    public void ChangetoNextScene()
    {
        levelLogic.StartLoad(levelLogic.nextScene);
    }

    public void ChangetoBackScene()
    {
        levelLogic.StartLoad(levelLogic.backScene);
    }

    public void ChangeToMenu()
    {
        levelLogic.StartLoad(titleScene);
    }

    public void ChangeToGameplay()
    {
        levelLogic.StartLoad(gameplayScene);
    }

    public void ChangeToOptions()
    {
        levelLogic.StartLoad(optionsScene);
    }

    public void ChangeToTavern()
    {
        levelLogic.StartLoad(tavernScene);
    }

    public void ChangeToLost()
    {
        levelLogic.StartLoad(lostScene);
    }

    public void ChangeToWin()
    {
        levelLogic.StartLoad(wonScene);
    }
    #endregion
}
