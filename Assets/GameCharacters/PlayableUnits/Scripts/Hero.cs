using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Hero : PlayableUnitBehaviour
{
    [Header("Skills")]
    bool isDoingSkill;

    [SerializeField]
    Transform skillCircle;

    [SerializeField]
    LayerMask attackMask;
    [SerializeField]
    float skillCircleRadius;
    SpriteRenderer circleRenderer;

    Collider[] hitColliders;

    void Start()
    {
        UnitStart();
        circleRenderer = skillCircle.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDoingSkill)
        {
            SkillUpdate();
            return;
        }
        UnitUpdate();
    }

    public void SkillUpdate()
    {
        circleRenderer.enabled = true;
        hitColliders = Physics.OverlapSphere(transform.position, skillCircleRadius, attackMask); 
        for (int i = 0; i < hitColliders.Length; i++)
        {
            EnemyBehaviour attackedTarget = hitColliders[i].GetComponent<EnemyBehaviour>();
            attackedTarget.TakeDamage(attack*5);
            if (attackedTarget.hitPoints <= 0)
            {
                EnemyDies(attackedTarget);
                return;
            }
        }
        hitColliders = null;
        isDoingSkill = false;
        circleRenderer.enabled = false; 
    }

    void EnemyDies(Characters attackedTarget)
    {
        if (attackedTarget.isDead == false) attackedTarget.SetDead();
        attackedTarget = null;
        isAttacking = false;
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
