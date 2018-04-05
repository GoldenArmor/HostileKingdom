using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IPooledObject
{
    [SerializeField]
    SpawnScale spawnScale;

    [HideInInspector]
    public bool isSelected;

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
        if (isSelected)
        {
            SelectedUpdate(); 
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }

    public virtual void Sell()
    {
    }

    protected virtual void SelectedUpdate()
    {
    }

    public virtual void EndSell() 
    {
        spawnScale.ResetEasing();
        gameObject.SetActive(false);
    }

    public virtual void Select()
    {
        isSelected = true; 
    }

    public virtual void Unselect()
    {
        isSelected = false; 
    }
}
