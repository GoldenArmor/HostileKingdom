using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] 
    Transform objective;
    public static Transform enemiesObjective; 

    void Start()
    {
        enemiesObjective = objective; 
    }
}
