using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class InputManager : MonoBehaviour
{
    [SerializeField]
    LevelLogic levelLogic;
    LevelLoader levelLoader = null;
    bool wasLoaded;

    [SerializeField]
    bool gamePause;

    [Header("MouseInputsManager")]
    [SerializeField]
    Mouse mouse = null;
    public static Vector3 mousePosition; 
    Vector3 formationPosition;

    [Header("CameraInputs")]
    [SerializeField]
    CameraController cameraController = null;
    [SerializeField]
    CameraRotation cameraRotation = null;
    [SerializeField]
    CameraZoom cameraZoom = null; 
    float scrollAxis;
    float rotateAxis;
    float mouseAxis; 
    Vector2 inputAxis;

    [Header("GodMode")]
    [SerializeField]
    bool isGodModeEnabled;
    [SerializeField]
    bool immunityEnabled;

    [Header("Skills")]
    [SerializeField]
    Hero hero = null;
    [SerializeField]
    Archer archer = null;
    [SerializeField]
    Mage mage = null;
    [SerializeField]
    Paladin paladin = null;

    void Update()
    {
        mousePosition = Input.mousePosition;
        if (GameObject.FindGameObjectWithTag("LevelLoader") != null)
        {
            if (!wasLoaded) SetGameValues();

            if (Input.GetKeyDown(KeyCode.Escape)) gamePause = !gamePause;
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

    void SetGameValues()
    {
        levelLoader = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<LevelLoader>();
        mouse = levelLoader.mouse;
        cameraController = levelLoader.cameraController;
        cameraRotation = levelLoader.cameraRotation;
        cameraZoom = levelLoader.cameraZoom;
        hero = levelLoader.hero;
        mage = levelLoader.mage;
        archer = levelLoader.archer;
        paladin = levelLoader.paladin; 
        wasLoaded = false;
    }

    void Paused()
    {
        
    }

    void NoPaused()
    {
        if (archer != null)
        {
            if (archer.isUpdatingCirclePosition && archer.isActiveAndEnabled)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    archer.isDoingSkill = true;
                    return;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    archer.isUpdatingCirclePosition = false;
                }
            }
        }

        mouse.mousePosition = mousePosition;

        #region CameraControllerAndZoom
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        rotateAxis = Input.GetAxis("Rotation");
        mouseAxis = Input.GetAxis("Mouse X");
        scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cameraController.SetInputAxis(inputAxis, mousePosition);
        cameraRotation.SetRotationAxis(rotateAxis);
        cameraZoom.SetAxis(scrollAxis);

        if (Input.GetButton("Fire3"))
        {
            cameraRotation.SetMouseRotationAxis(mouseAxis);
        }
        #endregion

        #region Selection&MovementBehaviours
        if (Input.GetMouseButton(0)) mouse.isDragging = true;
        if (Input.GetMouseButtonUp(0)) mouse.MouseButtonUp();
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                mouse.multipleUnitSelection = true;
            }
            mouse.ClickState();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (mouse.selectedUnit != null)
            {
                formationPosition = Vector3.zero;
                mouse.selectedUnit.ClickUpdate(formationPosition, mousePosition);
            }
            if (mouse.selectedUnits != null)
            {
                for (int i = 0; i < mouse.selectedUnits.Count; i++)
                {
                    if (i == 0) formationPosition = Vector3.zero;
                    if (i == 1) formationPosition = new Vector3(-4, 0, 0);
                    if (i == 2) formationPosition = new Vector3(4, 0, 0);
                    if (i == 3) formationPosition = new Vector3(0, 0, 4);

                    mouse.selectedUnits[i].ClickUpdate(formationPosition, mousePosition);
                }
            }
        }
        #endregion

        #region PlayableUnitSkills
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            hero.SkillUpdate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            archer.isUpdatingCirclePosition = true;
        }
        if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            mage.SkillUpdate();
        }
        if(Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            paladin.SkillUpdate();
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.M)) levelLogic.StartLoad(4);  
    }

    void GodModeUpdate()
    {
        mouse.mousePosition = mousePosition;
        if (Input.GetMouseButton(0)) mouse.isDragging = true;
        if (Input.GetMouseButtonUp(0)) mouse.MouseButtonUp();
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                mouse.multipleUnitSelection = true;
            }
            mouse.ClickState();
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (mouse.selectedUnit != null)
            {
                formationPosition = Vector3.zero;
                mouse.selectedUnit.GodUpdate(formationPosition, mousePosition);
            }
            if (mouse.selectedUnits != null)
            {
                for (int i = 0; i < mouse.selectedUnits.Count; i++)
                {
                    if (i == 0) formationPosition = Vector3.zero;
                    if (i == 1) formationPosition = new Vector3(-4, 0, 0);
                    if (i == 2) formationPosition = new Vector3(4, 0, 0);
                    if (i == 3) formationPosition = new Vector3(0, 0, 4);

                    mouse.selectedUnits[i].GodUpdate(formationPosition, mousePosition);
                }
            }
        }

        #region CameraControllerAndZoom
        inputAxis.x = Input.GetAxis("Horizontal");
        inputAxis.y = Input.GetAxis("Vertical");
        rotateAxis = Input.GetAxis("Rotation");
        mouseAxis = Input.GetAxis("Mouse X");
        scrollAxis = Input.GetAxis("Mouse ScrollWheel");

        cameraController.SetInputAxis(inputAxis, mousePosition);
        cameraRotation.SetRotationAxis(rotateAxis);
        cameraZoom.SetAxis(scrollAxis);

        if (Input.GetButton("Fire3"))
        {
            cameraRotation.SetMouseRotationAxis(mouseAxis);
        }
        #endregion

        #region enableMechanics

        if (Input.GetKeyDown(KeyCode.I))
        {
            immunityEnabled = !immunityEnabled;
            if (mouse.selectedUnit != null)
            {
                mouse.selectedUnit.godMode = immunityEnabled;
            }
            if (mouse.selectedUnits != null)
            {
                for (int i = 0; i < mouse.selectedUnits.Count; i++)
                {
                    mouse.selectedUnits[i].godMode = immunityEnabled;
                }
            }
        }
        #endregion
    }
}
