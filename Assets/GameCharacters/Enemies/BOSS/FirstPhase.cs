using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FirstPhase : BossPhase
{
    //[Header("AreaAttack")]
    //[SerializeField]
    //Transform skillCircle;
    //[SerializeField]
    //float skillCircleRadius;
    //SpriteRenderer circleRenderer;

    //[Header("SweepAttack")]
    ////[SerializeField]
    ////Collider attackArea;
    //[SerializeField]
    //Transform attackPosition;
    //[SerializeField]
    //float attackDuration;
    //float iniAttackDuration; 

    //[SerializeField]
    //LayerMask attackMask;


    //Collider[] hitColliders;

    //public override void InternalStart()
    //{
    //    base.InternalStart();
    //    boss.agent = GetComponent<NavMeshAgent>();
    //    circleRenderer = skillCircle.GetComponent<SpriteRenderer>();
    //    iniAttackDuration = attackDuration; 
    //}

    //public override void InternalUpdate()
    //{
    //    base.InternalUpdate();
    //}

    //protected override void Firstbehaviour()
    //{
    //    base.Firstbehaviour();

    //    circleRenderer.enabled = true;
    //    hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, attackMask);
    //    for (int i = 0; i < hitColliders.Length; i++)
    //    {
    //        PlayableUnitBehaviour attackedTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
    //        attackedTarget.TakeDamage(boss.attack * 5, boss);
    //        if (attackedTarget.currentHitPoints <= 0)
    //        {
    //            TargetDies(attackedTarget);
    //            return;
    //        }
    //    }

    //    hitColliders = null;
    //    //canAttack = true;
    //    circleRenderer.enabled = false;
    //}

    //protected override void SecondBehaviour()
    //{
    //    base.SecondBehaviour();
    //    cooldown = 10;
    //    boss.agent.SetDestination(attackPosition.position);

    //    if (Vector3.Distance(boss.transform.position, boss.agent.destination) < boss.agent.stoppingDistance)
    //    {
    //        attackDuration -= Time.deltaTime; 

    //        boss.agent.isStopped = true;
    //        boss.transform.localRotation = Quaternion.Slerp(Quaternion.Euler(boss.transform.localRotation.x, boss.transform.localRotation.y, boss.transform.localRotation.z),
    //            Quaternion.Euler(boss.transform.localRotation.x, 90, boss.transform.localRotation.z), attackDuration); 

    //    }
    //    //else
    //    //{
    //    //    boss.agent.SetDestination(attackPosition.position);
    //    //}

    //    if (attackDuration <= 0)
    //    {
    //        canAttack = true;
    //        attackDuration = iniAttackDuration; 
    //    }
    //}

    //void OnCollisionEnter(Collision collision)
    //{
    //    for (int i = 0; i < hitColliders.Length; i++)
    //    {
    //        PlayableUnitBehaviour attackedTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
    //        attackedTarget.TakeDamage(boss.attack * 5, boss);
    //        if (attackedTarget.currentHitPoints <= 0)
    //        {
    //            TargetDies(attackedTarget);
    //            return;
    //        }
    //    }
    //}

    //protected override void ThirdBehaviour()
    //{
    //    base.ThirdBehaviour();

    //    circleRenderer.enabled = true;
    //    hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, attackMask);
    //    for (int i = 0; i < hitColliders.Length; i++)
    //    {
    //        PlayableUnitBehaviour attackedTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
    //        attackedTarget.TakeDamage(boss.attack * 5, boss);
    //        if (attackedTarget.currentHitPoints <= 0)
    //        {
    //            TargetDies(attackedTarget);
    //            return;
    //        }
    //    }

    //    hitColliders = null;
    //    //canAttack = true;
    //    circleRenderer.enabled = false;
    //}

    //protected override void SetSecond()
    //{
    //    base.SetSecond();
    //    //boss.agent.SetDestination(attackPosition.position);
    //    //boss.agent.destination = attackPosition.position;
    //    boss.transform.localRotation = Quaternion.Euler(boss.transform.localRotation.x, -90, boss.transform.localRotation.z);
    //}

    //void TargetDies(PlayableUnitBehaviour attackedTarget)
    //{
    //    if (attackedTarget.isDead == false)
    //    {
    //        attackedTarget.SetDead();
    //    }
    //    attackedTarget = null;
    //    return;
    //}

    //protected override int RandomBehaviour()
    //{
    //    return base.RandomBehaviour();
    //}
}
