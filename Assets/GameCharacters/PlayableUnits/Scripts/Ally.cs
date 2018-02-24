using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Characters
{
    [Header("Movement")]
    [SerializeField]
    Transform objective;

    protected override void MyUpdate()
    {
        if (selectedTarget != null)
        {
            //if(selectedTarget.isBeingAttacked)
            //{
            //    Debug.Log("Change Target");
            //    ClearTarget();
            //    return; 
            //}
            CalculateDistanceFromTarget();
        }
        else
        {
            if (unitsCanAttack.Count > 0)
            {
                FindClosestObject();
            }
        }
        base.MyUpdate(); 
    }

    #region Updates
    protected override void IdleUpdate()
    {
        if (selectedTarget != null)
        {
            if (distanceFromTarget > attackRange)
            {
                SetChase();
                return;
            }

            LookAtTarget();
            timeCounter -= Time.deltaTime;
            if (selectedTarget.isDead)
            {
                ClearUnit();
                return;
            }
            if (timeCounter <= 0)
            {
                SetAttack();
            }
        }
    }

    protected override void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            SetMovement();
        }
    }

    protected override void ChaseUpdate()
    {
        agent.SetDestination(targetTransform.position);

        if (distanceFromTarget <= attackRange)
        {
            SetAttack();
        }
        //else
        //{
        //    SetIdle();
        //}
    }

    protected override void AttackUpdate()
    {
        selectedTarget.TakeDamage(attack, this, true);

        SetIdle();
    }
    #endregion

    #region Sets
    protected override void SetIdle()
    {
        timeCounter = attackCooldown;
        base.SetIdle();
    }

    protected override void SetAttack()
    {
        base.SetAttack();
    }

    public override void SetDead()
    {
        //enemiesManager.enemiesCount.Remove(this);
        base.SetDead();
    }

    public override void SetMovement()
    {
        agent.SetDestination(SetDestination(objective));
        base.SetMovement();
    }

    protected Vector3 SetDestination(Transform newDestination)
    {
        return newDestination.position;       
    }
    #endregion

    #region OnTriggerVoids
    void OnTriggerEnter(Collider other) //If a unit enters the collider, it's added to the interactive units list.
    {
        if (other.CompareTag("Enemy"))
        {
            unitsCanAttack.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other) //Units which leave the collider are deleted from the interactive units list. 
    {
        if (other.CompareTag("Enemy"))
        {
            unitsCanAttack.Remove(other.transform);
            //targetTransform = null;
            //selectedTarget = null;
            //distanceFromTarget = Mathf.Infinity;
        }
    }
    #endregion

    void ClearUnit()
    {
        unitsCanAttack.Remove(selectedTarget.transform);
        distanceFromTarget = Mathf.Infinity;
        //if (selectedTarget.isDead == false) selectedTarget.SetDead();
        targetTransform = null;
        selectedTarget = null;
        closestObject = null;
        SetIdle();
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}
