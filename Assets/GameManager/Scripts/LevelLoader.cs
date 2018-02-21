using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public Mouse mouse;

    public CameraController cameraController;
    public CameraRotation cameraRotation; 
    public CameraZoom cameraZoom;

    [Header("Skills")]
    public Hero hero;
    public Mage mage;
    public Archer archer;
    public Paladin paladin;
}
