using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Characters
{
    //public Animator anim;
    public EnemiesManager enemiesManager;

    [Header("Movement")]
    [SerializeField]
    Transform objective;

    [SerializeField]
    float maxDistanceAttack; 

    [Header("UnitsCanAttack")]  
    bool canAttack;

    protected override void MyUpdate()
    {
        if (selectedTarget != null)
        {
            CalculateDistanceFromTarget();
            //if (!selectedTarget.isActiveAndEnabled)
            //{
            //    ClearUnit();
            //}
        }
        else
        {
            if (unitsCanAttack.Count > 0)
            {
                FindClosestObject();
                return; 
            }

            SetMovement(); 
        }
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
            timeCounter += Time.deltaTime;
            if (selectedTarget.isDead)
            {
                ClearUnit();
                return;
            }
            if (timeCounter > attackCooldown)
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
        selectedTarget.TakeDamage(attack, this);

        SetIdle();
    }
    #endregion

    #region Sets
    protected override void SetIdle()
    {
        timeCounter = 0;
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
        agent.SetDestination(objective.position);
        base.SetMovement();
    }
    #endregion

    #region OnTriggerVoids
    void OnTriggerEnter(Collider other) //If a unit enters the collider, it's added to the interactive units list.
    {
        if (other.tag == "PlayableUnit")
        {
            unitsCanAttack.Add(other.transform);
        }
    }

    void OnTriggerExit(Collider other) //Units which leave the collider are deleted from the interactive units list. 
    {
        if (other.tag == "PlayableUnit")
        {
            unitsCanAttack.Remove(other.transform);
            //targetTransform = null;
            //selectedTarget = null;
            //distanceFromTarget = Mathf.Infinity;
        }
    }
    #endregion

    //public override void TakeDamage(float damage, Characters attacker)
    //{
    //    base.TakeDamage(damage, attacker);
    //}

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
