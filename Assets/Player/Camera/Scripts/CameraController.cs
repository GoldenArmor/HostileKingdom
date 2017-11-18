using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;
    public Vector2 panLimit;
    private Vector3 moveForward = Vector3.forward;
    private Vector3 moveBack = Vector3.back;
    private Vector3 moveRight = Vector3.right;
    private Vector3 moveLeft = Vector3.left;

    [Header("Rotation")]
    //private bool rotationEnabled;
    private float cameraRotationY;
    private float easeFactor = 10f;
    private float mouseX;

    void Update()
    {
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            this.transform.Translate(moveForward * Time.deltaTime * panSpeed, Space.Self);
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            this.transform.Translate(moveBack * Time.deltaTime * panSpeed, Space.Self);
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            this.transform.Translate(moveRight * Time.deltaTime * panSpeed, Space.Self);
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            this.transform.Translate(moveLeft * Time.deltaTime * panSpeed, Space.Self);
        }

        //position.x = Mathf.Clamp(position.x, -panLimit.x, panLimit.x);
        //position.z = Mathf.Clamp(position.z, -panLimit.y, panLimit.y);

        if (Input.GetButton("Fire3"))
        {
            if (Input.mousePosition.x != mouseX)
            {
                cameraRotationY = (Input.mousePosition.x - mouseX) * easeFactor * Time.deltaTime;

                transform.Rotate(0, cameraRotationY, 0, Space.World);
            }
        }
    }

    private void LateUpdate()
    {
        mouseX = Input.mousePosition.x;
    }
}
