using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTurret : Turret
{
    [SerializeField]
    WarriorGroup myGroup;
    [SerializeField]
    SpawnScalePingPong selectionCircleAnim;
    [SerializeField]
    ParticleSystem[] selectionCirclePart;

    public override void PooledStart()
    {
        base.PooledStart();
        myGroup.PooledStart();
        //selectionCircleAnim.ResetEasing(); 
    }

    public override void Select()
    {
        if(!isSelected) selectionCircleAnim.ResetEasing();
        for (int i = 0; i < selectionCirclePart.Length; i++)
        {
            selectionCirclePart[i].Play(); 
        }
        base.Select();        
    }

    public override void Unselect()
    {
        if (isSelected) selectionCircleAnim.ResetEasing();
        for (int i = 0; i < selectionCirclePart.Length; i++)
        {
            selectionCirclePart[i].Stop();
        }
        base.Unselect();
    }

    public override void Sell()
    {
        base.Sell(); 
        myGroup.Sell(); 
    }
}
