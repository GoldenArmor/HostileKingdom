using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IPooledObject
{
    [SerializeField]
    SpawnScale spawnScale; 

    public void PooledAwake()
    {
        gameObject.SetActive(true); 
    }

    public void PooledStart()
    {

    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }

    public void Sell()
    {
        gameObject.SetActive(false);
        spawnScale.ResetEasing();
    }
}
