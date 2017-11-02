using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : UnitStats
{
    //public Animator anim;

    [Header("Timers")]
    public float cooldownAttack;
    public float timeCounter;

    [Header("UnitsCanAttack")]
    public List<GameObject> unitsCanAttack = new List<GameObject>();
    private GameObject closestObject;
    private bool canAttack;

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        if (selectedTarget == null && unitsCanAttack.Count > 0)
        {
            if (unitsCanAttack.Count > 1)
            {
                FindClosestObject();
                return;
            }
            selectedTarget = unitsCanAttack[0].transform.gameObject;
            targetTransform = selectedTarget.transform;
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
        }
    }

    public override void MoveUpdate()
    {

    }

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
        if (targetTransform.GetComponent<PlayableUnitBehaviour>().hitPoints <= 0)
        {
            UnitDies(); 
        }
        else if (canAttack)
        {        
            targetTransform.GetComponent<PlayableUnitBehaviour>().TakeDamage(attack, this.gameObject);

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
    #endregion

    #region CalculationVoids
    void FindClosestObject()
    {
        for (int i = 0; i < unitsCanAttack.Count; i++)
        {
            if (closestObject != null)
            {
                if (Vector3.Distance(this.transform.position, unitsCanAttack[i].transform.position) <=
                Vector3.Distance(this.transform.position, closestObject.transform.position))
                {
                    closestObject = unitsCanAttack[i];
                }
            }
            else closestObject = unitsCanAttack[i];

        }
        selectedTarget = closestObject;
        targetTransform = selectedTarget.transform;
    }
    #endregion

    #region OnTriggerVoids
    void OnTriggerEnter(Collider other) //If a unit enters the collider, it's added to the interactive units list.
    {
        if (other.tag == "PlayableUnit")
        {
            unitsCanAttack.Add(other.transform.gameObject);
        }
    }

    void OnTriggerExit(Collider other) //Units which leave the collider are deleted from the interactive units list. 
    {
        if (other.tag == "PlayableUnit" && unitsCanAttack != null)
        {
            unitsCanAttack.Remove(other.transform.gameObject);
            targetTransform = null;
            selectedTarget = null;
            distanceFromTarget = Mathf.Infinity;
        }
    }
    #endregion

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

        //if (lifeBar.selectedUnit == this.gameObject) lifeBar.UpdateBar(hitPoints);
    }

    void UnitDies()
    {
        targetTransform.GetComponent<PlayableUnitBehaviour>().SetDead();
        unitsCanAttack.Remove(selectedTarget);
        targetTransform = null;
        selectedTarget = null;
        distanceFromTarget = Mathf.Infinity;
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
