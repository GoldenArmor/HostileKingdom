using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviour
{
    public static int EnemiesAlive = 0; 

    [Header("Wave Properties")]
    [SerializeField]
    Wave[] waves;

    [Header("Wave Control")]
    [SerializeField]
    float waveDelay;
    [SerializeField]
    Text waveDelayText; 
    float timer;
    int waveIndex;

    void Update()
    {
        if (EnemiesAlive > 0)
        {
            return; 
        }

        if (timer <= 0)
        {
            StartCoroutine(SpawnWave());
            timer = waveDelay;
            return; 
        }

        timer -= Time.deltaTime;

        timer = Mathf.Clamp(timer, 0f, Mathf.Infinity); 

        //waveDelayText.text = string.Format("{0:00.00}", timer); 
    }

    IEnumerator SpawnWave()
    {
        //PlayerStats.Rounds++; 

        Wave wave = waves[waveIndex];

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy, wave.spawnPoint);
            yield return new WaitForSeconds(1f / wave.rate); 
        }
            
        waveIndex++;
    }

    void SpawnEnemy (GameObject enemy, Transform spawnPoint)
    {
        Instantiate(enemy, 
            spawnPoint.position, 
            spawnPoint.rotation);

        EnemiesAlive++; 

        //Cuando un enemigo muere tengo que restarle EnemiesAlive--; 
    }
}
