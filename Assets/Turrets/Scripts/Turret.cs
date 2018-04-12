using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, IPooledObject
{
    [HideInInspector]
    public bool isSelected;

    [SerializeField]
    RangeCircleScale rangeCircle;

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
        rangeCircle.ResetEasing();
        rangeCircle.InstantEnd(); 
    }

    protected virtual void SelectedUpdate()
    {
    }

    public virtual void EndSell() 
    {
        gameObject.SetActive(false);
    }

    public virtual void Select()
    {
        isSelected = true;
        rangeCircle.ResetEasing(); 
    }

    public virtual void Unselect()
    {
        isSelected = false;
        rangeCircle.ResetEasing();
    }
}
