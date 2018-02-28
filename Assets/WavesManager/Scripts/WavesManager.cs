using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviour
{
    [Header("Wave Properties")]
    [SerializeField]
    Wave[] waves;

    [Header("Wave Control")]
    [SerializeField]
    Transform spawnPoint; 
    [SerializeField]
    float waveDelay;
    [SerializeField]
    Text waveDelayText; 
    float timer;
    int waveIndex;

    Wave currentWave; 
    bool isSpawning;
    float spawnCounter;
    int spawnedEnemyIndex; 

    void Update()
    {
        if (isSpawning)
        { 
            SpawnWave();
        }

        if (EnemyWaveManager.enemiesAlive > 0)
        {
            //Debug.Log(EnemyWaveManager.enemiesAlive);
            return;
        }

        if (timer <= 0)
        {
            isSpawning = true;
            currentWave = waves[waveIndex];
            spawnedEnemyIndex = currentWave.count;
            timer = waveDelay;
            return; 
        }

        timer -= Time.deltaTime;

        timer = Mathf.Clamp(timer, 0f, Mathf.Infinity); 

        //waveDelayText.text = string.Format("{0:00.00}", timer); 
    }

    void SpawnWave()
    {
        //PlayerStats.Rounds++;  

        spawnCounter -= Time.deltaTime; 

        if (spawnCounter <= 0)
        {
            SpawnUnit(currentWave.character, spawnPoint);
            spawnedEnemyIndex--; 

            spawnCounter = 1f / currentWave.rate; 
            if (spawnedEnemyIndex <= 0)
            {
                isSpawning = false; 
                waveIndex++;
                if (waveIndex > waves.Length - 1)
                {
                    Debug.Log("LEVEL WON!");
                    enabled = false; 
                }
            }
        }


        //for (int i = 0; i < wave.count; i++)
        //{
        //    SpawnUnit(wave.characterTag, spawnPoint);
        //    yield return new WaitForSeconds(1f / wave.rate);
        //}
    }

    void SpawnUnit (GameObject character, Transform spawnPoint)
    {
        ObjectPoolingManager.Instance.CharacterPool.GetObject(character, spawnPoint); 

        EnemyWaveManager.enemiesAlive++; 

        //Cuando un enemigo muere tengo que restarle EnemiesAlive--; 
    }
}
