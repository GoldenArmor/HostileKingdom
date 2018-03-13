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

    public virtual void PooledStart()
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

    public virtual void Sell()
    {
    }

    public virtual void EndSell() 
    {
        spawnScale.ResetEasing();
        gameObject.SetActive(false);
    }
}
