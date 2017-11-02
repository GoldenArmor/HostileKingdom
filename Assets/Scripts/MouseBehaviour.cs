using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBehaviour : MonoBehaviour
{
    [Header("Unit Selection")]
    public GameObject selectedUnit; //GameObject seleccionado para actualizar.
    public List<GameObject> selectedUnits = new List<GameObject>(); //Lista de GameObjects para una selección multiple.
    public List<GameObject> unitsOnScreenSpace = new List<GameObject>(); //Lista de GameObjects para saber cuantas hay en pantalla.
    public LayerMask mask; //Máscara que se aplica al rayo para detectar una capa determinada de objetos. 
    private RaycastHit hit; //Creamos un RaycastHit que nos devolverá la información del objeto con el que el rayo colisiona.
    private float maxDistance = Mathf.Infinity; //Máxima distancia que puede recorrer el rayo lanzado des de la cámara. 

    [Header("Drag Selection")]
    public List<GameObject> unitsInDrag = new List<GameObject>(); //Lista de GameObjects para una selección de click y arrastrar.
    public bool isDragging; //Comprobar si estamos pulsando y arrastrando el ratón. 
    private Vector3 originDragPoint; //Punto de origen de la selección rectangular. 
    private Vector3 currentDragPoint; //El punto en que el ratón se encuentra actualmente.  
    private Vector2 boxStart; //El punto donde empieza la selección rectangular.
    private Vector2 boxEnd; //El punto donde termina la selección rectangular.
    private float boxWidth; //La anchura del rectángulo que generamos.
    private float boxHeight; //La altura del rectángulo que generamos
    private float boxLeft; //Posición en X del rectángulo que generamos.
    private float boxTop; //Posición en Y del rectángulo que generamos. 
    public RectTransform selectionBox;
    //private Vector2 selectionBoxOrigin;
    private int i = 0;

    void OnGUI()
    {
        if (isDragging) //Si estamos pulsando y arrastrando el ratón.
        {
            GUI.Box(new Rect(boxLeft, boxTop, boxWidth, boxHeight), ""); //Generamos un rectángulo en pantalla con los parámetros establecidos. 
        }
    }

    void DrawSelectionBox()
    {
        if (i == 0)
        {
            selectionBox.anchoredPosition = new Vector2(boxLeft, boxTop);
            i++;
        }
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(boxWidth), Mathf.Abs(boxHeight));
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo.

        if (Physics.Raycast(ray, out hit, maxDistance)) //Si el rayo colisiona con algo.
        {
            currentDragPoint = hit.point; //El punto de impacto es la posición actual del ratón. 
        }

        if (isDragging) //Si estamos pulsando y arrastrando el ratón.
        {
            boxWidth = Camera.main.WorldToScreenPoint(originDragPoint).x - Camera.main.WorldToScreenPoint(currentDragPoint).x; //La anchura de la selección viene dada por el punto de origenX - el finalX.
            boxHeight = Camera.main.WorldToScreenPoint(originDragPoint).y - Camera.main.WorldToScreenPoint(currentDragPoint).y; //La altura de la selección viene dada por el punto de origenY - el finalY.
            boxLeft = Input.mousePosition.x; //La posición en X del rectángulo viene dada por la posición en X del ratón.
            boxTop = (Screen.height - Input.mousePosition.y) - boxHeight; //La posición en X del rectángulo viene dada por la (altura de la pantalla de juego - la posición del ratón en Y) - la altura del rectángulo. 

            if (boxWidth > 0f && boxHeight < 0f) //Si la anchura es mayor que 0 y la altura menor de 0.
            {
                boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y); //El punto dónde empieza a dibujarse el rectángulo viene dado por la posición en X y en Y del ratón.
            }
            else if (boxWidth > 0f && boxHeight > 0f) //Si la anchura es mayor que 0 y la altura mayor de 0.
            {
                boxStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y + boxHeight); //El punto dónde empieza a dibujarse el rectángulo viene dado por la posición en X y en (Y + su altura) del ratón.
            }
            else if (boxWidth < 0f && boxHeight < 0f) //Si la anchura es menor que 0 y la altura menor de 0.
            {
                boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y); //El punto dónde empieza a dibujarse el rectángulo viene dado por la posición en (X + su anchura) y en Y del ratón.
            }
            else if (boxWidth < 0f && boxHeight > 0f) //Si la anchura es menor que 0 y la altura mayor de 0.
            {
                boxStart = new Vector2(Input.mousePosition.x + boxWidth, Input.mousePosition.y + boxHeight); //El punto dónde empieza a dibujarse el rectángulo viene dado por la posición en (X + su anchura) y en (Y + su altura) del ratón.
            }
            boxEnd = new Vector2(boxStart.x + Mathf.Abs(boxWidth), boxStart.y - Mathf.Abs(boxHeight)); //El punto dónde termina de dibujarse el rectángulo viene dado por (el comienzo en X + el módulo de su anchura) y (el comienzo en Y - el módulo de su altura).

            DrawSelectionBox();
        }
        else i = 0;

        if (selectedUnit != null)
        {
            selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = true; 
        }
        if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = true;
            }
        }
    }

    void LateUpdate()
    {
        unitsInDrag.Clear();
        if (isDragging && unitsOnScreenSpace.Count > 0)
        {
            selectedUnit = null;
            for (int i = 0; i < unitsOnScreenSpace.Count; i++)
            {
                GameObject unitObject = unitsOnScreenSpace[i] as GameObject;
                PlayableUnitBehaviour positionScript = unitObject.transform.GetComponent<PlayableUnitBehaviour>();
                if (!unitsInDrag.Contains(unitObject))
                {
                    if (UnitWithinDrag(positionScript.screenPosition))
                    {
                        unitsInDrag.Add(unitObject);
                    }
                }
            }
        }
    }

    #region DragSelection
    public void MouseButtonPressed() //Función que se ejecuta cuando estamos presionando el ratón (botón derecho).
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo.

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
        {
            if (hit.transform.gameObject.layer != LayerMask.NameToLayer("PlayableUnit")) //Si el objeto con el que el rayo ha impactado no está en la capa de "Unit".
            {
                if (CheckMouseDrag())
                {
                    isDragging = true;
                }
            }
        }
    }

    public void MouseButtonUp() //Función que se ejecuta cuando dejamos de pulsar el ratón (botón derecho).
    {
        SelectUnitsInDrag();
        isDragging = false;
        selectionBox.sizeDelta = Vector2.zero;
        selectionBox.anchoredPosition = Vector2.zero;
    }

    public bool UnitWithinScreenSpace(Vector2 unitScreenPosition)
    {
        if ((unitScreenPosition.x < Screen.width && unitScreenPosition.y < Screen.height) && (unitScreenPosition.x > 0f && unitScreenPosition.y > 0f))
            return true;
        else
            return false;
    }

    public bool UnitWithinDrag(Vector2 unitScreenPosition)
    {
        if ((unitScreenPosition.x > boxStart.x && unitScreenPosition.y < boxStart.y) && (unitScreenPosition.x < boxEnd.x && unitScreenPosition.y > boxEnd.y))
            return true;
        else
            return false;
    }

    public void SelectUnitsInDrag()
    {
        if (unitsInDrag.Count > 0)
        {
            for (int i = 0; i < unitsInDrag.Count; i++)
            {
                if (!selectedUnits.Contains(unitsInDrag[i]))
                {
                    selectedUnits.Add(unitsInDrag[i]);
                }
            }
        }
        unitsInDrag.Clear();
    }

    private bool CheckMouseDrag() //Función que determina si estamos arrastrando el ratón o no.
    {
        if (currentDragPoint.x - 1 >= originDragPoint.x || currentDragPoint.y - 1 >= originDragPoint.y || currentDragPoint.z - 1 >= originDragPoint.z ||
            currentDragPoint.x < originDragPoint.x - 1 || currentDragPoint.y < originDragPoint.y - 1 || currentDragPoint.z < originDragPoint.z - 1) return true;
        else return false;  //Si ninguna de las condiciones anteriores es cierta, el ratón no se está arrastrando y retornamos el valor falso. 
    }
    #endregion

    public void OneUnitSelection() //Función que se ejecuta al hacer Click con el botón izquierdo del Mouse. 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo. 

        if (selectedUnit == null) //Si no hay ninguna unidad seleccionada.
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
            {
                Debug.Log(hit.transform.name); //Hace un debug del nombre del objeto con el que el rayo ha colisionado.
                Debug.DrawLine(ray.origin, hit.point, Color.blue, 3); //Se dibuja una línia azul en el trazo del rayo. 

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    Debug.LogError("Unit Selected"); //Se debuguea un error que nos informa de dicha colisión. 
                    selectedUnit = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = false;
                    }
                    selectedUnits.Clear(); 
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = false;
                    }
                    selectedUnit = null;
                    selectedUnits.Clear();
                }
            }
        }
        else//En cualquier otro caso.
        {
            if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
            {
                Debug.Log(hit.transform.name); //Hace un debug del nombre del objeto con el que el rayo ha colisionado.
                Debug.DrawLine(ray.origin, hit.point, Color.blue, 3); //Se dibuja una línia azul en el trazo del rayo. 

                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = false;
                    selectedUnit = null; //Se deselecciona la actual unidad seleccionada.
                    Debug.LogError("Unit Selected"); //Se debuguea un error que nos informa de dicha colisión.
                    selectedUnit = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = false;
                    selectedUnit = null; //Se deselecciona la actual unidad seleccionada.
                }
            }
        }
        if (Physics.Raycast(ray, out hit, maxDistance)) //Si el rayo colisiona con algo.
        {
            originDragPoint = hit.point; //El punto en que se origina el rectángulo es el punto dónde impacta el rayo lanzado por el ratón.
            //selectionBoxOrigin = new Vector2(Camera.main.ScreenToViewportPoint(Input.mousePosition).x, Camera.main.ScreenToViewportPoint(Input.mousePosition).y);    
        }
        /*if (i == 0)
        {
            selectionBox.anchoredPosition = new Vector2(boxLeft, boxTop);
            i++;
        }*/
    }

    public void MultipleUnitSelection() //Función que se ejecuta al hacer Click con el botón izquierdo del Mouse + Shift.
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creamos un rayo que va a la dirección del Mouse desde la cámara y la traducimos a una posición en el mundo.

        if (Physics.Raycast(ray, out hit, maxDistance, mask, QueryTriggerInteraction.Ignore)) //Si el rayo colisiona con algo.
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("PlayableUnit")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit"
            {
                if (selectedUnit != null) //Si ya hay una unidad seleccionada.
                {
                    selectedUnits.Add(selectedUnit); //Se añade la unidad seleccionada a la lista.
                    selectedUnit.gameObject.GetComponent<PlayableUnitBehaviour>().isSelected = false;
                    selectedUnit = null; //Vaciamos la selección individual de unidad porqué queremos seleccionar más de una.
                }
                selectedUnits.Add(hit.transform.gameObject); //Se añade el GameObject con el que el rayo ha chocado a la Lista.
            }
        }
    }
}
