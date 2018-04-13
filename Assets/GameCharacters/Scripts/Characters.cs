using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Characters : MonoBehaviour, IPooledObject
{
    [HideInInspector] public enum UnitState { Idle, Movement, Chase, Attack, Stun, Dead }
    public UnitState state;

    [Header("Stats")]
    public float startingHitPoints;
    float currentHitPoints;
    public float armor;
    public float attack;
    [SerializeField]
    float magicAttack;
    [SerializeField]
    float magicArmor;
    public float attackRange;
    [SerializeField]
    protected float attackCooldown;
    public string characterName;
    public bool isBeingAttacked;

    [Header("LifeBar")]
    [SerializeField]
    Image lifeBar; 

    [HideInInspector] public bool isDead;

    [Header("Timers")]
    protected float timeCounter;

    [Header("Sounds")]
    float footstepsCounter;
    int randomAudioClip; 

    [Header("CharactersInteraction")]
    [SerializeField]
    protected List<Transform> unitsCanAttack = new List<Transform>();
    protected Transform closestObject;
    protected Characters selectedTarget;
    protected Transform targetTransform;
    protected float distanceFromTarget = Mathf.Infinity;
    float rotateSpeed = 125f;

    [Header("NavMeshAgent")]
    public NavMeshAgent agent;

    [Header("Animator")]
    [SerializeField]
    protected Animator anim;

    [Header("Movement")]
    public Transform objective;

    public LayerMask mask;

    void Update()
    {
        MyUpdate();     
    }

    protected virtual void MyStart()
    {
        currentHitPoints = startingHitPoints;
   
        SetIdle();
    }

    protected virtual void MyUpdate()
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
            case UnitState.Stun:
                StunUpdate();
                break;
            case UnitState.Dead:
                DeadUpdate();
                break;
            default:
                break;
        }
    }

    #region Updates
    protected virtual void IdleUpdate()
    {

    }

    protected virtual void MoveUpdate()
    {

    }

    protected virtual void ChaseUpdate()
    {
        //PlayFootsteps(); 
    }

    protected virtual void AttackUpdate()
    {

    }

    protected virtual void StunUpdate()
    {

    }

    protected virtual void DeadUpdate()
    {
        //SetDead();
    }
    #endregion

    #region Sets
    protected virtual void SetIdle()
    {
        agent.isStopped = true;
        anim.SetBool("IsMoving", false); 
        anim.SetTrigger("Idle");
        state = UnitState.Idle;
    }

    public virtual void SetMovement()
    {
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        state = UnitState.Movement;
    }

    protected void SetChase()
    {
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        state = UnitState.Chase;
    }

    protected virtual void SetAttack()
    {
        agent.isStopped = true;
        anim.SetBool("IsMoving", true);
        anim.SetTrigger("Attack");
        state = UnitState.Attack;
    }

    protected virtual void SetStun()
    {
        agent.isStopped = true;
        anim.SetTrigger("Stun");
        state = UnitState.Stun;
    }

    public virtual void SetDead()
    {
        isDead = true;
        currentHitPoints = 0;
        agent.isStopped = true;
        anim.SetTrigger("Die");
        state = UnitState.Dead;

        gameObject.SetActive(false);
    }
    #endregion

    #region CalculationVoids
    protected virtual void LookAtTarget()
    {
        Vector3 lookDir = targetTransform.position - transform.position;
        Quaternion q = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }

    protected virtual void FindClosestObject()
    {
        for (int i = 0; i < unitsCanAttack.Count; i++)
        {
            if (closestObject != null)
            {
                if (Vector3.Distance(transform.position, unitsCanAttack[i].position) <=
                Vector3.Distance(transform.position, closestObject.position))
                {
                    closestObject = unitsCanAttack[i];
                }
            }
            else closestObject = unitsCanAttack[i];
            selectedTarget = closestObject.GetComponent<Characters>();
            targetTransform = closestObject;
        }
    }

    protected virtual void CalculateDistanceFromTarget() //Calculates the distance between the Unit and the Selected enemy. 
    {
        targetTransform = selectedTarget.transform;
        distanceFromTarget = Vector3.Distance(transform.position, targetTransform.position);
    }

    void UpdateLifebar()
    {
        lifeBar.fillAmount = currentHitPoints / startingHitPoints;
    }
    #endregion

    #region PublicVoids
    public virtual void TakeDamage(float damage, Characters attacker, bool isAttacked)
    {
        currentHitPoints -= damage;

        UpdateLifebar();

        if (currentHitPoints <= 0)
        {
            attacker.ClearTarget();
           if (!isDead) SetDead(); 
        }

        //isBeingAttacked = isAttacked; 
    }

    public virtual void TakeDamage(float damage, bool isAttacked)
    {
        currentHitPoints -= damage;

        UpdateLifebar();

        if(currentHitPoints <= 0)
        {
            if(!isDead) SetDead();
        }

        //isBeingAttacked = isAttacked; 
    }

    public virtual void ClearTarget()
    {
        unitsCanAttack.Remove(selectedTarget.transform);
        distanceFromTarget = Mathf.Infinity;
        //if (selectedTarget.isDead == false) selectedTarget.SetDead();
        targetTransform = null;
        selectedTarget = null;
        closestObject = null;
        SetIdle();
    }

    public void PooledAwake()
    {
        gameObject.SetActive(true);
    }

    public void PooledStart()
    {
        MyStart();
        UpdateLifebar();
    }
    
    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }
    #endregion

    protected virtual void PlayFootsteps()
    {
        //footstepsCounter += Time.deltaTime;
        //randomAudioClip = UnityEngine.Random.Range(0, 2); 
        //if (footstepsCounter >= AudioManager.Play() audioManager.sounds[randomAudioClip].playingSound = false;
        //if (audioManager.sounds[randomAudioClip].playingSound == false)
        //{
        //    audioManager.Play(randomAudioClip); 
        //    footstepsCounter = 0; 
        //}
    }
}
