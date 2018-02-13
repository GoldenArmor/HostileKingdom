using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPhase : BossPhase
{
    [Header("Skill")]
    [SerializeField]
    Transform skillCircle;

    [SerializeField]
    LayerMask attackMask;
    [SerializeField]
    float skillCircleRadius;
    SpriteRenderer circleRenderer;

    Collider[] hitColliders;

    public override void InternalStart()
    {
        base.InternalStart();
        circleRenderer = skillCircle.GetComponent<SpriteRenderer>();
    }

    public override void InternalUpdate()
    {
        base.InternalUpdate();
    }

    protected override void Firstbehaviour()
    {
        base.Firstbehaviour();

        circleRenderer.enabled = true;
        hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, attackMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            PlayableUnitBehaviour attackedTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
            attackedTarget.PlayableUnitTakeDamage(boss.attack * 5, boss);
            if (attackedTarget.hitPoints <= 0)
            {
                TargetDies(attackedTarget);
                return;
            }
        }

        hitColliders = null;
        canAttack = false;
        circleRenderer.enabled = false;
    }

    void TargetDies(PlayableUnitBehaviour attackedTarget)
    {
        if (attackedTarget.isDead == false)
        {
            attackedTarget.SetDead();
        }
        attackedTarget = null;
        return;
    }

    protected override int RandomBehaviour()
    {
        return base.RandomBehaviour();
    }
}
