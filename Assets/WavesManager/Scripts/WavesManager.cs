using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WavesManager : MonoBehaviour
{
    public static int enemiesAlive = 0; 

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

    void Update()
    {
        if (enemiesAlive > 0)
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
            SpawnUnit(wave.character, spawnPoint);
            yield return new WaitForSeconds(1f / wave.rate); 
        }
            
        waveIndex++;
    }

    void SpawnUnit (GameObject unitPrefab, Transform spawnPoint)
    {
        Characters character = ObjectPoolingManager.Instance.CharacterPool.GetObject(unitPrefab, spawnPoint);

        enemiesAlive++; 

        //Cuando un enemigo muere tengo que restarle EnemiesAlive--; 
    }
}
