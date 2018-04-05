using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherTurret : Turret
{
    [SerializeField]
    GameObject arrowPrefab; 

    [SerializeField]
    Transform arrowSpawnPoint;

    [SerializeField]
    float range;

    [SerializeField]
    float attackCooldown;
    float currentCooldown;

    [SerializeField]
    List<GameObject> enemiesCanAttack;

    void Start()
    {
        enemiesCanAttack = new List<GameObject>(); 
    }

    void Update()
    {
        if (enemiesCanAttack.Count > 0)
        {
            currentCooldown -= Time.deltaTime; 
            if (currentCooldown <= 0)
            {
                Attack(); 
                currentCooldown = attackCooldown; 
            }
        }
    }

    void Attack()
    {
        Arrow newArrow = ObjectPoolingManager.ArrowPool.GetObject(arrowPrefab, arrowSpawnPoint);
        if (enemiesCanAttack.Count > 1)
        {
            newArrow.GetTarget(enemiesCanAttack[RandomEnemy()].transform.position);
        }
        else newArrow.GetTarget(enemiesCanAttack[0].transform.position); 
    }

    int RandomEnemy()
    {
        return Random.Range(0, 2); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesCanAttack.Add(other.gameObject); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesCanAttack.Remove(other.gameObject);
        }
    }

    public override void Sell()
    {
        base.Sell();
        EndSell(); 
    }

    void OnDrawGizmos()
    {
        Color color = Color.blue;
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, range); 
    }
}
