using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTurret : Turret
{
    [SerializeField]
    float range;
    [SerializeField]
    float baseDamage = 0.00f;
    float currentDamage = 0.00f; 

    [SerializeField]
    float damageIncrease;

    [SerializeField]
    float attackCooldown;
    float currentCooldown;

    [SerializeField]
    float damageMultiplier;
    float currentMultiplier; 

    bool isAttacking; 

    [SerializeField]
    List<Enemy> enemiesCanAttack;

    Enemy oldClosestEnemy; 
    Enemy closestEnemy;
    Enemy target;

    [Header("AttackRay")]
    [SerializeField]
    Transform raySpawnPoint; 
    [SerializeField]
    LineRenderer lr;
    [SerializeField]
    ParticleSystem partSystemTurret;
    [SerializeField]
    ParticleSystem finalRaySystem; 

    void Start()
    {
        enemiesCanAttack = new List<Enemy>();
        currentDamage = baseDamage;
        currentMultiplier = damageMultiplier; 
    }

    public override void PooledStart()
    {
        enemiesCanAttack = new List<Enemy>();
        currentDamage = baseDamage;
        currentMultiplier = damageMultiplier;
    }

    void Update()
    {
        if (enemiesCanAttack.Count > 0)
        {
            if (isAttacking)
            {
                Attack();
                return; 
            }
            finalRaySystem.Stop();

            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {   
                FindClosestTarget(); 
                currentCooldown = attackCooldown;
            }
        }
        else
        {
            lr.enabled = false; 
        }
    }

    void Attack()
    {
        if (currentDamage <= 200)
        {
            currentDamage += damageIncrease * currentMultiplier;
        }

        target.TakeDamage(currentDamage);
        
        if (target.isDead)
        {
            ClearDeadTarget();
            return; 
        }

        currentMultiplier += damageIncrease;

        lr.enabled = true; 
        lr.SetPosition(0, raySpawnPoint.position); 
        lr.SetPosition(1, target.transform.position);

        finalRaySystem.Play();
        finalRaySystem.transform.position = lr.GetPosition(1);
    }

    void ClearDeadTarget()
    {
        lr.enabled = false;
        enemiesCanAttack.Remove(target);
        target = null;
        currentDamage = baseDamage;
        currentMultiplier = damageMultiplier;
        isAttacking = false;
        finalRaySystem.Stop();
    }

    void ClearTarget()
    {
        target.ClearDamage();
        lr.enabled = false;
        enemiesCanAttack.Remove(target);
        target = null;
        currentDamage = baseDamage;
        currentMultiplier = damageMultiplier; 
        isAttacking = false;
        finalRaySystem.Stop();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesCanAttack.Add(other.GetComponent<Enemy>());
            if (target == null)
            {
                FindClosestTarget(); 
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (target != null && Vector3.Distance(target.transform.
                position, transform.position) > range)
            {
                ClearTarget(); 
            }
            enemiesCanAttack.Remove(other.GetComponent<Enemy>());
        }
    }

    public override void Sell()
    {
        base.Sell();
        EndSell();
    }

    protected virtual void FindClosestTarget()
    {       
        for (int i = 0; i < enemiesCanAttack.Count; i++)
        {
            if (closestEnemy != null)
            {
                if (Vector3.Distance(transform.position, enemiesCanAttack[i].transform.position) <=
                Vector3.Distance(transform.position, closestEnemy.transform.position))
                {
                    closestEnemy = enemiesCanAttack[i];
                }
            }
            else closestEnemy = enemiesCanAttack[i];

            target = closestEnemy.GetComponent<Enemy>();
            oldClosestEnemy = closestEnemy;

            isAttacking = true; 
        }
    }

    void OnDrawGizmos()
    {
        Color color = Color.blue;
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, range);
    }
}
