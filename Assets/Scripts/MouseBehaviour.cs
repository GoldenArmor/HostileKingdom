using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image selectionBox;
    private Vector2 selectionBoxOrigin;
    private Rect selectionRect;

    void Update()
    {
        if (isDragging) //Si estamos pulsando y arrastrando el ratón.
        {
            if (Input.mousePosition.x < selectionBoxOrigin.x)
            {
                selectionRect.xMin = Input.mousePosition.x;
                selectionRect.xMax = selectionBoxOrigin.x;
            }
            else
            {
                selectionRect.xMin = selectionBoxOrigin.x;
                selectionRect.xMax = Input.mousePosition.x;
            }

            if (Input.mousePosition.y < selectionBoxOrigin.y)
            {
                selectionRect.yMin = Input.mousePosition.y;
                selectionRect.yMax = selectionBoxOrigin.y;
            }
            else
            {
                selectionRect.yMin = selectionBoxOrigin.y;
                selectionRect.yMax = Input.mousePosition.y;
            }

            selectionBox.rectTransform.offsetMin = selectionRect.min;
            selectionBox.rectTransform.offsetMax = selectionRect.max;

            selectionBox.gameObject.SetActive(true);
        }

        if (selectedUnit != null)
        {
            selectedUnit.gameObject.GetComponent<Characters>().isSelected = true;
        }
        if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].gameObject.GetComponent<Characters>().isSelected = true;
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
        selectionBox.gameObject.SetActive(false);
        isDragging = false;
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
        if ((unitScreenPosition.x > selectionBoxOrigin.x && unitScreenPosition.y < selectionBoxOrigin.y) && (unitScreenPosition.x < selectionRect.x && unitScreenPosition.y > selectionRect.y))
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
        if (selectionRect.x - 1 >= selectionBoxOrigin.x || selectionRect.y - 1 >= selectionBoxOrigin.y ||
            selectionRect.x < selectionBoxOrigin.x - 1 || selectionRect.y < selectionBoxOrigin.y - 1) return true;
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
                    //Debug.LogError("Unit Selected"); //Se debuguea un error que nos informa de dicha colisión. 
                    selectedUnit = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].gameObject.GetComponent<Characters>().isSelected = false;
                    }
                    selectedUnits.Clear();
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    for (int i = 0; i < selectedUnits.Count; i++)
                    {
                        selectedUnits[i].gameObject.GetComponent<Characters>().isSelected = false;
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
                    selectedUnit.gameObject.GetComponent<Characters>().isSelected = false;
                    selectedUnit = null; //Se deselecciona la actual unidad seleccionada.
                    Debug.LogError("Unit Selected"); //Se debuguea un error que nos informa de dicha colisión.
                    selectedUnit = hit.transform.gameObject; //La unidad seleccionada (selectedUnit) pasa a ser el GameObject de dicha colisión.
                }
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground")) //Si el objeto con el que el rayo ha impactado está en la capa de "Unit".
                {
                    selectedUnit.gameObject.GetComponent<Characters>().isSelected = false;
                    selectedUnit = null; //Se deselecciona la actual unidad seleccionada.
                }
            }
        }

        //selectionBoxOrigin = new Vector2(Camera.main.ScreenToViewportPoint(Input.mousePosition).x, Camera.main.ScreenToViewportPoint(Input.mousePosition).y);
        selectionBoxOrigin = Input.mousePosition;
        selectionRect = new Rect();
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
                    selectedUnit.gameObject.GetComponent<Characters>().isSelected = false;
                    selectedUnit = null; //Vaciamos la selección individual de unidad porqué queremos seleccionar más de una.
                }
                selectedUnits.Add(hit.transform.gameObject); //Se añade el GameObject con el que el rayo ha chocado a la Lista.
            }
        }
    }
}
