using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters
{
    [SerializeField]
    int moneyValue;

    [Header("Popup Text")]
    [SerializeField]
    Canvas myCanvas; 
    [SerializeField]
    GameObject popupTextPrefab;

    [Header("ExplosionWhenDie")]
    [SerializeField]
    BodyExplosion bodyExplosion; 

    PopupText newDamagePopup;

    protected override void MyStart()
    {
        base.MyStart();
        objective = GameObject.FindGameObjectWithTag("Objective").transform; 
        
    }

    protected override void MyUpdate()
    {
        SetMovement();

        base.MyUpdate(); 
    }

    #region Updates
    protected override void MoveUpdate()
    {
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance)
        {
            SetMovement();
        }
    }
    #endregion

    #region Sets
    public override void SetDead()
    {
        //enemiesManager.enemiesCount.Remove(this);
        EnemyWaveManager.enemiesAlive--;
        Player.money += moneyValue;
        if (bodyExplosion != null)
        {
            bodyExplosion.Die(); 
        }
        base.SetDead();
    }

    public override void SetMovement()
    {
        agent.SetDestination(objective.position);
        base.SetMovement();
    }
    #endregion

    #region OnTriggerVoids
    void OnTriggerEnter(Collider other) //If a unit enters the collider, it's added to the interactive units list.
    {
        if (other.CompareTag("Objective"))
        {
            Player.lives -= 1;
            SetDead();
            Player.money -= moneyValue; 
        }
    }
    #endregion

    public override void TakeDamage(float damage)
    {
        if (newDamagePopup != null)
        {
            if (!newDamagePopup.IsActive())
            {
                newDamagePopup = ObjectPoolingManager.PopupPool.GetObject(popupTextPrefab, myCanvas.transform);
                newDamagePopup.transform.SetParent(myCanvas.transform, false);
                newDamagePopup.transform.localPosition = Vector3.zero;
                newDamagePopup.transform.localScale = Vector3.one;
                newDamagePopup.SetDamage(damage);
            }
            else
            {

                newDamagePopup.SetDamage(damage);
            }
        }
        else
        {
            newDamagePopup = ObjectPoolingManager.PopupPool.GetObject(popupTextPrefab, transform);
            newDamagePopup.transform.SetParent(myCanvas.transform, false);
            newDamagePopup.transform.localPosition = Vector3.zero;
            newDamagePopup.transform.localScale = Vector3.one;
            newDamagePopup.SetDamage(damage);
        }

        base.TakeDamage(damage);
    }

    public void ClearDamage()
    {
        if (newDamagePopup != null)
        {
            newDamagePopup.ClearDamage(); 
            newDamagePopup = null;
        }
    }
}
