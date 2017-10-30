using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom")]
    public float scrollSpeed = 20f;
    public float minY = 20f;
    public float maxY = 120f;

    private float scroll;

    void Update()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        this.transform.Translate((Vector3.forward * scroll) * scrollSpeed * 100f * Time.deltaTime);
    }
}
