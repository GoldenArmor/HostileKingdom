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

    public void PooledStart()
    {

    }

    public void PooledAwake()
    {

    }

    void Update()
    {
        if (target != Vector3.zero)
        {
            myTransform.LookAt(target);
            myTransform.position = Vector3.Lerp(myTransform.position, target, velocity); 
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
                other.GetComponent<Enemy>().TakeDamage(damage, false);
                //Debug.Log(collision.gameObject); 
                myTransform.parent = other.transform;
                target = Vector3.zero;
            }
        }
    }
}
