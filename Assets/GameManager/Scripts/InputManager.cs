using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("MouseInputsManager")]
    public MouseBehaviour mouse; //Coje el Script de MouseBehaviour para actualizar su comportamiento.
    private Vector3 formationPosition;

    private bool pauseBool = false; //Booleano que determina si el juego está en pausa o no. 

    void Update()
    {
        if (pauseBool == false) NoPaused(); //Si el juego no está pausado se ejecuta la función NoPaused().
        if (pauseBool == true) Paused(); // Si el juego está pausado se ejecuta la función Paused().
    }

    void Paused()
    {

    }

    void NoPaused()
    {
        if (Input.GetMouseButton(0)) mouse.MouseButtonPressed();
        if (Input.GetMouseButtonUp(0)) mouse.MouseButtonUp();
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                mouse.MultipleUnitSelection();
                return;
            }
            mouse.OneUnitSelection();
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (mouse.selectedUnit != null)
            {
                formationPosition = Vector3.zero;
                mouse.selectedUnit.ClickUpdate(formationPosition);
            }
            if (mouse.selectedUnits != null)
            {
                for (int i = 0; i < mouse.selectedUnits.Count; i++)
                {
                    if (i == 0) formationPosition = Vector3.zero;
                    if (i == 1) formationPosition = new Vector3(-4, 0, 0);
                    if (i == 2) formationPosition = new Vector3(4, 0, 0);
                    if (i == 3) formationPosition = new Vector3(0, 0, 4);

                    mouse.selectedUnits[i].ClickUpdate(formationPosition);
                }
            }
        }
    }
}
