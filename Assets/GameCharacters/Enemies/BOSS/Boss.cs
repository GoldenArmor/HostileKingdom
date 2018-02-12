using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : EnemyBehaviour
{
    [Header("Stats")]
    [SerializeField]
    float startingLife;
    public float attack; 
    float currentLife;

    [Header("Boss Phases")]
    BossPhase currentBossPhase;

    void Start()
    {
        currentBossPhase.InternalStart();
    }

    void Update()
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
