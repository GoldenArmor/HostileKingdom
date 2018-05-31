using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Characters
{
    [SerializeField]
    protected int moneyValue;

    [Header("Popup Text")]
    [SerializeField]
    Canvas myCanvas; 
    [SerializeField]
    GameObject popupTextPrefab;

    [Header("ExplosionWhenDie")]
    [SerializeField]
    BodyExplosion bodyExplosion; 

    PopupText newDamagePopup;

    [Header("Sounds")]
    [SerializeField]
    protected AudioPlayer audioPlayer;

    public bool die; 

    protected override void MyStart()
    {
        base.MyStart();
        die = false; 
        objective = GameObject.FindGameObjectWithTag("Objective").transform; 
    }

    public override void PooledStart()
    {
        if (bodyExplosion != null) bodyExplosion.PooledStart(); 
        base.PooledStart();
    }

    protected override void MyUpdate()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.F)) die = true; 

        if (die)
        {
            SetDead();  
            return;
        }

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

    protected override void DeadUpdate()
    {
        base.DeadUpdate();
    }
    #endregion

    #region Sets
    public override void SetDead()
    {
        //enemiesManager.enemiesCount.Remove(this);
        if (!isDead)
        {
            ClearDamage(); 
            die = true; 
            EnemyWaveManager.enemiesAlive--;
            Player.money += moneyValue;
            if (bodyExplosion != null)
            {
                bodyExplosion.Die();
                audioPlayer.PlaySFX(0);
            }
            base.SetDead();
        }
        else
        {
            DeadUpdate(); 
        }
    }

    public override void SetMovement()
    {
        if(objective != null) agent.SetDestination(objective.position);
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
