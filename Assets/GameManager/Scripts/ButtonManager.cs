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
    const int optionsScene = 3;
    const int tavernScene = 4;
    const int lostScene = 5;
    const int winScene = 6; 

    void Start()
    {
        levelLogic = GameObject.FindGameObjectWithTag("Manager").GetComponent<LevelLogic>(); 
    }

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
        levelLogic.StartLoad(1);
    }

    public void ChangeToGameplay()
    {
        levelLogic.StartLoad(2);
    }

    public void ChangeToOptions()
    {
        levelLogic.StartLoad(3);
    }

    public void ChangeToTavern()
    {
        levelLogic.StartLoad(4);
    }

    public void ChangeToLost()
    {
        levelLogic.StartLoad(5);
    }

    public void ChangeToWin()
    {
        levelLogic.StartLoad(6); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
