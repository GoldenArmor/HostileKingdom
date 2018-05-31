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
    Archer[] archers; 

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
            for(int i = 0; i < enemiesCanAttack.Count; i++)
            {
                if(!enemiesCanAttack[i].activeSelf)
                {
                    enemiesCanAttack.Remove(enemiesCanAttack[i]); 
                }
            }
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
        Archer newArcher = RandomArcher(); 
        arrowSpawnPoint = newArcher.arrowSpawnPoint;
        newArcher.anim.SetTrigger("Attack");
        Arrow newArrow = ObjectPoolingManager.ArrowPool.GetObject(arrowPrefab, arrowSpawnPoint);
        if (enemiesCanAttack.Count > 1)
        {
            int newRandomEnemy = RandomEnemy(); 
            newArrow.GetTarget(enemiesCanAttack[newRandomEnemy].transform.position);

            Vector3 lookPosition = enemiesCanAttack[newRandomEnemy].transform.position - newArcher.transform.position;
            lookPosition.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPosition);
            newArcher.transform.rotation = rotation; 
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

    Archer RandomArcher()
    {
        return archers[Random.Range(0, archers.Length)];
    }

    void OnDrawGizmos()
    {
        Color color = Color.blue;
        Gizmos.color = color;

        Gizmos.DrawWireSphere(transform.position, range); 
    }
}
