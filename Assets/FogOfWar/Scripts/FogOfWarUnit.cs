using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarUnit : MonoBehaviour
{
    Transform[] positions;
    float timeCounter;
	
	void Update ()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= 1)
        {
            timeCounter = 0; 

            //positions
        }
	}
}
