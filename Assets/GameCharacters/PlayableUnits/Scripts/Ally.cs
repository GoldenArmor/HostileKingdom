using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Characters
{
    //[HideInInspector]
    //public WarriorGroup myGroup;

    //[SerializeField]
    //SpawnScalePingPong spawnScale; 

    //Transform spawnPoint; 
    //[SerializeField]
    //float maxDistanceFromSpawnPoint;
    //bool isMoving;

    //public void TurretStart(WarriorGroup newGroup)
    //{
    //    myGroup = newGroup; 
    //    spawnPoint = myGroup.patrolPoint;
    //    objective = spawnPoint; 
    //    spawnScale.ResetEasing();
    //    SetMovement(); 
    //}

    //protected override void MyUpdate()
    //{
    //    base.MyUpdate(); 
    //}

    //#region Updates
    //protected override void IdleUpdate()
    //{
    //    if (selectedTarget != null)
    //    {
    //        if (distanceFromTarget > attackRange)
    //        {
    //            SetChase();
    //            return;
    //        }

    //        LookAtTarget();
    //        timeCounter -= Time.deltaTime;
    //        if (selectedTarget.isDead)
    //        {
    //            ClearUnit();
    //            return;
    //        }
    //        if (timeCounter <= 0)
    //        {
    //            SetAttack();
    //        }
    //    }
    //    else
    //    {
    //        if (Vector3.Distance(transform.position, agent.destination) > agent.stoppingDistance)
    //        {
    //            objective = spawnPoint;
    //            SetMovement();
    //        }
    //    }
    //}

    //protected override void MoveUpdate()
    //{
    //    if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance || selectedTarget != null)
    //    {
    //        SetIdle();  
    //    }
    //}

    //protected override void AttackUpdate()
    //{
    //    SetIdle();
    //}
    //#endregion

    //#region Sets
    //protected override void SetIdle()
    //{
    //    timeCounter = attackCooldown;
    //    base.SetIdle();
    //}

    //protected override void SetAttack()
    //{
    //    base.SetAttack();
    //}

    //public override void SetDead()
    //{
    //    myGroup.ClearUnit(this);
    //    myGroup = null;
    //    spawnScale.ResetEasing();
    //    base.SetDead();
    //}

    //public override void SetMovement()
    //{
    //    agent.SetDestination(SetDestination(objective));
    //    base.SetMovement();
    //}

    //protected Vector3 SetDestination(Transform newDestination)
    //{
    //    return newDestination.position;       
    //}
    //#endregion

    //#region OnTriggerVoids
    //void OnTriggerEnter(Collider other) //If a unit enters the collider, it's added to the interactive units list.
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        unitsCanAttack.Add(other.transform);
    //        objective = transform; 
    //        SetIdle(); 
    //    }
    //}

    //void OnTriggerExit(Collider other) //Units which leave the collider are deleted from the interactive units list. 
    //{
    //    if (other.CompareTag("Enemy"))
    //    {
    //        unitsCanAttack.Remove(other.transform);
    //        //targetTransform = null;
    //        //selectedTarget = null;
    //        //distanceFromTarget = Mathf.Infinity;
    //    }
    //}
    //#endregion

    //void ClearUnit()
    //{
    //    unitsCanAttack.Remove(selectedTarget.transform);
    //    distanceFromTarget = Mathf.Infinity;
    //    //if (selectedTarget.isDead == false) selectedTarget.SetDead();
    //    targetTransform = null;
    //    selectedTarget = null;
    //    closestObject = null;
    //    SetIdle();
    //}

    //void OnDrawGizmos()
    //{
    //    Color newColor = Color.green;
    //    newColor.a = 0.2f;
    //    Gizmos.color = newColor;
    //    Gizmos.DrawSphere(transform.position, attackRange);

    //    Color seconDoclor = Color.blue;
    //    Gizmos.color = newColor;
    //    Gizmos.DrawWireSphere(spawnPoint.position, maxDistanceFromSpawnPoint); 
    //}
}
