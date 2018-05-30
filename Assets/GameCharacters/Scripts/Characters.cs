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
    [SerializeField]
    float currentHitPoints;
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
        isDead = false; 
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
    public virtual void SetMovement()
    {
        agent.isStopped = false;
        anim.SetBool("IsMoving", true);
        state = UnitState.Movement;
    }

    public virtual void SetDead()
    {
        isDead = true;
        currentHitPoints = 0;
        agent.isStopped = true;
        anim.SetTrigger("Die");
        state = UnitState.Dead;

        //gameObject.SetActive(false);
    }
    #endregion

    #region CalculationVoids
    void UpdateLifebar()
    {
        lifeBar.fillAmount = currentHitPoints / startingHitPoints;
    }
    #endregion

    #region PublicVoids
    public virtual void TakeDamage(float damage)
    {
        currentHitPoints -= damage;

        UpdateLifebar();

        if(currentHitPoints <= 0)
        {
            if(!isDead) SetDead();
        }

        //isBeingAttacked = isAttacked; 
    }

    public void PooledAwake()
    {
        gameObject.SetActive(true);
    }

    public virtual void PooledStart()
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
