using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Characters
{
    //public Animator anim;

    [Header("Timers")]
    [SerializeField]
    float cooldownAttack;
    [SerializeField]
    float timeCounter;

    [Header("UnitsCanAttack")]
    [SerializeField]
    List<Transform> unitsCanAttack = new List<Transform>();
    Transform closestObject;
    Characters characters; 
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
            selectedTarget.GetComponent<PlayableUnitBehaviour>().PlayableUnitTakeDamage(attack, gameObject);

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
                if (Vector3.Distance(transform.position, unitsCanAttack[i].position) <=
                Vector3.Distance(transform.position, closestObject.position))
                {
                    closestObject = unitsCanAttack[i];
                }
            }
            else closestObject = unitsCanAttack[i];
        }
        selectedTarget = closestObject.gameObject;
        targetTransform = closestObject;
        characters = selectedTarget.GetComponent<Characters>();
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
        Debug.Log("DEAD"); 
        unitsCanAttack.Remove(selectedTarget.transform);
        distanceFromTarget = Mathf.Infinity;
        if (characters.isDead == false) characters.SetDead();
        characters = null;
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
