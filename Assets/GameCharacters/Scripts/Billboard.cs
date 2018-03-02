using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    Transform mainCamera;
    [SerializeField]
    Transform myTransform;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;    
    }

    void Update()
    {
        myTransform.LookAt(myTransform.position + mainCamera.rotation * Vector3.forward,
            mainCamera.rotation * Vector3.up);
    }
}
