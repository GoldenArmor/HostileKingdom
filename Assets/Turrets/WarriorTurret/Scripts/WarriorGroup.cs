using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorGroup : MonoBehaviour
{
    [SerializeField]
    WarriorTurret myTurret; 
    [SerializeField]
    GameObject warriorPrefab; 
    [SerializeField]
    List<Ally> warriors = new List<Ally>();

    public Transform spawnPoint; 
    public Transform patrolPoint;
    float spawnPointArea;

    [SerializeField]
    float spawnCooldown;
    float currentSpawnCooldown;

    bool lessThanThree;

    public void PooledStart()
    {
        CreateUnit();
        CreateUnit();
        CreateUnit();
    }

    void Update()
    {
        if (lessThanThree)
        {
            currentSpawnCooldown -= Time.deltaTime; 
            if (currentSpawnCooldown <= 0)
            {
                CreateUnit();
                currentSpawnCooldown = spawnCooldown;
                if (warriors.Count == 3) lessThanThree = false; 
            }
        }
    }

    public void CreateUnit()
    {
        Ally newAlly = ObjectPoolingManager.AllyPool.GetObject(warriorPrefab, spawnPoint);
        warriors.Add(newAlly);
        newAlly.TurretStart(this); 
    }

    public void ClearUnit(Ally warrior)
    {
        warriors.Remove(warrior);
        lessThanThree = true;  
    }

    public void Sell()
    {
        for (int i = 0; i < warriors.Count; i++)
        {
            warriors[i].SetDead();
            Debug.Log(warriors[i]); 
        }
        myTurret.EndSell(); 
    }
}
