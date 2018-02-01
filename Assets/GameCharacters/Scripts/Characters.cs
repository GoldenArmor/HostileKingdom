using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Characters : MonoBehaviour
{
    protected Animator anim;
    AudioManager audioManager; 

    [HideInInspector] public enum UnitState { Idle, Movement, Chase, Attack, Stun, Dead }
    public UnitState state;

    [Header("Stats")]
    public float startingHitPoints;
    public float hitPoints;
    public float armor;
    public float attack;
    [SerializeField]
    float magicAttack;
    [SerializeField]
    float magicArmor;
    public float attackRange;
    public string characterName;
    float rotateSpeed = 125f;
    float footstepsCounter;
    int randomAudioClip; 
    [HideInInspector] public bool isDead;

    [Header("CharactersInteraction")]
    public Transform targetTransform;
    public float distanceFromTarget = Mathf.Infinity;

    [Header("NavMeshAgent")]
    protected NavMeshAgent agent;

    public LayerMask mask; 

    protected virtual void MyStart()
    {
        anim = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>(); 
        agent = GetComponent<NavMeshAgent>();
        hitPoints = startingHitPoints;
   
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
        PlayFootsteps(); 
    }

    protected virtual void AttackUpdate()
    {

    }

    protected virtual void StunUpdate()
    {

    }

    void DeadUpdate()
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

    protected virtual void SetMovement()
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
        hitPoints = 0;
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
    #endregion

    protected void PlayFootsteps()
    {
        footstepsCounter += Time.deltaTime;
        randomAudioClip = UnityEngine.Random.Range(0, 2); 
        if (footstepsCounter >= audioManager.sounds[randomAudioClip].clip.length) audioManager.sounds[randomAudioClip].playingSound = false;
        if (audioManager.sounds[randomAudioClip].playingSound == false)
        {
            audioManager.Play(randomAudioClip); 
            footstepsCounter = 0; 
        }
    }
}
