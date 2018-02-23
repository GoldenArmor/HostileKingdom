using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public GameObject enemy;
    public Transform spawnPoint; 
    public int count;
    public float rate; //velocidad de spawneo; 

}
