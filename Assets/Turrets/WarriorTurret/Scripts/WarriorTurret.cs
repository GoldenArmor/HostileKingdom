using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorTurret : Turret
{
    [SerializeField]
    WarriorGroup myGroup;

    public override void PooledStart()
    {
        base.PooledStart();
        myGroup.PooledStart(); 
    }

    public override void Sell()
    {
        base.Sell(); 
        myGroup.Sell(); 
    }
}
