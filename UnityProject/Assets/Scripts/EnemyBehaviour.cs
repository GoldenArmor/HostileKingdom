using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    private enum UnitState { Idle, Patrol, Chase, Attack, Dead }
    [SerializeField] private UnitState state;

    [Header("Properties")]
    public float hitPoints;
    public float armor;
    public float attack;
    public float magicAttack;
    public float magicArmor;
    public float scope;
    private float rotateSpeed = 125f; 
    [SerializeField] private bool isDead;

    [Header("Stats")]
    //public Animator anim;

    [Header("Timers")]
    public float cooldownAttack;
    public float timeCounter;

    [Header("Distances")]
    [SerializeField] private float distanceFromUnit = Mathf.Infinity;

    [Header("OnScreen")]
    private UnityEngine.AI.NavMeshAgent agent;

    [Header("UnitsInteraction")]
    [SerializeField] private GameObject selectedUnit;
    [SerializeField] private Transform unitTransform;
    public List<GameObject> unitsCanAttack = new List<GameObject>();
    private GameObject closestObject;
    private bool canAttack;

    public LayerMask mask;

    void Start()
    {
        agent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        SetIdle();
    }

    void Update()
    {
        if (selectedUnit != null) CalculateDistanceFromTarget();
        if (selectedUnit == null && unitsCanAttack.Count > 0)
        {
            if (unitsCanAttack.Count > 1)
            {
                FindClosestObject();
                return;
            }
            selectedUnit = unitsCanAttack[0].transform.gameObject;
            unitTransform = selectedUnit.transform;
        }

        switch (state)
        {
            case UnitState.Idle:
                IdleUpdate();
                break;
            case UnitState.Patrol:
                PatrolUpdate();
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
    void IdleUpdate()
    {
        if (selectedUnit != null)
        {
            if (distanceFromUnit >= scope)
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
                LookAtUnit(); 
                timeCounter += Time.deltaTime;
            }
        }
    }

    void PatrolUpdate()
    {

    }

    void ChaseUpdate()
    {
        if (selectedUnit != null)
        {
            agent.SetDestination(unitTransform.position);

            if (distanceFromUnit < scope)
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

    void AttackUpdate()
    {
        if (canAttack)
        {
            unitTransform.GetComponent<PlayableUnitBehaviour>().TakeDamage(attack, this.gameObject);

            SetIdle();
            return;
        }
    }

    void DeadUpdate()
    {
        //SetDead();
    }
    #endregion

    #region Sets
    void SetIdle()
    {
        agent.isStopped = true;
        //anim.SetBool("IsMoving", false);
        //anim.SetTrigger("Idle");
        timeCounter = 0;
        state = UnitState.Idle;
    }

    void SetChase()
    {
        agent.isStopped = false;
        //anim.SetBool("Attack", false);
        //anim.SetBool("IsMoving", true);
        state = UnitState.Chase;
    }

    void SetAttack()
    {
        canAttack = true;
        agent.isStopped = true; 
        //anim.SetBool("Attack", true);
        state = UnitState.Attack;
    }

    void SetDead()
    {
        agent.isStopped = true;
        //anim.SetTrigger("Die");
        state = UnitState.Dead;
    }
    #endregion

    #region CalculationVoids
    void CalculateDistanceFromTarget() //Calculates the distance between the Unit and the Selected enemy. 
    {
        unitTransform = selectedUnit.transform;
        distanceFromUnit = Vector3.Distance(transform.position, unitTransform.position);
    }

    void LookAtUnit()
    {
        Vector3 lookDir = unitTransform.position - transform.position;
        Quaternion q = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }

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
        selectedUnit = closestObject;
        unitTransform = selectedUnit.transform;
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
            unitTransform = null;
            selectedUnit = null;
            distanceFromUnit = Mathf.Infinity;
        }
    }
    #endregion

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;

        if (hitPoints <= 0 && !isDead)
        {
            isDead = true;
            hitPoints = 0;
            SetDead();
        }
        /*else if (damage > 0)
        {

        }*/
    }

    void OnDrawGizmos()
    {
        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, scope);
    }
}
