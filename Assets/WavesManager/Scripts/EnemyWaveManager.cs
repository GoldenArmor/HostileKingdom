using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class EnemyWaveManager : MonoBehaviour
{
    public static int enemiesAlive = 0;
    public static int wavesCount = 0;
    [SerializeField]
    Text waveCounterText;

    void Start()
    {
        waveCounterText.text = ("0/10");    
    }

    public void ChangeWavesCount(int newValue)
    {
        waveCounterText.text = (newValue + "/10");
    }
}
