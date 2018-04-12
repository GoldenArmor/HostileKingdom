using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTurret : Turret
{
    [SerializeField]
    WarriorGroup myGroup;
    [SerializeField]
    RangeCircleScale selectionCircleAnim;
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
        selectionCircleAnim.ResetEasing();
        for (int i = 0; i < selectionCirclePart.Length; i++)
        {
            selectionCirclePart[i].Play(); 
        }
        base.Select();        
    }

    public override void Unselect()
    {
        selectionCircleAnim.ResetEasing();
        for (int i = 0; i < selectionCirclePart.Length; i++)
        {
            selectionCirclePart[i].Stop();
        }
        base.Unselect();
    }

    public override void Sell()
    {
        selectionCircleAnim.ResetEasing();
        selectionCircleAnim.InstantEnd();
        for (int i = 0; i < selectionCirclePart.Length; i++)
        {
            selectionCirclePart[i].Stop();
        }
        base.Sell();
        myGroup.Sell(); 
    }
}
