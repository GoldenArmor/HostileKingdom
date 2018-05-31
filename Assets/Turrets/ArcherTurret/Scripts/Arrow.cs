using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPooledObject
{
    [SerializeField]
    Transform myTransform;
    [SerializeField]
    GameObject target;

    [SerializeField]
    float velocity;

    [SerializeField]
    float damage; 

    [SerializeField]
    bool hasCollided;

    [SerializeField]
    float lifeCounter;
    float currentLifeCounter;

    [SerializeField]
    ParticleSystem partSystem; 

    public void PooledStart()
    { 
        currentLifeCounter = lifeCounter; 
    }

    public void PooledAwake()
    {
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (target != null && !hasCollided)
        {
            myTransform.LookAt(target.transform);
            myTransform.position = Vector3.Lerp(myTransform.position, target.transform.position, velocity); 
        }

        if (hasCollided)
        {
            currentLifeCounter -= Time.deltaTime;

            if (currentLifeCounter <= 0)
            {
                myTransform.parent = null;
                hasCollided = false;
                partSystem.Stop(); 
                gameObject.SetActive(false); 
            }
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }

    public void GetTarget(GameObject newTarget)
    {
        target = newTarget;
        myTransform.LookAt(target.transform); 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(other.GetComponent<Enemy>() != null)
            {
                Enemy newEnemy = other.GetComponent<Enemy>(); 
                newEnemy.TakeDamage(damage);
                newEnemy.arrows.Add(this); 
                myTransform.parent = other.transform;
                //target.transform.position = Vector3.zero;
            }
            hasCollided = true;
            partSystem.Play();
        }
        //myTransform.parent = other.transform;    
        else
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other); 
        }
    }

    public void ClearArrow()
    {
        myTransform.parent = null;
        gameObject.SetActive(false);
    }
}
