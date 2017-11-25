using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    Transform zoomCamTransform; 

    [Header("Zoom")]
    [SerializeField]
    float scrollSensitivity;
    [SerializeField]
    float zoomPosition;
    float scrollAxis;

    [SerializeField]
    LayerMask mask;
    float maxDistance;

    [Header("Limits")]
    [SerializeField]
    float maxHeight = 20f;
    [SerializeField]
    float minHeight = 120f;
    float heightDamp = 5f; 

    void Start()
    {
        zoomCamTransform = transform;
        maxDistance = minHeight; 
    }

    void Update()
    {
        ZoomUpdate();
    }

    void ZoomUpdate()
    {
        float distanceToGround = DistanceToGround();
        zoomPosition += scrollAxis * Time.deltaTime * scrollSensitivity;

        zoomPosition = Mathf.Clamp01(zoomPosition);

        float targetHeight = Mathf.Lerp(minHeight, maxHeight, zoomPosition);
        float difference = 0; 

        if (distanceToGround != targetHeight)
        {
            difference = targetHeight - distanceToGround;
        }

        zoomCamTransform.position = Vector3.Lerp(zoomCamTransform.position,
            new Vector3(zoomCamTransform.position.x, targetHeight + difference, zoomCamTransform.position.z),
            Time.deltaTime * heightDamp); 
    }

    float DistanceToGround()
    {
        Ray ray = new Ray(zoomCamTransform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore))
        {
            return (hit.point - zoomCamTransform.position).magnitude;
        }
        else return 0;
    }

    public void SetAxis(float axis)
    {
        scrollAxis = axis; 
    }
}
