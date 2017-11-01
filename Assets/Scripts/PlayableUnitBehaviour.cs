using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnitBehaviour : MonoBehaviour
{
    private enum UnitState { Idle, Movement, Chase, Attack, Dead }
    [SerializeField] private UnitState state;

    [Header("Properties")]
    public float hitPoints;
    public float armor;
    public float attack;
    public float magicAttack;
    public float magicArmor;
    public float scope;
    private float rotateSpeed = 125f;
    [SerializeField] private bool isDead = false;

    [Header("Stats")]
    //public Animator anim;
    public bool isSelected = false;

    [Header("Timers")]
    public float cooldownAttack;
    public float timeCounter;

    [Header("Distances")]
    public float chaseRange;
    [SerializeField] private float distanceFromEnemy = Mathf.Infinity;
    private Vector3 newFormationPosition;
    public float newDestinationRadius;

    [Header("OnScreen")]
    public bool isOnScreen = false;
    [HideInInspector] public Vector2 screenPosition;
    private float maxDistance = Mathf.Infinity;
    private MouseBehaviour mouse;
    private UnityEngine.AI.NavMeshAgent agent;
    private RaycastHit hit;

    [Header("EnemyInteraction")]
    [SerializeField] private GameObject selectedEnemy;
    [SerializeField] private Transform enemyTransform;
    private LifebarBehaviour lifeBar;
    private bool isAttacking; 
    private bool canAttack;

    public LayerMask mask;

    void Start()
    {
        lifeBar = GameObject.FindGameObjectWithTag("LifeBar").GetComponent<LifebarBehaviour>();
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>();
        agent = this.gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();

        SetIdle();
    }

    void Update()
    {
        if (selectedEnemy != null) CalculateDistanceFromTarget();

        screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (mouse.UnitWithinScreenSpace(screenPosition)) //This function lets the player know if the Unit is in the screenview to do a drag selection. 
        {
            isOnScreen = true;
            if (!mouse.unitsOnScreenSpace.Contains(this.gameObject))
            {
                mouse.unitsOnScreenSpace.Add(this.gameObject);
            }
        }
        else
        {
            if (isOnScreen)
            {
                mouse.unitsOnScreenSpace.Remove(this.gameObject);
                isOnScreen = false;
            }
        }

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
    void IdleUpdate()
    {
        if (isAttacking)
        {
            if (timeCounter >= cooldownAttack)
            {
                canAttack = true;
                SetAttack();
                return;
            }
            else timeCounter += Time.deltaTime;

            LookAtEnemy();
        }
    }

    void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= newDestinationRadius)
        {
            SetIdle();
            return; 
        }
    }

    void ChaseUpdate()
    {
        if (distanceFromEnemy < scope)
        {
            if (!isAttacking)
            {
                canAttack = true;
                SetAttack();
                return;
            }
            else
            {
                SetIdle();
                return; 
            }
        }
        else agent.SetDestination(enemyTransform.position);
    }

    void AttackUpdate()
    {
        if (enemyTransform.GetComponent<EnemyBehaviour>().hitPoints <= 0)
        {
            EnemyDies();
        }
        else if (canAttack)
        {
            if (!isAttacking) isAttacking = true; 
            canAttack = false; 
            enemyTransform.GetComponent<EnemyBehaviour>().TakeDamage(attack);

            timeCounter = 0;
            SetIdle();
            return;
        }
        else if (distanceFromEnemy >= scope)
        {
            isAttacking = false; 
            SetChase();
            return;
        }
    }

    void DeadUpdate()
    {
        SetDead();
    }
    #endregion

    #region Sets
    void SetIdle()
    {
        agent.isStopped = true;
        //anim.SetBool("IsMoving", false);
        //anim.SetTrigger("Idle");
        state = UnitState.Idle;
    }

    void SetMovement()
    {
        isAttacking = false; 
        agent.isStopped = false; 
        //anim.SetBool("Attack", false);
        //anim.SetBool("IsMoving", true);
        state = UnitState.Movement;
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
        agent.isStopped = true; 
        //anim.SetBool("Attack", true);
        state = UnitState.Attack;
    }

    public void SetDead()
    {
        isDead = true;
        hitPoints = 0; 
        agent.isStopped = true;
        //anim.SetTrigger("Die");
        state = UnitState.Dead;

        this.gameObject.SetActive(false);
    }
    #endregion

    #region CalculationVoids
    void CalculateDistanceFromTarget() //Calculates the distance between the Unit and the Selected enemy. 
    {
        enemyTransform = selectedEnemy.transform;
        distanceFromEnemy = Vector3.Distance(transform.position, enemyTransform.position);
    }

    void LookAtEnemy()
    {
        Vector3 lookDir = enemyTransform.position - transform.position;
        Quaternion q = Quaternion.LookRotation(lookDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, Time.deltaTime * rotateSpeed);
    }
    #endregion

    #region PublicVoids
    public void TakeDamage(float damage, GameObject autoTarget)
    {
        hitPoints -= damage;

        if (selectedEnemy == null)
        {
            selectedEnemy = autoTarget;
            if (!isAttacking)
            {
                canAttack = true;
                SetAttack();
                return;
            }                
        }

        if (lifeBar.selectedUnit == this.gameObject) lifeBar.UpdateBar(hitPoints);
    }

    public void ClickUpdate(Vector3 formationPosition) //When I click right button. It's called from the InputManager script.  
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (selectedEnemy != null)
                {
                    enemyTransform = null;
                    selectedEnemy = null;
                }
                newFormationPosition = formationPosition; //If I have more than 1 unit selected it will change the value to avoid conflicts. 
                agent.SetDestination(hit.point + newFormationPosition);

                SetMovement();
                return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (selectedEnemy != null)
                {
                    enemyTransform = null;
                    selectedEnemy = null;
                }
                newFormationPosition = formationPosition; //If I have more than 1 unit selected it will change the value to avoid conflicts.
                agent.SetDestination(hit.point + newFormationPosition);

                SetMovement();
                return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.transform.gameObject != selectedEnemy)
                {
                    selectedEnemy = hit.transform.gameObject;
                    enemyTransform = selectedEnemy.transform;

                    SetChase();
                    return;
                }
            }
        }
    }
    #endregion

    void EnemyDies()
    {
        enemyTransform.GetComponent<EnemyBehaviour>().SetDead();
        enemyTransform = null;
        selectedEnemy = null;
        distanceFromEnemy = Mathf.Infinity;
        isAttacking = false;
        SetIdle();
        return;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, scope);

       if (state == UnitState.Movement)
        {
            Gizmos.color = newColor;
            Gizmos.DrawSphere(agent.destination, newDestinationRadius);
        }
    }
}
