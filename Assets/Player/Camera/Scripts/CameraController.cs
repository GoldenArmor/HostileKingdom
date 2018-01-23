using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform mainCameraTransform; 
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
    float rotationAxis;
    float mouseRotationAxis; 
    Vector2 inputAxis;

    void Start()
    {
        camTransform = transform;
        mainCameraTransform.LookAt(transform);
    }

    void MovementUpdate(Vector3 mousePosition)
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

    void Rotation()
    {
        camTransform.Rotate(Vector3.up, rotationAxis * Time.deltaTime * cameraRotationSpeed, Space.World); 
    }
    void MouseRotation()
    {
        camTransform.Rotate(Vector3.up, mouseRotationAxis * Time.deltaTime * cameraRotationSpeed, Space.World);
    }

    public void SetInputAxis(Vector2 newAxis, Vector3 mousePosition)
    {
        inputAxis = newAxis;
        MovementUpdate(mousePosition);
        LimitPosition();
    }

    public void SetRotationAxis(float newAxis)
    {
        rotationAxis = newAxis;
        Rotation(); 
    }
    public void SetMouseRotationAxis(float newAxis)
    {
        mouseRotationAxis = newAxis;
        MouseRotation(); 
    }
}
