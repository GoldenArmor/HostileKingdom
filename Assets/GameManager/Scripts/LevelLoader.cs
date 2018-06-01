using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Player mouse;
	public StatsManager statsManager; 

    [Header("Camera")]
    public CameraController cameraController;
    public CameraRotation cameraRotation; 
    public CameraZoom cameraZoom;
}
