using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("MouseInputsManager")]
    public MouseBehaviour mouse; //Coje el Script de MouseBehaviour para actualizar su comportamiento.
    private Vector3 formationPosition;

    //public BetaMouseBehaviour mouse;
    //public Click mouse;

    private bool pauseBool = false; //Booleano que determina si el juego está en pausa o no. 

    void Start ()
    {
        //formationPosition = new Vector3(0, 0, 0); 
	}
	
	void Update ()
    {
        if (pauseBool == false) NoPaused(); //Si el juego no está pausado se ejecuta la función NoPaused().
        if (pauseBool == true) Paused(); // Si el juego está pausado se ejecuta la función Paused().
    }

    void Paused()
    {

    }

    void NoPaused()
    {       
        if (Input.GetMouseButton(0)) mouse.MouseButtonPressed(); //Si mantenemos el botón derecho del mouse ejecutamos la función MouseButtonPressed() de MouseBehaviour.
        if (Input.GetMouseButtonUp(0)) mouse.MouseButtonUp(); //Si dejamos de pulsar el botón derecho del mouse ejecutamos la función MouseButtonUp() de MouseBehaviour.
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) mouse.OneUnitSelection(); //Si pulsamos el botón izquierdo del Mouse y ninguna tecla shift, se ejecuta la función OneUnitSelection() de MouseBehaviour.
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) mouse.MultipleUnitSelection(); //Si pulsamos el botón izquierdo del Mouse + alguno de los 2 shifts se ejecuta la función MultipleUnitSelection() de MouseBehaviour.
        if (Input.GetMouseButtonDown(1)) //Si pulsamos el botón derecho del Mouse; 
        {
            if (mouse.selectedUnit != null)
            {
                formationPosition = new Vector3(0, 0, 0);
                mouse.selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().ClickUpdate(formationPosition); //Si tenemos una unidad seleccionada ejecutamos su función Movement();
            }
            if (mouse.selectedUnits != null) //Si tenemos varias unidades seleccionadas. 
            {
                for (int i = 0; i < mouse.selectedUnits.Count; i++) //Bucle for que se repetirá tantas veces como unidades seleccionadas tenga.
                {
                    if (i == 0) formationPosition = new Vector3(0, 0, 0);
                    if (i == 1) formationPosition = new Vector3(-4, 0, 0);
                    if (i == 2) formationPosition = new Vector3(4, 0, 0);
                    if (i == 3) formationPosition = new Vector3(0, 0, 4);
                    if (i == 4) formationPosition = new Vector3(0, 0, -4);
                    mouse.selectedUnits[i].gameObject.GetComponent<PlayableUnitBehaviour>().ClickUpdate(formationPosition); //Por cada unidad seleccionada ejecuto su función Movement().
                }
            }
        } 

        #region Pruebas
        /*
        mouse.isClicking = false;
        mouse.isHoldingDown = false;

        
        if (Input.GetMouseButtonDown(0)) mouse.StartDrag();
        if (Input.GetMouseButtonUp(0)) mouse.SelectDrag();
        if (Input.GetMouseButton(0)) mouse.IsHoldingDownMouseButton();

         
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift)) mouse.OneUnitSelection(); //Si pulsamos el botón izquierdo del Mouse y ninguna tecla shift, se ejecuta la función OneUnitSelection() de MouseBehaviour.
        if (Input.GetMouseButtonDown(0) && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))) mouse.MultipleUnitSelection(); //Si pulsamos el botón izquierdo del Mouse + alguno de los 2 shifts se ejecuta la función MultipleUnitSelection() de MouseBehaviour.
        if (Input.GetMouseButtonUp(0)) mouse.MouseButtonUp(); //Si dejamos de pulsar el botón derecho del mouse ejecutamos la función MouseButtonUp() de MouseBehaviour.
        */
        #endregion
    }
}