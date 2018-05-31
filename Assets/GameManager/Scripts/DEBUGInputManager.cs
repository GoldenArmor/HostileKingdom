using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUGInputManager : MonoBehaviour
{
    LevelLoader levelLoader;
    bool wasLoaded = true;

    [SerializeField]
    bool gamePause;
    [SerializeField]
    StatsManager statsManager; 

    [Header("MouseInputsManager")]
    [SerializeField]
    Player mouse;
    public static Vector3 mousePosition;

    [Header("CameraInputs")]
    [SerializeField]
    CameraController cameraController;
    [SerializeField]
    CameraRotation cameraRotation; 
    [SerializeField]
    CameraZoom cameraZoom;
    float scrollAxis;
    float rotationAxis;
    float mouseAxis;
    Vector2 inputAxis;

    [Header("GodMode")]
    [SerializeField]
    bool isGodModeEnabled;

    void Update()
    {
        mousePosition = Input.mousePosition;
        if (wasLoaded)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                gamePause = !gamePause;
                if (gamePause == false)
                {
                    statsManager.UnPaused();
                }
            }

            if (Input.GetKeyDown(KeyCode.F10))
            {
                isGodModeEnabled = !isGodModeEnabled;
                Debug.Log("GOD Mode enabled");
            }
            if (isGodModeEnabled)
            {
                GodModeUpdate();
                return;
            }

            if (gamePause == true)
            {
                statsManager.Paused();
                Time.timeScale = 0.0f;
                Paused();
            }
            else
            {
                Time.timeScale = 1.0f;
                NoPaused();
            }
        }
    }

    public void SearchLevelLoader()
    {
        if (GameObject.FindGameObjectWithTag("LevelLoader") != null)
        {
            if (!wasLoaded) SetGameValues();
        }
    }

    void SetGameValues()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        mouse = levelLoader.mouse;
        cameraController = levelLoader.cameraController;
        cameraRotation = levelLoader.cameraRotation; 
        cameraZoom = levelLoader.cameraZoom;
        wasLoaded = true;
    }

    void Paused()
    {

    }

    void NoPaused()
    {
        mouse.SetMousePosition(mousePosition); 

        #region CameraControllerAndZoom
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        rotationAxis = Input.GetAxis("Rotation");
        mouseAxis = Input.GetAxis("Mouse X");
        scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cameraController.SetInputAxis(inputAxis, mousePosition);
        cameraRotation.SetRotationAxis(rotationAxis);
        cameraZoom.SetAxis(scrollAxis);

        if (Input.GetButton("Fire3"))
        {
            cameraRotation.SetMouseRotationAxis(mouseAxis); 
        }
        #endregion

        #region Selection&MovementBehaviours
        if (Input.GetMouseButtonDown(0))
        {
            mouse.ClickState();
        }
        #endregion
    }

    void GodModeUpdate()
    {
        mouse.SetMousePosition(mousePosition); 

        if (Input.GetMouseButtonDown(0))
        {
            mouse.ClickState();
        }

        #region CameraControllerAndZoom
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        rotationAxis = Input.GetAxis("Rotation");
        mouseAxis = Input.GetAxis("Mouse X");
        scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cameraController.SetInputAxis(inputAxis, mousePosition);
        cameraRotation.SetRotationAxis(rotationAxis);
        cameraZoom.SetAxis(scrollAxis);

        if (Input.GetButton("Fire3"))
        {
            cameraRotation.SetMouseRotationAxis(mouseAxis);
        }
        #endregion
    }
}
