﻿using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    Transform selfTransform; 

    Transform focalPoint;
    Vector3 initialPosition;
    Quaternion initialRotation;

    Vector3 cameraForward;
    Vector3 mouseOrigin;
    
    [Header("Scrolling")]
    [SerializeField]
    float ZoomSpeed;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;
    float scrollAxis;
    float mouseAxis; 
    
    void Start()
    {
        selfTransform = transform; 
        focalPoint = transform.parent;

        initialPosition = focalPoint.position;
        initialRotation = focalPoint.rotation;
    }
	void Update ()
    {
        cameraForward = transform.forward;

        scrollAxis = mouseAxis * ZoomSpeed;
        if(scrollAxis != 0)
        {
            if(selfTransform.position.y > minY && selfTransform.position.y < maxY)
            {
                Scrolling();
                return; 
            }

            if(selfTransform.position.y <= minY && scrollAxis < 0)
            {
                Scrolling();
            }

            if(selfTransform.position.y >= maxY && scrollAxis > 0)
            {
                Scrolling();
            }
        }
    }

    void Scrolling()
    {
        transform.Translate(cameraForward.x * scrollAxis, cameraForward.y * scrollAxis, cameraForward.z * scrollAxis, Space.World);
    }

    public void SetAxis(float axis)
    {
        mouseAxis = axis; 
    }
}
