using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Characters : MonoBehaviour
{
    Animator anim; 

    [HideInInspector] public enum UnitState { Idle, Movement, Chase, Attack, Dead }
    public UnitState state;

    [Header("Stats")]
    public float startingHitPoints;
    public float hitPoints;
    public float armor;
    public float attack;
    public float magicAttack;
    public float magicArmor;
    public float scope;
    public string characterName;
    private float rotateSpeed = 125f;
    [HideInInspector] public bool isDead = false;

    [Header("CharactersInteraction")]
    public Transform targetTransform;
    public float distanceFromTarget = Mathf.Infinity;

    [Header("NavMeshAgent")]
    [HideInInspector] public NavMeshAgent agent;

    public LayerMask mask; 

    public virtual void MyStart()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        hitPoints = startingHitPoints;
   
        SetIdle();
    }

    public virtual void MyUpdate()
    {
        switch (state)
        {
            case UnitState.Idle:
                IdleUpdate();
                break;
            case UnitState.Movement:
                MoveUpdate();
                break;
            case UnitState.Chase:
                ChaseUpdate();
                break;
            case UnitState.Attack:
                AttackUpdate();
                break;
            case UnitState.Dead:
                DeadUpdate();
                break;
            default:
                break;
        }
    }

    #region Updates
    public virtual void IdleUpdate()
    {

    }

    public virtual void MoveUpdate()
    {

    }

    public virtual void ChaseUpdate()
    {

    }

    public virtual void AttackUpdate()
    {

    }

    void DeadUpdate()
    {
        //SetDead();
    }
    #endregion

    #region Sets
    public virtual void SetIdle()
    {
        agent.isStopped = true;
        anim.SetBool("IsMoving", false);
        anim.SetTrigger("Idle");
        state = UnitState.Idle;
    }

    public virtual void SetMovement()
    {
        agent.isStopped = false;
        anim.SetBool("Attack", false);
        anim.SetBool("IsMoving", true);
        state = UnitState.Movement;
    }

    public void SetChase()
    {
        agent.isStopped = false;
        anim.SetBool("Attack", false);
        anim.SetBool("IsMoving", true);
        state = UnitState.Chase;
    }

    public virtual void SetAttack()
    {
        agent.isStopped = true;
        anim.SetBool("Attack", true);
        state = UnitState.Attack;
    }

    public virtual void SetDead()
    {
        isDead = true;
        hitPoints = 0;
        agent.isStopped = true;
        anim.SetTrigger("Die");
        state = UnitState.Dead;

        gameObject.SetActive(false);
    }
    #endregion

    #region CalculationVoids
    public virtual void LookAtTarget()
    {
        Vector3 lookDir = targetTransform.position - transform.position;
        Quaternion q = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }
    #endregion

    #region TakeDamage

    #endregion
}
