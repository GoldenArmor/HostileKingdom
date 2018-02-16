using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Characters
{
    //public Animator anim;
    public EnemiesManager enemiesManager;

    [Header("Path")]
    [SerializeField]
    float idleTime;
    [SerializeField]
    Transform[] path;
    int pathIndex = 0;
    float patrolCounter;
    int idlePercent = 40;
    [SerializeField]
    float maxDistanceAttack; 

    [Header("Timers")]
    [SerializeField]
    float cooldownAttack;
    [SerializeField]
    float timeCounter;

    [Header("UnitsCanAttack")]
    [SerializeField]
    List<Transform> unitsCanAttack = new List<Transform>();
    Transform closestObject;
    protected PlayableUnitBehaviour selectedTarget;  
    bool canAttack;

    protected virtual void Start()
    {
        MyStart();
    }

    protected virtual void Update()
    {
        MyUpdate();
    }

    protected override void MyUpdate()
    {
        base.MyUpdate(); 

        if (selectedTarget != null)
        {
            CalculateDistanceFromTarget();
            if (!selectedTarget.isActiveAndEnabled)
            {
                ClearUnit();
            }
        }
        else
        {
            if (path.Length > 0)
            {
                patrolCounter += Time.deltaTime;
                if (patrolCounter >= idleTime)
                {
                    SetMovement();
                }
            }

            if (unitsCanAttack.Count > 0) FindClosestObject();
        }
    }

    #region Updates
    protected override void IdleUpdate()
    {
        if (selectedTarget != null && distanceFromTarget < maxDistanceAttack)
        {
            if (distanceFromTarget > attackRange && distanceFromTarget < maxDistanceAttack)
            {
                SetChase();
                return;
            }

            LookAtTarget();
            timeCounter += Time.deltaTime;
            if (selectedTarget.hitPoints <= 0)
            {
                ClearUnit();
                return;
            }
            if (timeCounter > cooldownAttack)
            {
                SetAttack();
            }
        }
    }

    protected override void MoveUpdate()
    {        
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            pathIndex++;
            if (pathIndex >= path.Length) pathIndex = 0;
            if (RandomIdle()) SetIdle();
            else SetMovement();
        }
    }

    protected override void ChaseUpdate()
    {
        base.ChaseUpdate();
        if (selectedTarget != null)
        {
            agent.SetDestination(targetTransform.position);

            if (distanceFromTarget <= attackRange)
            {
                SetAttack();
            }
        }
        else
        {
            SetIdle();
        }
    }

    protected override void AttackUpdate()
    {
        selectedTarget.PlayableUnitTakeDamage(attack, this);

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
        enemiesManager.enemiesCount.Remove(this);
        base.SetDead();
    }

    protected override void SetMovement()
    {
        agent.isStopped = false;
        if (path.Length > 0)
        {
            agent.SetDestination(path[pathIndex].position);
        }
        else SetIdle();
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

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    void ClearUnit()
    {
        unitsCanAttack.Remove(selectedTarget.transform);
        distanceFromTarget = Mathf.Infinity;
        if (selectedTarget.isDead == false) selectedTarget.SetDead();
        targetTransform = null;
        selectedTarget = null;
        closestObject = null; 
        SetIdle();
    }

    bool RandomIdle()
    {
        int random = Random.Range(0, 101); 
        if (random <= idlePercent) return true;
        return false;
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);
    }

    #region CalculationVoids
    void FindClosestObject()
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
        }
        selectedTarget = closestObject.GetComponent<PlayableUnitBehaviour>();
        targetTransform = closestObject;
    }

    void CalculateDistanceFromTarget() //Calculates the distance between the Unit and the Selected enemy. 
    {
        targetTransform = selectedTarget.transform;
        distanceFromTarget = Vector3.Distance(transform.position, targetTransform.position);
    }
    #endregion
}
