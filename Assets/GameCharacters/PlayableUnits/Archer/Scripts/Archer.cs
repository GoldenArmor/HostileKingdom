using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayableUnitBehaviour
{
    //[Header("Skill")]
    //[SerializeField]
    //float selectTarget;
    //float currentSelectTarget;
    //public bool isDoingSkill;

    //[SerializeField]
    //Transform skillCircle;
    //float maxDistanceHero = Mathf.Infinity;

    //[SerializeField]
    //LayerMask attackMask;
    //[SerializeField]
    //LayerMask circleMask; 
    //[SerializeField]
    //float skillCircleRadius;
    //SpriteRenderer circleRenderer; 

    //Collider[] hitColliders;

    //public bool isUpdatingCirclePosition;

    //void Start()
    //{
    //    MyStart();
    //    currentSelectTarget = selectTarget;
    //    circleRenderer = skillCircle.GetComponent<SpriteRenderer>();
    //}

    //void Update()
    //{
    //    MyUpdate();

    //    if (isDoingSkill)
    //    {
    //        SkillUpdate();
    //        return;
    //    }
    //    if (isUpdatingCirclePosition)
    //    {
    //        mouse.isDoingSkill = true;
    //        circleRenderer.enabled = true;
    //        CirclePositionUpdate();
    //    }
    //    else
    //    {
    //        circleRenderer.enabled = false;
    //    }
    //}

    //void CirclePositionUpdate()
    //{
    //    Ray ray = mainCamera.ScreenPointToRay(InputManager.mousePosition);

    //    RaycastHit hit;
    //    if (Physics.Raycast(ray, out hit, maxDistanceHero, circleMask))
    //    {
    //        skillCircle.position = new Vector3(hit.point.x, skillCircle.position.y, hit.point.z);
    //    }
    //}

    //void SelectTarget()
    //{
    //    currentSelectTarget -= Time.deltaTime;
    //    if (currentSelectTarget < 0)
    //    {
    //        SkillUpdate();
    //    }
    //}

    //void SkillUpdate()
    //{
    //    hitColliders = Physics.OverlapSphere(skillCircle.position, skillCircleRadius, attackMask);
    //    for (int i = 0; i < hitColliders.Length; i++)
    //    {
    //        EnemyBehaviour attackedTarget = hitColliders[i].GetComponent<EnemyBehaviour>();
    //        attackedTarget.TakeDamage(attack * 5, this);
    //        if (attackedTarget.currentHitPoints <= 0)
    //        {
    //            EnemyDies(attackedTarget);
    //            return;
    //        }
    //    }
    //    hitColliders = null;
    //    currentSelectTarget = selectTarget;
    //    isDoingSkill = false;
    //    isUpdatingCirclePosition = false;
    //}

    //void EnemyDies(EnemyBehaviour attackedTarget)
    //{
    //    if (attackedTarget.isDead == false)
    //    {
    //        attackedTarget.enemiesManager.enemiesCount.Remove(attackedTarget);
    //        attackedTarget.SetDead();
    //    }
    //    attackedTarget = null;
    //    isAttacking = false;
    //    return;
    //}

    //public override void SetDead()
    //{
    //    circleRenderer.enabled = false;
    //    base.SetDead();
    //}

    //void OnDrawGizmos()
    //{
    //    Color newColor = Color.magenta;
    //    newColor.a = 0.2f;
    //    Gizmos.color = newColor;
    //    Gizmos.DrawSphere(skillCircle.position, skillCircleRadius);
    //}
}
