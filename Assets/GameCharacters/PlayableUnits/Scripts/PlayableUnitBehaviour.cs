using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableUnitBehaviour : Characters
{
    [Header("Cards")]
    [SerializeField]
    UnitCards cards;
    public bool isSelected;

    [Header("Timers")]
    [SerializeField]
    float cooldownAttack;
    float timeCounter;

    [Header("Distances")]
    [SerializeField]
    float chaseRange;
    Vector3 newFormationPosition;
    //[SerializeField]
    //float newDestinationRadius;

    [Header("OnScreen")]
    public bool isOnScreen;
    [HideInInspector]
    public Vector2 screenPosition;
    float maxDistance = Mathf.Infinity;
    [SerializeField]
    protected Mouse mouse;
    RaycastHit hit;
    protected Camera mainCamera;

    [Header("EnemyInteraction")]
    EnemyBehaviour selectedTarget;
    protected bool isAttacking;

    [Header("GOD Mode")]
    public bool godMode;

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
        if (selectedTarget != null)
        {
            CalculateDistanceFromTarget();
            if (!selectedTarget.isActiveAndEnabled)
            {
                ClearEnemy();
            }
        } 
        screenPosition = mainCamera.WorldToScreenPoint(transform.position);
        isOnScreen = false; 
        if (mouse.UnitWithinScreenSpace(screenPosition)) //This function lets the player know if the Unit is in the screenview to do a drag selection. 
        {
            isOnScreen = true;
        }
        MyUpdate();
    }

    #region Updates
    protected override void IdleUpdate()
    {
        if (isAttacking)
        {
            if (distanceFromTarget > attackRange)
            {
                isAttacking = false;
                SetChase();
                return;
            }

            timeCounter += Time.deltaTime;
            LookAtTarget();

            if (timeCounter > cooldownAttack)
            {
                SetAttack();
            }
        }
    }

    protected override void MoveUpdate()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            SetIdle();
        }
    }

    protected override void ChaseUpdate()
    {
        if (distanceFromTarget < chaseRange)
        {
            agent.SetDestination(targetTransform.position);
            if (distanceFromTarget < attackRange)
            {
                SetAttack();
            }
        }
        else SetIdle();
    }

    protected override void AttackUpdate()
    { 
        selectedTarget.TakeDamage(attack);
        if (selectedTarget.hitPoints <= 0) ClearEnemy();

        timeCounter = 0;
        SetIdle();
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
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                targetTransform = hit.transform;
                selectedTarget = targetTransform.GetComponent<EnemyBehaviour>();

                SetChase();
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
        SetIdle();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Color newColor = Color.green;
        newColor.a = 0.2f;
        Gizmos.color = newColor;
        Gizmos.DrawSphere(transform.position, attackRange);

       if (state == UnitState.Movement)
       {
            Gizmos.color = newColor;
            Gizmos.DrawSphere(agent.destination, agent.stoppingDistance);
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
                newFormationPosition = formationPosition; 
                transform.position = hit.point + newFormationPosition;
                agent.enabled = true; 

                SetMovement();
            }
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (hit.transform.gameObject != selectedTarget)
                {
                    targetTransform = hit.transform;
                    selectedTarget = targetTransform.GetComponent<EnemyBehaviour>();

                    SetChase();
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
