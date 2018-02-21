using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    Transform mainCameraTransform; 

    Transform camTransform;

    [Header("Rotation")]
    [SerializeField]
    float cameraRotationSpeed;

    [Header("Inputs")]
    float rotationAxis;
    float mouseRotationAxis;
    Vector2 inputAxis;

    void Start()
    {
        camTransform = transform;
        mainCameraTransform.LookAt(camTransform); 
    }

    void Rotation()
    {
        camTransform.Rotate(Vector3.up, rotationAxis * Time.deltaTime * cameraRotationSpeed, Space.World);
    }
    void MouseRotation()
    {
        camTransform.Rotate(Vector3.up, mouseRotationAxis * Time.deltaTime * cameraRotationSpeed, Space.World);
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
