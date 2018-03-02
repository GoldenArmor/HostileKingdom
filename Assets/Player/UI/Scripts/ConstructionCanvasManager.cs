using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionCanvasManager : MonoBehaviour
{
    [SerializeField]
    SpawnScalePingPong spawnEasing;
    [SerializeField]
    public Transform myTransform;
    [SerializeField]
    Billboard billboard;

    public void Initialize(Vector3 spawnPosition)
    {
        myTransform.position = spawnPosition; 
        spawnEasing.ResetEasing(); 
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        spawnEasing.ResetEasing();
    }
}
