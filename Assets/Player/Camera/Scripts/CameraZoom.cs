using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour
{
    Transform selfTransform; 

    Transform m_focalPoint;
    Vector3 m_iniPosition;
    Quaternion m_iniRotation;

    Vector3 m_cameraForward;
    Vector3 m_mouseOrigin;
    
    [Header("Scrolling")]
    [SerializeField]
    float ZoomSpeed;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;
    float m_scrollAxis;
    float mouseAxis; 
    
    void Start()
    {
        selfTransform = transform; 
        m_focalPoint = transform.parent;

        m_iniPosition = m_focalPoint.position;
        m_iniRotation = m_focalPoint.rotation;
    }
	void Update ()
    {
        if(selfTransform.position.y <= 18)
        {
            //selfTransform.position = new Vector3(selfTransform.position.x, 18, selfTransform.position.z);
            //selfTransform.position = selfTransform.position;
        }

        m_cameraForward = transform.forward;

        m_scrollAxis = mouseAxis * ZoomSpeed;
        if(m_scrollAxis != 0)
        {
            if(selfTransform.position.y > minY && selfTransform.position.y < maxY)
            {
                Scrolling();
                return; 
            }

            if(selfTransform.position.y <= minY && m_scrollAxis < 0)
            {
                Scrolling();
            }

            if(selfTransform.position.y >= maxY && m_scrollAxis > 0)
            {
                Scrolling();
            }
        }
    }

    void Scrolling()
    {
        transform.Translate(m_cameraForward.x * m_scrollAxis, m_cameraForward.y * m_scrollAxis, m_cameraForward.z * m_scrollAxis, Space.World);
    }

    public void SetAxis(float axis)
    {
        mouseAxis = axis; 
    }
}
