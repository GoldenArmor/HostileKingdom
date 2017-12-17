using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Characters, IEnemy
{
    //public Animator anim;
    public EnemiesManager enemiesManager; 

    [Header("Timers")]
    [SerializeField]
    float cooldownAttack;
    [SerializeField]
    float timeCounter;

    [Header("UnitsCanAttack")]
    [SerializeField]
    List<Transform> unitsCanAttack = new List<Transform>();
    Transform closestObject;
    PlayableUnitBehaviour selectedTarget;  
    bool canAttack;

    void Start()
    {
        MyStart();
    }

    void Update()
    {
        MyUpdate();
        if (selectedTarget == null && unitsCanAttack.Count > 0)
        {
            FindClosestObject();
        }
        
        if (selectedTarget != null)
        {
            CalculateDistanceFromTarget(); 
        }
    }

    #region Updates
    public override void IdleUpdate()
    {
        if (selectedTarget != null)
        {
            if (distanceFromTarget >= scope)
            {
                SetChase();
                return;
            }

            if (selectedTarget.hitPoints <= 0)
            {
                ClearUnit();
                return;
            }
            if (timeCounter >= cooldownAttack)
            {
                SetAttack();
                return;
            }
            else
            {
                LookAtTarget(); 
                timeCounter += Time.deltaTime;
            }

            if (!selectedTarget.isActiveAndEnabled)
            {
                ClearUnit();
            }
        }
    }

    /*public override void MoveUpdate()
    {

    }*/

    public override void ChaseUpdate()
    {
        if (selectedTarget != null)
        {
            agent.SetDestination(targetTransform.position);

            if (distanceFromTarget < scope)
            {
                SetAttack();
                return;
            }
        }
        else
        {
            SetIdle();
            return; 
        }
    }

    public override void AttackUpdate()
    {
        if (canAttack)
        {        
            selectedTarget.PlayableUnitTakeDamage(attack, this);

            SetIdle();
            return;
        }
    }
    #endregion

    #region Sets
    public override void SetIdle()
    {
        timeCounter = 0;
        base.SetIdle(); 
    }

    public override void SetAttack()
    {
        canAttack = true;
        base.SetAttack();
    }

    public override void SetDead()
    {
        enemiesManager.enemiesCount.Remove(this);
        base.SetDead();
    }
    #endregion

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
        if (other.tag == "PlayableUnit" && unitsCanAttack != null)
        {
            unitsCanAttack.Remove(other.transform);
            targetTransform = null;
            selectedTarget = null;
            distanceFromTarget = Mathf.Infinity;
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
        return;
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, scope);
    }
}
