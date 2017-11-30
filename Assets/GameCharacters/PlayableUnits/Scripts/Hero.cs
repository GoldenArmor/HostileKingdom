using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    float selectTarget;
    float currentSelectTarget;

    float skillRadius; 
	
	void AttackUpdate(Vector3 center)
    {
        
    }

    void SelectTarget()
    {
        currentSelectTarget -= Time.deltaTime;
        if(currentSelectTarget < 0)
        {
            AttackUpdate(); 
        }
    }
}
