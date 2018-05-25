using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    [SerializeField]
    GameObject[] bodyParts;
    Rigidbody[] rigidbodies; 
    bool isDead; 

	void Start ()
    {
        rigidbodies = new Rigidbody[bodyParts.Length]; 
        for(int i = 0; i < bodyParts.Length; i++)
        {
            rigidbodies[i] = bodyParts[i].GetComponent<Rigidbody>(); 
        }
	}
	
	void FixedUpdate ()
    {
		
	}

    public void Die()
    {
        isDead = true; 
    }
}
