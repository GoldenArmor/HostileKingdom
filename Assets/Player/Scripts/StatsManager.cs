using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class StatsManager : MonoBehaviour
{
    Text lives;
    Text money;

    void Update()
    {
        lives.text = Player.lives.ToString();
        money.text = Player.money.ToString();
    }
}
