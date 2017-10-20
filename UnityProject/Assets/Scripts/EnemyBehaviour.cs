using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

	void Start ()
    {
		
	}
	
	void Update ()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime, Space.Self);
    }
}
