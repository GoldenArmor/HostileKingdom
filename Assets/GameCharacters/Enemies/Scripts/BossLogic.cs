using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : EnemyBehaviour
{
    [Header("Skill")]
    [SerializeField]
    Transform skillCircle;

    bool isDoingSkill;

    [SerializeField]
    LayerMask attackMask;
    [SerializeField]
    float skillCircleRadius;
    SpriteRenderer circleRenderer;

    float skillCooldown; 

    Collider[] hitColliders;

    protected override void Start()
    {
        base.Start(); 
        circleRenderer = skillCircle.GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        skillCooldown -= Time.deltaTime; 

        if (selectedTarget != null && skillCooldown > 25)
        {
            skillCooldown = 0;
            SkillUpdate(); 
        }
        if (isDoingSkill)
        {
            SkillUpdate();
        }
        agent.isStopped = true;
        SetIdle();
    }

    public void SkillUpdate()
    {
        circleRenderer.enabled = true;
        hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, attackMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            PlayableUnitBehaviour attackedTarget = hitColliders[i].GetComponent<PlayableUnitBehaviour>();
            attackedTarget.PlayableUnitTakeDamage(attack * 5, this);
            if (attackedTarget.hitPoints <= 0)
            {
                UnitDies(attackedTarget);
                return;
            }
        }
        hitColliders = null;
        isDoingSkill = false;
        circleRenderer.enabled = false;
    }

    void UnitDies(PlayableUnitBehaviour attackedTarget)
    {
        if (attackedTarget.isDead == false)
        {
            attackedTarget.SetDead();
        }
        attackedTarget = null;
        return;
    }

    public override void SetDead()
    {
        circleRenderer.enabled = false;
        base.SetDead();
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.magenta;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, skillCircleRadius);
    }
}
