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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject); 
        myTransform.parent = collision.transform;
        target = Vector3.zero; 
    }
}
