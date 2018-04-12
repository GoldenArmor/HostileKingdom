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
        pausePanel.SetActive(false);
    }
}
