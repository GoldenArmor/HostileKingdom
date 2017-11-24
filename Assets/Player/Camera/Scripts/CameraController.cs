using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform camTransform; 

    [Header("Pan")]
    [SerializeField]
    float panSpeed;
    [SerializeField]
    float panBorderThickness ;

    [Header("Rotation")]
    [SerializeField]
    float cameraRotationSpeed;

    [Header("Limits")]
    [SerializeField]
    Vector2 panLimit;

    [Header("Inputs")]
    Vector2 inputAxis;
    Vector2 mouseAxis;
    [HideInInspector]
    public Vector2 mousePosition;

    void Start()
    {
        camTransform = transform; 
    }

    void Update()
    {
        MovementUpdate();
        LimitPosition(); 
    }

    void MovementUpdate()
    {
        Vector3 newPosition = new Vector3(inputAxis.x, 0, inputAxis.y);

        if (mousePosition.y >= Screen.height - panBorderThickness) newPosition = Vector3.forward;
        else if (mousePosition.y <= panBorderThickness) newPosition = Vector3.back;
        else if (mousePosition.x >= Screen.width - panBorderThickness) newPosition = Vector3.right;
        else if (mousePosition.x <= panBorderThickness) newPosition = Vector3.left;

        newPosition *= panSpeed * Time.deltaTime;
        newPosition = Quaternion.Euler(new Vector3(0f, transform.eulerAngles.y, 0f)) * newPosition;
        newPosition = camTransform.InverseTransformDirection(newPosition);

        camTransform.Translate(newPosition, Space.Self); 
    }

    void LimitPosition()
    {
        camTransform.position = new Vector3(Mathf.Clamp(camTransform.position.x, -panLimit.x, panLimit.x),
            camTransform.position.y,
            Mathf.Clamp(camTransform.position.z, -panLimit.y, panLimit.y));
    }

    public void Rotation()
    {
        camTransform.Rotate(Vector3.up, mouseAxis.x * Time.deltaTime * cameraRotationSpeed, Space.World); 
    }

    public void SetMouseAxis(Vector2 newAxis)
    {
        mouseAxis = newAxis; 
    }

    public void SetInputAxis(Vector2 newAxis)
    {
        inputAxis = newAxis;
    }
}
