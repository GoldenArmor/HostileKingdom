using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorGroup : MonoBehaviour
{
    [SerializeField]
    GameObject warriorPrefab; 
    List<Ally> warriors = new List<Ally>();
    public Transform spawnPoint;
    float spawnPointArea;

    [SerializeField]
    float spawnCooldown;
    float currentSpawnCooldown;
    bool lessThanThree;

    void Update()
    {
        if (lessThanThree)
        {
            currentSpawnCooldown -= Time.deltaTime; 
            if (currentSpawnCooldown <= 0)
            {
                currentSpawnCooldown = spawnCooldown;
                lessThanThree = false; 
            }
        }
    }

    public void CreateUnit()
    {
        ObjectPoolingManager.AllyPool.GetObject(warriorPrefab, transform); 
    }

    public void ClearUnit(Ally warrior)
    {
        warriors.Remove(warrior);
        lessThanThree = true;  
    }
}
