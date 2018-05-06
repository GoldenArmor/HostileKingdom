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
    Text waveDelayText; 
    float timer;
    int waveIndex;
    int waveTimerIndex; 

    Wave currentWave; 
    bool isSpawning;
    bool hasSpawnedLastFrame; 
    float spawnCounter;
    int spawnedEnemyIndex;

    [SerializeField]
    EnemyWaveManager ememyWaveManager; 

    void Start()
    {
        currentWave = waves[waveIndex];
        timer = currentWave.waveDelay;
        spawnedEnemyIndex = currentWave.count;
    }

    void Update()
    {
        if (isSpawning)
        { 
            SpawnWave();
        }

        //if (EnemyWaveManager.enemiesAlive > 0)
        //{
        //    //Debug.Log(EnemyWaveManager.enemiesAlive);
        //    return;
        //}

        timer -= Time.deltaTime;

        timer = Mathf.Clamp(timer, 0f, Mathf.Infinity);

        waveDelayText.text = string.Format("{0:00}", timer);

        if (timer <= 0)
        {
            isSpawning = true;
            hasSpawnedLastFrame = true;
            ememyWaveManager.ChangeWavesCount(waveIndex + 1); 
        }
        if (hasSpawnedLastFrame)
        {
            waveTimerIndex += 1;
            if (waveTimerIndex > waves.Length -1)
            {
                return; 
            }

            timer = waves[waveTimerIndex].waveDelay;
            hasSpawnedLastFrame = false; 
        }
    }

    void SpawnWave()
    {
        //PlayerStats.Rounds++;  

        spawnCounter -= Time.deltaTime; 

        if (spawnCounter <= 0)
        {
            SpawnUnit(currentWave.character, RandomTransform());
            spawnedEnemyIndex--; 

            spawnCounter = 1f / currentWave.rate; 
            if (spawnedEnemyIndex <= 0)
            {
                isSpawning = false; 
                waveIndex++;

                currentWave = waves[waveIndex];
                spawnedEnemyIndex = currentWave.count;

                if (waveIndex > waves.Length - 1)
                {
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
        ObjectPoolingManager.EnemyPool.GetObject(character, spawnPoint); 

        EnemyWaveManager.enemiesAlive++; 

        //Cuando un enemigo muere tengo que restarle EnemiesAlive--; 
    }

    Transform RandomTransform()
    {

        return currentWave.spawnPoint[Random.Range(0, currentWave.spawnPoint.Length)]; 
    }
}
