using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBehaviour
{
    [Header("Stats")]
    [SerializeField]
    float startingLife; 
    float currentLife;

    [Header("Boss Phases")]
    BossPhase currentBossPhase;

    protected override void MyStart()
    {
        currentBossPhase.InternalStart();
    }

    protected override void MyUpdate()
    {
        UpdatePhase(currentBossPhase);
    }

    void UpdatePhase(BossPhase bossPhase)
    {
        bossPhase.InternalUpdate();
    }

    public void GoNext(BossPhase nextBossPhase)
    {
        if(nextBossPhase != null)
        {
            currentBossPhase = nextBossPhase;
        }
    }
}
