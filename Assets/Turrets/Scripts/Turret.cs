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

    void Update()
    {
        MyUpdate();     
    }

    protected virtual void MyUpdate()
    {
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }

    public void Sell()
    {
        spawnScale.ResetEasing();
        transform.localScale = Vector3.zero; 
        gameObject.SetActive(false);
    }
}
