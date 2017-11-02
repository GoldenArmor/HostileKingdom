using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnitBehaviour : Characters
{
    //public Animator anim;
    public bool isSelected = false;

    [Header("Timers")]
    public float cooldownAttack;
    public float timeCounter;

    [Header("Distances")]
    public float chaseRange;
    private Vector3 newFormationPosition;
    public float newDestinationRadius;

    [Header("OnScreen")]
    public bool isOnScreen = false;
    [HideInInspector] public Vector2 screenPosition;
    private float maxDistance = Mathf.Infinity;
    private MouseBehaviour mouse;
    private RaycastHit hit;

    [Header("EnemyInteraction")]
    private bool isAttacking; 
    private bool canAttack;

    public override void Start()
    {
        mouse = GameObject.FindGameObjectWithTag("Player").GetComponent<MouseBehaviour>();
        base.Start();
    }

    public override void Update()
    {
        base.Update();
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
    }

    #region Updates
    public override void IdleUpdate()
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

            LookAtTarget();
        }
    }

    public override void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= newDestinationRadius)
        {
            SetIdle();
            return; 
        }
    }

    public override void ChaseUpdate()
    {
        if (distanceFromTarget < scope)
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
        else agent.SetDestination(targetTransform.position);
    }

    public override void AttackUpdate()
    {
        if (targetTransform.GetComponent<EnemyBehaviour>().hitPoints <= 0)
        {
            EnemyDies();
        }
        else if (canAttack)
        {
            if (!isAttacking) isAttacking = true; 
            canAttack = false; 
            targetTransform.GetComponent<EnemyBehaviour>().TakeDamage(attack);

            timeCounter = 0;
            SetIdle();
            return;
        }
        else if (distanceFromTarget >= scope)
        {
            isAttacking = false; 
            SetChase();
            return;
        }
    }
    #endregion

    #region Sets
    public override void SetMovement()
    {
        isAttacking = false;
        base.SetMovement();
    }

    public override void SetAttack()
    {
        isAttacking = true;
        base.SetAttack();
    }
    #endregion

    #region PublicVoids
    public void TakeDamage(float damage, GameObject autoTarget)
    {
        hitPoints -= damage;

        if (selectedTarget == null)
        {
            selectedTarget = autoTarget;
            if (!isAttacking)
            {
                canAttack = true;
                SetAttack();
                return;
            }                
        }

        //if (lifeBar.selectedUnit == this.gameObject) lifeBar.UpdateBar(hitPoints);
    }

    public void ClickUpdate(Vector3 formationPosition) //When I click right button. It's called from the InputManager script.  
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                if (selectedTarget != null)
                {
                    targetTransform = null;
                    selectedTarget = null;
                }
                newFormationPosition = formationPosition; //If I have more than 1 unit selected it will change the value to avoid conflicts. 
                agent.SetDestination(hit.point + newFormationPosition);

                SetMovement();
                return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (selectedTarget != null)
                {
                    targetTransform = null;
                    selectedTarget = null;
                }
                newFormationPosition = formationPosition; //If I have more than 1 unit selected it will change the value to avoid conflicts.
                agent.SetDestination(hit.point + newFormationPosition);

                SetMovement();
                return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.transform.gameObject != selectedTarget)
                {
                    selectedTarget = hit.transform.gameObject;
                    targetTransform = selectedTarget.transform;

                    SetChase();
                    return;
                }
            }
        }
    }
    #endregion

    void EnemyDies()
    {
        targetTransform.GetComponent<EnemyBehaviour>().SetDead();
        targetTransform = null;
        selectedTarget = null;
        distanceFromTarget = Mathf.Infinity;
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
