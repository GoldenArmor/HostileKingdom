using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayableUnitBehaviour : MonoBehaviour
{
    private enum UnitState { Idle, Movement, Chase, Attack, Stun, Dead }
    [SerializeField] private UnitState state;

    [Header("Stats")]
    //private bool canAttack;
    public Animator anim;
    public bool isSelected = false;

    [Header("Timers")]
    public float idleTime = 1;
    public float cooldownAttack = 1.5f;
    public float stunTime = 1;
    private float timeCounter;

    [Header("Distances")]
    public float chaseRange;
    public float attackRange;
    [SerializeField]
    private float distanceFromEnemy = Mathf.Infinity;
    private Vector3 newFormationPosition; 

    [Header("OnScreen")]
    [HideInInspector] public Vector2 screenPosition;
    private float maxDistance = Mathf.Infinity; //La distancia máxima que puede alcanzar el rayo. 
    private MouseBehaviour mouse; //El componente MouseBehaviour para actualizar.
    private NavMeshAgent agent;
    private RaycastHit hit;
    public bool isOnScreen = false;

    [Header("EnemyInteraction")]
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject selectedEnemy;
    public List<GameObject> enemiesCanAttack = new List<GameObject>();
    private bool canAttack; 

    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos.

    void Start ()
    {
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>(); //Buscamos el GameObject Player y cojemos su componente MouseBehaviour().
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        agent.isStopped = true;  

        SetIdle();
    }
	
	void Update ()
    {
        if (selectedEnemy != null) CalculateDistanceFromTarget();
        if (selectedEnemy == null && enemiesCanAttack.Count > 0)
        {
            selectedEnemy = enemiesCanAttack[0].transform.gameObject;
            enemyTransform = selectedEnemy.transform;
        }

        screenPosition = Camera.main.WorldToScreenPoint(this.transform.position);
        if (mouse.UnitWithinScreenSpace(screenPosition))
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
                IsMoving();
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
    void IdleUpdate()
    {
        if (agent.isStopped == false)
        {
            SetMovement();
            return;
        }
        if (selectedEnemy != null)
        {
            SetChase();
            return; 
        }
    }

    public void ClickUpdate(Vector3 formationPosition) //Función que se ejecuta para mover la unidad. 
    {
        newFormationPosition = formationPosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo.

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //Si el rayo colisiona con un objeto de la capa Ground. 
            {
                    if (selectedEnemy != null)
                    {
                        enemyTransform = null;
                        selectedEnemy = null;
                    }
                    agent.destination = hit.point + newFormationPosition;
                    agent.isStopped = false;

                    SetMovement();
                    return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                selectedEnemy = hit.transform.gameObject;
                enemyTransform = selectedEnemy.transform;

                agent.isStopped = false;

                SetChase();
                return; 
            }
        }
    }

    void IsMoving()
    {
        if (this.transform.position == agent.destination)
        {
            anim.SetBool("IsMoving", false);
            agent.isStopped = true;
            SetIdle();
            return;
        }
    }

    void ChaseUpdate()
    {
        if (selectedEnemy != null)
        {
            agent.SetDestination(enemyTransform.position);

            if (distanceFromEnemy < attackRange)
            {
                SetAttack();
                return;
            }
        }
        else
        {
            distanceFromEnemy = Mathf.Infinity;
            SetIdle();
            return; 
        }
    }

    void AttackUpdate()
    {
        if (distanceFromEnemy > attackRange)
        {
            SetChase();
            return; 
        }
    }

    void StunUpdate()
    {
        if (timeCounter >= stunTime)
        {
            idleTime = 0;
            SetIdle();
        }
        else timeCounter += Time.deltaTime;
    }

    void DeadUpdate()
    {
        //SetDead(); 
    }
    #endregion

    #region Sets
    void SetIdle()
    {
        //no se mueva
        anim.SetTrigger("Idle");
        timeCounter = 0;
        state = UnitState.Idle;
    }

    void SetMovement()
    {
        anim.SetBool("Attack", false); 
        anim.SetBool("IsMoving", true);
        state = UnitState.Movement;
    }

    void SetChase()
    {
        //Feedback pasa a modo persecución
        anim.SetBool("Attack", false);
        anim.SetBool("IsMoving", true);
        agent.isStopped = false; 
        state = UnitState.Chase;
    }

    public void SetAttack()
    {
        anim.SetBool("Attack", true);
        agent.isStopped = true;
        //Feedback pasa a modo ataque
        state = UnitState.Attack;
    }

    void SetStun()
    {
        anim.SetTrigger("Stun");
        agent.isStopped = true;
        timeCounter = 0;
        state = UnitState.Stun;
    }

    void SetDead()
    {
        anim.SetTrigger("Die");
        agent.isStopped = true;
        state = UnitState.Dead;

        //this.gameObject.SetActive(false);
    }
    #endregion

    void CalculateDistanceFromTarget()
    {
        distanceFromEnemy = Vector3.Distance(transform.position, enemyTransform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesCanAttack.Add(other.transform.gameObject); 
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" && enemiesCanAttack != null)
        {
            enemiesCanAttack.Remove(other.transform.gameObject);
            enemyTransform = null;  
            selectedEnemy = null;
            distanceFromEnemy = Mathf.Infinity; 
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Color newColor = Color.red;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}