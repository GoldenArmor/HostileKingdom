using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Characters
{
    //public Animator anim;

    [Header("Timers")]
    public float cooldownAttack;
    public float timeCounter;

    [Header("UnitsCanAttack")]
    public List<GameObject> unitsCanAttack = new List<GameObject>();
    private GameObject closestObject;
    private Characters characters; 
    private bool canAttack;

    void Start()
    {
        base.MyStart();
    }

    void Update()
    {
        base.MyUpdate();
        if (selectedTarget == null && unitsCanAttack.Count > 0)
        {
            if (unitsCanAttack.Count > 1)
            {
                FindClosestObject();
                return;
            }
            selectedTarget = unitsCanAttack[0].transform.gameObject.GetComponent<Characters>().gameObject;
            targetTransform = selectedTarget.transform;

            characters = selectedTarget.transform.gameObject.GetComponent<Characters>(); 
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

            if (characters.hitPoints <= 0)
            {
                if (characters.isDead == false)
                {
                    UnitDies();
                    return;
                }
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
        if (canAttack)
        {        
            selectedTarget.GetComponent<PlayableUnitBehaviour>().TakeDamage(attack, this.gameObject);

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
        characters = selectedTarget.transform.gameObject.GetComponent<Characters>();
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
            characters = null; 
            distanceFromTarget = Mathf.Infinity;
        }
    }
    #endregion

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    void UnitDies()
    {
        characters.SetDead(); 
        unitsCanAttack.Remove(selectedTarget);
        targetTransform = null;
        selectedTarget = null;
        characters = null; 
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
