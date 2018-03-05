using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    SpawnScalePingPong spawnEasing;
    [SerializeField]
    public Transform myTransform;

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
