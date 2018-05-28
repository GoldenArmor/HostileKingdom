using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    [Header("Body Parts")]
    [SerializeField]
    GameObject fullGolem;
    [SerializeField]
    GameObject[] bodyParts;
    Rigidbody[] rigidbodies;

    [Header("Explosion")]
    [SerializeField]
    Transform explosionCenter; 
    [SerializeField]
    float explosionForce;
    [SerializeField]
    float upForce; 
    [SerializeField]
    float explosionRadius; 
    [SerializeField]
    bool isDead; 

	void Start ()
    {
        rigidbodies = new Rigidbody[bodyParts.Length]; 
        for(int i = 0; i < bodyParts.Length; i++)
        {
            rigidbodies[i] = bodyParts[i].GetComponent<Rigidbody>(); 
        }
	}

    void Update()
    {
        if (isDead)
        {
            Die();
            isDead = false; 
        }
    }

    public void Die()
    {
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            bodyParts[i].transform.SetParent(null);
            fullGolem.SetActive(false); 
            bodyParts[i].SetActive(true);
            rigidbodies[i].AddExplosionForce(explosionForce, explosionCenter.position, explosionRadius, upForce, ForceMode.Impulse);
        }
    }
}
