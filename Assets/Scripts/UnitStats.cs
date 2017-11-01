using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStats
{
    private enum UnitState { Idle, Movement, Chase, Attack, Dead }
    [SerializeField] private UnitState state;

    [Header("Stats")]
    public float hitPoints;
    public float armor;
    public float attack;
    public float magicAttack;
    public float magicArmor;
    public float scope;
    private float rotateSpeed = 125f;
    [SerializeField] private bool isDead = false;

    [Header("UnitsInteraction")]
    [SerializeField] private GameObject selectedTarget;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float distanceFromTarget = Mathf.Infinity;
    private LifebarBehaviour lifeBar;

    public virtual void Start()
    {

    }

    public virtual void Update()
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

    public virtual void DeadUpdate()
    {

    }
    #endregion
}
