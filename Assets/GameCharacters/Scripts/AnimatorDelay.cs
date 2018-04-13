using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorDelay : MonoBehaviour
{
    [SerializeField]
    float delay;
    float currentDelay; 
    Animator anim; 

	void Start ()
    {
        anim = GetComponent<Animator>();
        currentDelay = delay; 
	}
	
	void Update ()
    {
        currentDelay -= Time.deltaTime;
        if (currentDelay <= 0)
        {
            currentDelay = delay;
            anim.enabled = true;
            enabled = false; 
        }
	}
}
