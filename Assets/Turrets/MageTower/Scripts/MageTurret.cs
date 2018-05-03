using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageTurret : MonoBehaviour
{
    [SerializeField]
    Transform raySpawnPoint; 

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

    void Start()
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

            currentCooldown -= Time.deltaTime;
            if (currentCooldown <= 0)
            {
                FindClosestTarget(); 
                currentCooldown = attackCooldown;
            }
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
            ClearTarget();
        }

        currentMultiplier += damageIncrease; 
    }

    void ClearTarget()
    {
        enemiesCanAttack.Remove(target);
        target.ClearDamage(); 
        target = null;
        currentDamage = baseDamage;
        currentMultiplier = damageMultiplier; 
        isAttacking = false; 
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
            if (other.gameObject == target)
            {
                ClearTarget(); 
            }
            enemiesCanAttack.Remove(other.GetComponent<Enemy>());
        }
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
        if (isAttacking)
        {
            Debug.DrawLine(raySpawnPoint.position, target.transform.position, Color.red); 
        }
    }
}
