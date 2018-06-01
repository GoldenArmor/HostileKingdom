using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StatsManager : MonoBehaviour
{
    [SerializeField]
    Text lives;
    [SerializeField]
    Text money;
    [SerializeField]
    GameObject pausePanel; 
	[SerializeField]
	InputManager inputManager; 

    void Update()
    {
        lives.text = Player.lives.ToString();
        money.text = Player.money.ToString();
    }

    public void Paused()
    {
        pausePanel.SetActive(true); 
    }

    public void UnPaused()
    {
		inputManager.gamePause = false; 
        pausePanel.SetActive(false);
    }
}
