using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour, IPooledObject
{
    [SerializeField]
    Transform myTransform;
    [SerializeField]
    Vector3 target;

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
        if (target != Vector3.zero)
        {
            myTransform.LookAt(target);
            myTransform.position = Vector3.Lerp(myTransform.position, target, velocity); 
        }

        if (hasCollided)
        {
            currentLifeCounter -= Time.deltaTime;

            if (currentLifeCounter <= 0)
            {
                myTransform.parent = null;
                gameObject.SetActive(false); 
            }
        }
    }

    public bool IsActive()
    {
        return gameObject.activeSelf; 
    }

    public void GetTarget(Vector3 newTarget)
    {
        target = newTarget; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(other.GetComponent<Enemy>() != null)
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
                Debug.Log(other);
                myTransform.parent = other.transform;
                target = Vector3.zero;
            }
        }

        hasCollided = true;
        partSystem.Play();

        Debug.Log(other); 
    }
}
