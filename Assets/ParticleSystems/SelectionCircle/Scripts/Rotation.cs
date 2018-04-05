using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Transform myTransform; 

    void Update()
    {
        myTransform.localRotation *= Quaternion.Euler(0, 0, Time.deltaTime * rotationSpeed);  
    }
}
