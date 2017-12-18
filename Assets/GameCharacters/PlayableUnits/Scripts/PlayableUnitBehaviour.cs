using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnitBehaviour : Characters, IPlayableUnit
{
    [Header("GOD Mode")]
    public bool godMode;

    //public Animator anim;
    [SerializeField]
    UnitCards cards;
    public bool isSelected = false;

    [Header("Timers")]
    [SerializeField]
    float cooldownAttack;
    [SerializeField]
    float timeCounter;

    [Header("Distances")]
    [SerializeField]
    float chaseRange;
    Vector3 newFormationPosition;
    [SerializeField]
    float newDestinationRadius;

    [Header("OnScreen")]
    public bool isOnScreen = false;
    [HideInInspector]
    public Vector2 screenPosition;
    float maxDistance = Mathf.Infinity;
    [SerializeField]
    protected Mouse mouse;
    RaycastHit hit;
    protected Camera mainCamera;

    [Header("EnemyInteraction")]
    [SerializeField]
    EnemyBehaviour selectedTarget;
    protected bool isAttacking; 
    bool canAttack;

    protected virtual void UnitStart()
    {
        MyStart();
        if(cards != null)
        {
            cards.targetName.text = characterName;
            cards.startingHealth = startingHitPoints;
            cards.MyStart();
        }
        mainCamera = Camera.main;
    }

    protected virtual void UnitUpdate()
    {
        MyUpdate();
        if (selectedTarget != null) CalculateDistanceFromTarget();
        screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        if (mouse.UnitWithinScreenSpace(screenPosition)) //This function lets the player know if the Unit is in the screenview to do a drag selection. 
        {
            isOnScreen = true;
        }
        else
        {
            if (isOnScreen)
            {
                isOnScreen = false;
            }
        }
        if (selectedTarget != null && !selectedTarget.isActiveAndEnabled)
        {
            ClearEnemy();
        }
    }

    #region Updates
    protected override void IdleUpdate()
    {
        if (isAttacking)
        {
            if (selectedTarget.hitPoints <= 0)
            {
                ClearEnemy();
                return;
            }
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

    protected override void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) <= newDestinationRadius)
        {
            SetIdle();
            return; 
        }
    }

    protected override void ChaseUpdate()
    {
        if (distanceFromTarget < scope)
        {
            agent.avoidancePriority = 99;
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
        else
        {
            agent.avoidancePriority = 0; 
            agent.SetDestination(targetTransform.position);
        }
    }

    protected override void AttackUpdate()
    {
        if (canAttack)
        {
            if (!isAttacking) isAttacking = true; 
            canAttack = false;
            selectedTarget.TakeDamage(attack);

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
    protected override void SetMovement()
    {
        isAttacking = false;
        base.SetMovement();
    }

    protected override void SetAttack()
    {
        isAttacking = true;
        base.SetAttack();
    }

    public override void SetDead()
    {
        if (isOnScreen)
        {
            mouse.selectableUnits.Remove(this);
            isOnScreen = false;
        }
        if (mouse.selectedUnit == this) mouse.selectedUnit = null;
        if (mouse.selectedUnits.Contains(this)) mouse.selectedUnits.Remove(this); 
        base.SetDead();
    }
    #endregion

    #region PublicVoids

    public void PlayableUnitTakeDamage(float damage, EnemyBehaviour autoTarget)
    {
        if (!godMode)
        {
            hitPoints -= damage;
            cards.UpdateLifeBar(hitPoints);
        }
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
    }

    public void ClickUpdate(Vector3 formationPosition, Vector3 mousePosition) //When I click right button. It's called from the InputManager script.  
    {
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
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
                    targetTransform = hit.transform;
                    selectedTarget = targetTransform.GetComponent<EnemyBehaviour>();

                    SetChase();
                    return;
                }
            }
        }
    }
    #endregion

    void ClearEnemy()
    {
        distanceFromTarget = Mathf.Infinity;
        if (selectedTarget.isDead == false) selectedTarget.SetDead();
        targetTransform = null;
        selectedTarget = null;
        isAttacking = false;
        anim.SetBool("Attack", false);
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

    public void GodUpdate(Vector3 formationPosition, Vector3 mousePosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground") ||
                hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit"))
            {
                if (selectedTarget != null)
                {
                    targetTransform = null;
                    selectedTarget = null;
                }
                agent.enabled = false;
                newFormationPosition = formationPosition; //If I have more than 1 unit selected it will change the value to avoid conflicts. 
                transform.position = hit.point + newFormationPosition;
                agent.enabled = true; 

                SetMovement();
                return;
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.transform.gameObject != selectedTarget)
                {
                    targetTransform = hit.transform;
                    selectedTarget = targetTransform.GetComponent<EnemyBehaviour>();

                    SetChase();
                    return;
                }
            }
        }
    }

    #region CalculationVoids
    void CalculateDistanceFromTarget() //Calculates the distance between the Unit and the Selected enemy. 
    {
        targetTransform = selectedTarget.transform;
        distanceFromTarget = Vector3.Distance(transform.position, targetTransform.position);
    }
    #endregion
}
