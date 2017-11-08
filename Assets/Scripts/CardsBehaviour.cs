using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] playableUnits; 

	void Start ()
    {
        playableUnits = GameObject.FindGameObjectsWithTag("PlayableUnit");
	}
	
	void Update ()
    {
		
	}
}
